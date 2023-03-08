using System.Collections.Concurrent;
using System.Text;
using Kyle.Infrastructure.Events;
using Kyle.Infrastructure.Events.Bus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Kyle.Infrastructure.RabbitMQExtensions;

public class ApplicationMessageConsumer//:IDisposable
{
    protected IConnectionPool ConnectionPool { get; }
    private ConcurrentDictionary<string, IModel> Channels { get; }
    private Dictionary<string, bool> QueueAutoAck = new Dictionary<string, bool>();
    private readonly RabbitMQMessageSerializer _rabbitMqMessageSerializer;

    public IEventBus EventBus { get; set; }

    public ILogger Logger { get; set; }

    public ApplicationMessageConsumer(IConnectionPool connectionPool, RabbitMQMessageSerializer rabbitMqMessageSerializer
        , ILoggerFactory logger)
    {
        EventBus = new EventBus();
        _rabbitMqMessageSerializer = rabbitMqMessageSerializer;
        ConnectionPool = connectionPool;
        Logger = logger.CreateLogger<ApplicationMessageConsumer>();
        Channels = new ConcurrentDictionary<string, IModel>();
    }

    public void Initialize(MallRabbitMQConsumerOptions options)
    {
        var connection = ConnectionPool.Get(options.ConnectionName);

        if (options.ExchangeDeclare != null)
        {
            foreach (var exchangeDeclare in options.ExchangeDeclare)
            {
                var queueName = string.Empty;

                if (exchangeDeclare.ExchangeType != "fanout")
                {

                }

                var channel = Channels.GetOrAdd(exchangeDeclare.ExchangeName, _ => connection.CreateModel());
                channel = connection.CreateModel();
                if (options.BasicQos != null)
                {
                    channel.BasicQos(options.BasicQos.PrefetchSize, options.BasicQos.PrefetchCount, options.BasicQos.Global);
                }

                channel.ExchangeDeclare(exchange: exchangeDeclare.ExchangeName, type: exchangeDeclare.ExchangeType);

                if (string.IsNullOrWhiteSpace(exchangeDeclare.QueueNameSuffix))
                {
                    queueName = channel.QueueDeclare().QueueName;
                }
                else
                {
                    queueName = channel
                        .QueueDeclare(queue: $"E-{exchangeDeclare.ExchangeName}-{exchangeDeclare.QueueNameSuffix}")
                        .QueueName;
                }

                if (exchangeDeclare.ExchangeType == "fanout")
                {
                    channel.QueueBind(queue: queueName, exchange: exchangeDeclare.ExchangeName, routingKey: string.Empty);
                }
                else
                {
                    foreach (var routingKey in exchangeDeclare.RoutingKey)
                    {
                        channel.QueueBind(queue: queueName, exchange: exchangeDeclare.ExchangeName, routingKey: routingKey);
                    }
                }

                SetConsumer(queueName, exchangeDeclare.AutoAck, exchangeDeclare.BasicRecover, channel);

            }
        }

        if (options.QueueDeclare != null)
        {
            foreach (var queueDeclare in options.QueueDeclare)
            {
                var channel = Channels.GetOrAdd(queueDeclare.QueueName, _ => connection.CreateModel());
                channel = connection.CreateModel();
                if (options.BasicQos != null)
                {
                    channel.BasicQos(options.BasicQos.PrefetchSize, options.BasicQos.PrefetchCount, options.BasicQos.Global);
                }

                var queueName = channel.QueueDeclare(queue: $"Q-{queueDeclare.QueueName}", durable: options.Durable,
                    exclusive: queueDeclare.Exclusive, autoDelete: false, arguments: queueDeclare.Arguments).QueueName;

                SetConsumer(queueName, queueDeclare.AutoAck, queueDeclare.BasicRecover, channel);
            }
        }
        Logger.LogInformation("ApplicationMessageConsumer Initialized");
    }

    private void SetConsumer(string queueName, bool autoAck, MallRabbitMQConsumerOptions.BasicRecoverOptions options, IModel channel)
    {
        QueueAutoAck.Add(queueName, autoAck);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = _rabbitMqMessageSerializer.DeserializeObject(body);
                var routingKey = ea.RoutingKey;

                Logger.LogInformation(
                    $"[x] Received:{routingKey}:{message}, DeliveryTag:{ea.DeliveryTag}, ConsumerTag:{ea.ConsumerTag}, Redelivered:{ea.Redelivered}, Exchange:{ea.Exchange}");

                var json = Encoding.UTF8.GetString(message.Body);
                Logger.LogInformation($"[x] Received Message {message.Tag}:{json}");

                // TODO:EventBus
                var type = EventBus.GetType(message.Tag);
                var obj = JsonConvert.DeserializeObject(json, type);

                EventBus.TriggerAsycn(type, obj as IEventData)
                .ContinueWith(x =>
                {
                    if (x.Exception == null)
                    {
                        if (!QueueAutoAck[queueName])
                        {
                            CompleteHandle(channel, ea);
                        }
                    }
                    else if (x.Exception.InnerException is IOException)
                    {
                        IOExceptionHandle(QueueAutoAck[queueName], options, ea, x.Exception.InnerException, channel, $"{message?.Tag} {json}");
                    }
                    else if (x.Exception.InnerException.InnerException is IOException)
                    {
                        IOExceptionHandle(QueueAutoAck[queueName], options, ea, x.Exception.InnerException.InnerException, channel, $"{message?.Tag} {json}");
                    }
                    else
                    {
                        ExceptionHandle(QueueAutoAck[queueName], ea, x.Exception, channel, $"{message?.Tag} {json}");
                    }
                });
            }
            catch (IOException e)
            {
                IOExceptionHandle(QueueAutoAck[queueName], options, ea, e, channel);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is IOException)
                    {

                        IOExceptionHandle(QueueAutoAck[queueName], options, ea, ex, channel);
                        return;
                    }
                }

                ExceptionHandle(QueueAutoAck[queueName], ea, ex, channel);
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: QueueAutoAck[queueName], consumer);

    }

    private void CompleteHandle(IModel channel, BasicDeliverEventArgs ea)
    {
        try
        {
            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "CompleteHandle");
        }
    }

    private void ExceptionHandle(bool autoAck, BasicDeliverEventArgs ea, Exception ex, IModel channel, string msg = null)
    {
        Logger.LogError(ex, $"Received Trigger {msg}");
        if (!autoAck)
        {
            try
            {
                channel.BasicReject(ea.DeliveryTag, false);
                Logger.LogDebug("BasicReject.Requeue:{0}", false);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "ExceptionHandle");
            }
        }
    }

    private void IOExceptionHandle(bool autoAck, MallRabbitMQConsumerOptions.BasicRecoverOptions basicRecoverOptions, BasicDeliverEventArgs ea, Exception ex, IModel channel, string msg = null)
    {
        Logger.LogError(ex, $"Received Trigger IOException {msg}");
        Thread.Sleep(1000);
        if (!autoAck)
        {
            try
            {
                if (basicRecoverOptions != null)
                {
                    channel.BasicRecover(basicRecoverOptions.Requeue);
                    Logger.LogDebug("BasicRecover.Requeue:{0}", basicRecoverOptions.Requeue);
                }
                else
                {
                    channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);// 重新入队
                    Logger.LogDebug("BasicReject.Requeue:{0}", true);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, "IOExceptionHandle");
            }
        }
    }

}
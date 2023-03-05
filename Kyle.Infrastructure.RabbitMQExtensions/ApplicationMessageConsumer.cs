using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Kyle.Infrastructure.RabbitMQExtensions;

public class ApplicationMessageConsumer//:IDisposable
{
    protected IConnectionPool ConnectionPool { get; }
    private ConcurrentDictionary<string,IModel> Channels { get; }
    private Dictionary<string, bool> QueueAutoAck = new Dictionary<string, bool>();
    private readonly RabbitMQMessageSerializer _rabbitMqMessageSerializer;
    public ILogger Logger { get; set; }

    public ApplicationMessageConsumer(IConnectionPool connectionPool,RabbitMQMessageSerializer rabbitMqMessageSerializer, ILogger<ApplicationMessageConsumer> logger)
    {
        _rabbitMqMessageSerializer = rabbitMqMessageSerializer;
        ConnectionPool = connectionPool;
        Logger = logger;
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
                    channel.BasicQos(options.BasicQos.PrefetchSize,options.BasicQos.PrefetchCount,options.BasicQos.Global);
                }
                
                channel.ExchangeDeclare(exchange:exchangeDeclare.ExchangeName,type:exchangeDeclare.ExchangeType);

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
                    channel.QueueBind(queue:queueName,exchange:exchangeDeclare.ExchangeName,routingKey:string.Empty);
                }
                else
                {
                    foreach (var routingKey in exchangeDeclare.RoutingKey)
                    {
                        channel.QueueBind(queue:queueName,exchange:exchangeDeclare.ExchangeName,routingKey:routingKey);
                    }
                }
                
                SetConsumer(queueName,exchangeDeclare.AutoAck,exchangeDeclare.BasicRecover,channel);
                
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
                    channel.BasicQos(options.BasicQos.PrefetchSize,options.BasicQos.PrefetchCount,options.BasicQos.Global);
                }

                var queueName = channel.QueueDeclare(queue: $"Q-{queueDeclare.QueueName}", durable: options.Durable,
                    exclusive: queueDeclare.Exclusive, autoDelete: false, arguments: queueDeclare.Arguments).QueueName;
                
                SetConsumer(queueName,queueDeclare.AutoAck,queueDeclare.BasicRecover,channel);
            }
        }
        Logger.LogInformation("ApplicationMessageConsumer Initialized");
    }

    private void SetConsumer(string queueName, bool autoAck, MallRabbitMQConsumerOptions.BasicRecoverOptions options,IModel channel)
    {
        QueueAutoAck.Add(queueName,autoAck);
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

                if (!QueueAutoAck[queueName])
                {
                    CompleteHandle(channel,ea);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: QueueAutoAck[queueName], consumer);

    }

    private void CompleteHandle(IModel channel, BasicDeliverEventArgs ea)
    {
        try
        {
            channel.BasicAck(deliveryTag:ea.DeliveryTag,multiple:false);
        }
        catch (Exception e)
        {
            Logger.LogError(e,"CompleteHandle");
        }
    }
    
    
}
using Kyle.Infrastructure.RabbitMQExtensions.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Kyle.Infrastructure.RabbitMQExtensions;

public class ApplicationMessagePublisher : IMessagePublisher<IApplicationMessage>, IDisposable
{
    protected IConnectionPool ConnectionPool { get; }
    private MallRabbitMQPublisherOptions _options;
    private readonly RabbitMQMessageSerializer _rabbitMqMessageSerializer;
    public ILogger Logger { get; set; }

    protected Dictionary<string, MallRabbitMQPublisherOptions.ExchangeDeclareOptions> ExchangeDeclareDic;
    protected Dictionary<string, MallRabbitMQPublisherOptions.QueueDeclareOptions> QueueDeclareDic;

    public ApplicationMessagePublisher(RabbitMQMessageSerializer rabbitMqMessageSerializer,
         IConnectionPool connectionPool, ILoggerFactory logger)
    {
        _rabbitMqMessageSerializer = rabbitMqMessageSerializer;
        //_options = options;
        ConnectionPool = connectionPool;
        Logger = logger.CreateLogger<ApplicationMessagePublisher>();



    }

    public void Initalize(MallRabbitMQPublisherOptions options)
    {
        ExchangeDeclareDic = options.ExchangeDeclare?
           .ToDictionary(k => k.ExchangeName, v => v);

        QueueDeclareDic = options.QueueDeclare
            .ToDictionary(k => k.QueueName, v => v);

        this._options = options;
    }


    public void Dispose()
    {

    }

    public async Task<AsyncTaskResult> PublishAsync(IApplicationMessage msg)
    {
        var message = CreateRabbitMQMessage(msg);
        return SendMessageAsync(message);
    }
    public AsyncTaskResult Publish(IApplicationMessage msg)
    {

        var message = CreateRabbitMQMessage(msg);
        return SendMessageAsync(message);
    }

    public AsyncTaskResult SendMessageAsync(RabbitMQMessage message)
    {
        var find = FindQueue(message);
        return SendMessageAsync(message, find.ExchangeName, find.QueueName);
    }

    //public AsyncTaskResult SendMessageAsync(RabbitMQMessage message, string exchangeName, string queueName)
    //{

    //}

    public (string ExchangeName, string QueueName) FindQueue(RabbitMQMessage message)
    {
        dynamic find = null;
        if (_options.ExchangeDeclare != null && (find = _options.ExchangeDeclare.FirstOrDefault(x => x.Tag.Contains(message.Tag))) != null)
        {
            return ((string)find.ExchangeName, null);
        }
        else if (_options.QueueDeclare != null && (find = _options.QueueDeclare.FirstOrDefault(x => x.Tag.Contains(message.Tag))) != null)
        {
            return (null, (string)find.QueueName);
        }
        else
        {
            throw new Exception($"未找到 Tag:{message.Tag} 对应的TagRoute");
        }

    }

    public AsyncTaskResult SendMessageAsync(RabbitMQMessage message, string exchangeName, string queueName)
    {
        var body = _rabbitMqMessageSerializer.SerializeObject(message);
        try
        {
            if (exchangeName != null)
            {
                using (var channel = GetExchangeDeclareChannel(exchangeName))
                {
                    channel.BasicPublish(exchange: exchangeName, routingKey: message.Topic, body: body);
                }
            }
            else if (queueName != null)
            {
                using (var channel = GetQueueDeclareChannel(queueName))
                {
                    channel.BasicPublish(exchange: string.Empty, routingKey: $"Q-{queueName}", body: body);
                }
            }
            else
            {
                Logger.LogError("未找到 Tag:{0} 对应TagRoute", message.Tag);
                return new AsyncTaskResult(AsyncTaskStatus.Failed, $"未找到 Tag:{message.Tag} 对应的TagRoute");
            }

            Logger.LogInformation($" [x] Sent {message}");
            return AsyncTaskResult.Success;
        }
        catch (Exception e)
        {
            Logger.LogError(e, $"消息发送失败: {message}");
            return new AsyncTaskResult(AsyncTaskStatus.IOException, e.Message);
        }
    }

    private IModel GetExchangeDeclareChannel(string name)
    {
        var exchangeDeclare = ExchangeDeclareDic[name];
        var channel = ConnectionPool.Get(_options.ConnectionName).CreateModel();
        channel.ExchangeDeclare(exchange: exchangeDeclare.ExchangeName, type: exchangeDeclare.ExchangeType,
            durable: _options.Durable, autoDelete: false, arguments: exchangeDeclare.Arguments);
        return channel;
    }

    private IModel GetQueueDeclareChannel(string name)
    {
        var queueDeclare = QueueDeclareDic[name];
        var channel = ConnectionPool.Get().CreateModel();
        channel.QueueDeclare(queue: $"Q-{queueDeclare.QueueName}", durable: _options.Durable,
            exclusive: queueDeclare.Exclusive, autoDelete: false, arguments: queueDeclare.Arguments);
        return channel;
    }

    public RabbitMQMessage CreateRabbitMQMessage(IApplicationMessage message)
    {
        var topic = message.GetRoutingKey() ?? string.Empty;
        var data = JsonConvert.SerializeObject(message);
        return new RabbitMQMessage(topic, 4, Encoding.UTF8.GetBytes(data), message.GetType().FullName);
    }
}
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Kyle.Infrastructure.RabbitMQExtensions;

public class ApplicationMessagePublisher
{
    protected IConnectionPool ConnectionPool { get; }
    private MallRabbitMQPublisherOptions _options;
    private readonly RabbitMQMessageSerializer _rabbitMqMessageSerializer;
    public ILogger Logger { get; set; }

    protected Dictionary<string, MallRabbitMQPublisherOptions.ExchangeDeclareOptions> ExchangeDeclareDic;
    protected Dictionary<string, MallRabbitMQPublisherOptions.QueueDeclareOptions> QueueDeclareDic;

    public ApplicationMessagePublisher(RabbitMQMessageSerializer rabbitMqMessageSerializer,
        MallRabbitMQPublisherOptions options,IConnectionPool connectionPool, ILogger<ApplicationMessagePublisher> logger)
    {
        _rabbitMqMessageSerializer = rabbitMqMessageSerializer;
        _options = options;
        ConnectionPool = connectionPool;
        Logger = logger;

        
        ExchangeDeclareDic = options.ExchangeDeclare?
            .ToDictionary(k=>k.ExchangeName, v =>v);

        QueueDeclareDic = options.QueueDeclare
            .ToDictionary(k => k.QueueName, v => v);
    }

    // public void Initialize()
    // {
    //     
    // }

    public void Dispose()
    {
        
    }

    public void Publish()
    {
        var message = CreateRabbitMQMessage();
        
    }

    public void SendMessageAsync(RabbitMQMessage message)
    {
        
    }

    public void SendMessageAsync(RabbitMQMessage message, string exchangeName, string queueName)
    {
        var body = _rabbitMqMessageSerializer.SerializeObject(message);
        try
        {
            if (exchangeName != null)
            {
                using (var channel = GetExchangeDeclareChannel(exchangeName))
                {
                    channel.BasicPublish(exchange:exchangeName,routingKey:message.Topic,body:body);
                }
            }
            else if (queueName != null)
            {
                using (var channel = GetQueueDeclareChannel(queueName))
                {
                    channel.BasicPublish(exchange:string.Empty,routingKey:$"Q-{queueName}",body:body);
                }
            }
            else
            {
                Logger.LogError("未找到 Tag:{0} 对应TagRoute",message.Tag);
            }
            
            Logger.LogInformation($" [x] Sent {message}");
        }
        catch (Exception e)
        {
            Logger.LogError(e,$"消息发送失败: {message}");
        }
    }

    private IModel GetExchangeDeclareChannel(string name)
    {
        var exchangeDeclare = ExchangeDeclareDic[name];
        var channel = ConnectionPool.Get(_options.ConnectionName).CreateModel();
        channel.ExchangeDeclare(exchange:exchangeDeclare.ExchangeName,type:exchangeDeclare.ExchangeType,
            durable:_options.Durable,autoDelete:false,arguments:exchangeDeclare.Arguments);
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

    public RabbitMQMessage CreateRabbitMQMessage()
    {
        return new RabbitMQMessage();
    }
}
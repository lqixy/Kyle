namespace Kyle.Infrastructure.RabbitMQExtensions;

public class MallRabbitMQConsumerOptions
{
    public string ConnectionName { get; set; } = "Default";
    public QueueDeclareOptions[] QueueDeclare { get; set; }
    public ExchangeDeclareOptions[] ExchangeDeclare { get; set; }
    public bool Durable { get; set; }
    public BasicQosOptions BasicQos { get; set; }

    public class BasicQosOptions
    {
        public uint PrefetchSize { get; set; }

        public ushort PrefetchCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Global { get; set; }
    }

    public class QueueDeclareOptions
    {
        public string QueueName { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoAck { get; set; } = true;

        public BasicRecoverOptions BasicRecover { get; set; }
    }

    public class ExchangeDeclareOptions
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
        public HashSet<string> RoutingKey { get; set; }
        public bool AutoAck { get; set; } = true;

        /// <summary>
        /// 是否绑定自定义交换机队列名,如果不为空 BindQueueName=>{ExchangeName}--{QueueNameSuffix}
        /// </summary>
        public string QueueNameSuffix { get; set; }
        /// <summary>
        /// 不为null则使用BasicRecover重新归队,模式使用BasicReject
        /// </summary>
        public BasicRecoverOptions BasicRecover { get; set; }
    }

    public class BasicRecoverOptions
    {
        public bool Requeue { get; set; }
    }
}
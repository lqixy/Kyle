namespace Kyle.Infrastructure.RabbitMQExtensions;

public class MallRabbitMQPublisherOptions
{
    public MallRabbitMQPublisherOptions()
    {
    }

    public QueueDeclareOptions[] QueueDeclare { get; set; }
    public ExchangeDeclareOptions[] ExchangeDeclare { get; set; }
    public bool Durable { get; set; }
    public string ConnectionName { get; set; } = "Default";
    
    public class QueueDeclareOptions
    {
        public string QueueName { get; set; }
        public Dictionary<string,object> Arguments { get; set; }
        public bool Exclusive { get; set; }
        public HashSet<string> Tag { get; set; }
    }
    
    public class ExchangeDeclareOptions
    {
        public string ExchangeName { get; set; }
        /// <summary>
        /// direct, topic, headers and fanout
        /// </summary>
        public string ExchangeType { get; set; } = "direct";

        public Dictionary<string,object> Arguments { get; set; }
        public HashSet<string> Tag { get; set; }
    }
}
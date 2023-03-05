namespace Kyle.Infrastructure.RabbitMQExtensions;

[Serializable]
public class RabbitMQMessage
{
    public string Topic { get; set; }

    public string Tag { get; set; }

    public int Code { get; set; }

    public byte[] Body { get; set; }

    public DateTime CreatedTime { get; set; }

    public RabbitMQMessage()
    {
    }

    public RabbitMQMessage(string topic,int code,byte[] body,string tag=null):this(topic,code,body,DateTime.Now,tag)
    {
        
    }
    public RabbitMQMessage(string topic, int code, byte[] body, DateTime createdTime,string tag = null)
    {
        Topic = topic;
        Tag = tag;
        Code = code;
        Body = body;
        CreatedTime = createdTime;
    }

    public override string ToString()
    {
        return string.Format("[Topic={0},Code={1},Tag={2},CreatedTime={3},BodyLength={4}", Topic, Code, Tag,
            CreatedTime, Body.Length);
    }
}
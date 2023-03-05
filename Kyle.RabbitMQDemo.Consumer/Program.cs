// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("消费者");

var factory = new ConnectionFactory()
{
    HostName = "10.179.240.115",
    Port = 5672,
    UserName = "admin",
    Password = "qi19841230",
    VirtualHost = "mall"
};

var connection = factory.CreateConnection();

var queueName = "queue1";
var channel = connection.CreateModel();channel.QueueDeclare(
    queue: queueName,//消息队列名称
    durable: false,//是否持久化,true持久化,队列会保存磁盘,服务器重启时可以保证不丢失相关信息。
    exclusive: false,//是否排他,true排他的,如果一个队列声明为排他队列,该队列仅对首次声明它的连接可见,并在连接断开时自动删除.
    autoDelete: false,//是否自动删除。true是自动删除。自动删除的前提是：致少有一个消费者连接到这个队列，之后所有与这个队列连接的消费者都断开时,才会自动删除.
    arguments: null ////设置队列的一些其它参数
);

channel.BasicQos(0,1,false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received+= (model, ea) =>
{
    Thread.Sleep(3000);
    var message = ea.Body.ToArray();
    Console.WriteLine("接收消息:{0}",Encoding.UTF8.GetString(message));
    
    channel.BasicAck(ea.DeliveryTag,true); // 开启返回消息确认
};

// 消费者开启监听
channel.BasicConsume(queueName,true,consumer);

Console.ReadKey();
channel.Dispose();
connection.Close();
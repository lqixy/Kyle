// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;

Console.WriteLine("生产者");

var factory = new ConnectionFactory()
{
    HostName = "10.179.240.115",
    Port = 5672,
    UserName = "admin",
    Password = "qi19841230",
    VirtualHost = "mall"
};

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

var queueName = "queue1";
channel.QueueDeclare(queueName, false, false, false, null);

var str = string.Empty;

do
{
    Console.WriteLine("发送内容");
    str = Console.ReadLine();
    var body = Encoding.UTF8.GetBytes(str);
    channel.BasicPublish("",queueName,null,body);
} while (str.Trim().ToLower()!="exit");

channel.Close();
connection.Close();
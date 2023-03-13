// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    HostName = "192.168.1.101",
    Port = 5672,
    VirtualHost = "mall",
    UserName = "kyle",
    Password = "19841230"
};

var connection = factory.CreateConnection();
var channel = connection.CreateModel();
var queueName = "Q-Demo";
channel.QueueDeclare(queueName, false, false, false, null);
var input = string.Empty;
do
{
    input = Console.ReadLine();

    var body = Encoding.UTF8.GetBytes(input);
    channel.BasicPublish("", queueName, null, body);
} while (input.Trim().ToLower() != "exit");

channel.Close();
connection.Close();
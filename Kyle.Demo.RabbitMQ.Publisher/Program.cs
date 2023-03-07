// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    HostName= "localhost",
    Port = 5672,
    VirtualHost=""
};

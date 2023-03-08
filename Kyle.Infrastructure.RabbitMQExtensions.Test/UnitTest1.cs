using Autofac;

namespace Kyle.Infrastructure.RabbitMQExtensions.Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        private readonly ApplicationMessagePublisher publisher;
        private readonly ApplicationMessageConsumer consumer;

        public UnitTest1()
        {

            publisher = Container.Resolve<ApplicationMessagePublisher>();

            publisher.Initalize(new MallRabbitMQPublisherOptions
            {
                QueueDeclare = new MallRabbitMQPublisherOptions.QueueDeclareOptions[]
                 {
                     new MallRabbitMQPublisherOptions.QueueDeclareOptions
                     {
                         Tag = new HashSet<string>
                         {
                             typeof(OrderCreatedApplicationMessage).FullName
                         },
                          QueueName= "test",
                     },

                 }
            });

            consumer = Container.Resolve<ApplicationMessageConsumer>();
           
        }

        [TestMethod]
        public void TestMethod1()
        {
            publisher.Publish(new OrderCreatedApplicationMessage());
        }

        [TestMethod]
        public void When_Consumer_Should_OK()
        {
            consumer.Initialize(new MallRabbitMQConsumerOptions
            {
                QueueDeclare = new MallRabbitMQConsumerOptions.QueueDeclareOptions[]
                 {
                     new MallRabbitMQConsumerOptions.QueueDeclareOptions
                     {
                         QueueName= "test",
                         AutoAck = true
                     }
                 }
            });
        }
    }
}
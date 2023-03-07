using Autofac;

namespace Kyle.Infrastructure.RabbitMQExtensions.Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        private readonly ApplicationMessagePublisher publisher;

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
        }

        [TestMethod]
        public void TestMethod1()
        {
            publisher.Publish(new OrderCreatedApplicationMessage());
        }
    }
}
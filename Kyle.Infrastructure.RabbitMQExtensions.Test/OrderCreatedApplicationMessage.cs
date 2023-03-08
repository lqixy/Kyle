using Kyle.Infrastructure.Events.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RabbitMQExtensions.Test
{
    public class OrderCreatedApplicationMessage : ApplicationMessage
    {
        public override string GetRoutingKey()
        {
            return "test";
        }
    }

    public class OrderEventHandler : IEventHandler<OrderCreatedApplicationMessage>
    {

        public void HandleEvent(OrderCreatedApplicationMessage eventData)
        {
            var str = JsonConvert.SerializeObject(eventData);
            Console.WriteLine(str);
        }
    }
}

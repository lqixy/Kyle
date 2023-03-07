using Kyle.Infrastructure.Events.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events.Test
{
    public class OrderEventHandler : IEventHandler<OrderEventData>
    {
        public void HandleEvent(OrderEventData eventData)
        {
            var str = JsonConvert.SerializeObject(eventData);
            Console.WriteLine($"Order: {eventData.Id}; {str}");
        }
    }

    public class UserEventHandler : IEventHandler<UserEventData>
    {
        public void HandleEvent(UserEventData eventData)
        {
            var str = JsonConvert.SerializeObject(eventData);
            Console.WriteLine($"User: {eventData.Id}; {str}");
        }
    }
}

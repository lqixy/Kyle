using Kyle.Demo.Events.EventDatas;
using Kyle.Infrastructure.Events.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Demo.Events.Handlers
{
    public class OrderEventHandler : IEventHandler<OrderEventData>
    {
        public void HandleEvent(OrderEventData eventData)
        {
            Console.WriteLine($"data: {eventData.Id}, {JsonConvert.SerializeObject(eventData)}");
        }
    }
}

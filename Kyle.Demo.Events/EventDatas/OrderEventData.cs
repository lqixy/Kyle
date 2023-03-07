using Kyle.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Demo.Events.EventDatas
{
    public class OrderEventData : EventData
    {
        public OrderEventData()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }


    }
}

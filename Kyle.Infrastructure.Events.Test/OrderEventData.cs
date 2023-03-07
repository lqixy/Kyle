using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events.Test
{
    public class OrderEventData : EventData
    {


        public Guid Id { get; set; }

        public OrderEventData(Guid id)
        {
            Id = id;
        }

        public OrderEventData()
        {
        }
    }

    public class UserEventData : EventData
    {
        public UserEventData()
        {
        }

        public Guid Id { get; set; }

        public UserEventData(Guid id)
        {
            Id = id;
        }
    }
}

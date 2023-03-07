using Kyle.Infrastructure.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events.Test
{
    public class TestBase
    {
        protected EventBus EventBus { get; }
        public TestBase()
        {
            EventBus = EventsExtensions.AddEventService();
        }
    }
}

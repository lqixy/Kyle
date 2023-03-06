using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events.Test
{
    public class TestBase
    {
        public TestBase()
        {
            EventsExtensions.AddEventService();
        }
    }
}

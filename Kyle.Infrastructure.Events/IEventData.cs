﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events
{
    public interface IEventData
    {
        DateTime EventTime { get; set; }

        object EventSource { get; set; }
    }

    public class EventData : IEventData
    {
        public DateTime EventTime { get; set; }

        public object EventSource { get; set; }

        public EventData()
        {
            EventTime = DateTime.Now;
        }
    }
}
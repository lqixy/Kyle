// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");



using Kyle.Demo.Events.EventDatas;
using Kyle.Infrastructure.Events;

var bus = EventsExtensions.AddEventService();

bus.Trigger(new OrderEventData());


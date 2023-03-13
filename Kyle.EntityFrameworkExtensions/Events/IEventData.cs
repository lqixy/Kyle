using MediatR;

namespace Kyle.EntityFrameworkExtensions.Events;

// public interface IEventData : IRequest
// {
//     bool IsPublisher { get; set; }
// }
//
// public interface IMessage
// {
//     string GetRoutingKey();
//     string GetTypeName();
// }
//
// public abstract class ApplicationMessage : IEventData, IMessage
// {
//     public bool IsPublisher { get; set; } = true;
//     public  virtual string GetRoutingKey()
//     {
//         return null;
//     }
//
//     public virtual string GetTypeName()
//     {
//         return this.GetType().FullName;
//     }
// }
//
// public abstract class EventData : IEventData
// {
//     public bool IsPublisher { get; set; } = false;
// }
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events.Handlers
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<TEventData> : IEventHandler where TEventData : IEventData
    {
        void HandleEvent(TEventData eventData);
    }
}

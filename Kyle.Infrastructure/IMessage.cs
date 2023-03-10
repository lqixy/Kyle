using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure
{
    public interface IMessage
    {
        string Id { get; set; }

        DateTime Timestamp { get; set; }

        int Sequence { get; set; }

        string GetRoutingKey();

        string GetTypeName();
    }
}

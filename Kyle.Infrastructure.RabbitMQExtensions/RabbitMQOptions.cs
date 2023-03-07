using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RabbitMQExtensions
{
    public class RabbitMQOptions
    {
        public RabbitMQOptions()
        {
            Connections = new RabbitMqConnections();
        }

        public RabbitMqConnections Connections { get; }


    }
}

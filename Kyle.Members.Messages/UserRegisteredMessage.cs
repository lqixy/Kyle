using Kyle.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Messages
{
    public class UserRegisteredMessage : EventData
    {
        public UserRegisteredMessage()
        {
        }

        public UserRegisteredMessage(Guid userId, Guid tenantId)
        {
            UserId = userId;
            TenantId = tenantId;
        }

        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}

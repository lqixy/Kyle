using Kyle.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Domain.Events
{
    public class UserRegisteredHaveRecord : EventData
    {
        public UserRegisteredHaveRecord()
        {
        }

        public UserRegisteredHaveRecord(Guid userId, Guid tenantId)
        {
            UserId = userId;
            TenantId = tenantId;
        }

        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}

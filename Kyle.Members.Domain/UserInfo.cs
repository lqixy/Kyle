using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Domain
{
    public class UserInfo
    {
        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public DateTime RegDate { get; set; }

        //public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}

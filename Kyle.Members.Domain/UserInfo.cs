using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Domain
{
    public class UserInfo
    {
        public string UserId { get; set; }

        public string TenantId { get; set; }

        public DateTime RegDate { get; set; }

        //public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}

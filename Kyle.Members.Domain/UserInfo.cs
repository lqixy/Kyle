using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Domain
{
    public class UserInfo
    {
        public Guid UserId { get; set; } = Guid.NewGuid();


        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}

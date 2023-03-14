using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Application.Constructs
{
    public class UserInfoDto
    {
        public UserInfoDto()
        {
        }

        public UserInfoDto(Guid userId, Guid tenantId, DateTime regDate)
        {
            UserId = userId;
            TenantId = tenantId;
            RegDate = regDate;
        }

        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public DateTime RegDate { get; set; }

        public string UserName { get; set; }
        
        public string Pwd { get; set; }

        public byte? DelStatus { get; set; }
        
    }
}

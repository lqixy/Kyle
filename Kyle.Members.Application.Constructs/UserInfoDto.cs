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

        public UserInfoDto(string userId, string tenantId, DateTime regDate)
        {
            UserId = userId;
            TenantId = tenantId;
            RegDate = regDate;
        }

        public string UserId { get; set; }

        public string TenantId { get; set; }

        public DateTime RegDate { get; set; }
    }
}

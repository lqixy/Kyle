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

        public UserInfoDto(Guid userId, DateTime creationTime)
        {
            UserId = userId;
            CreationTime = creationTime;
        }

        public Guid UserId { get; set; } 


        public DateTime CreationTime { get; set; }
    }
}

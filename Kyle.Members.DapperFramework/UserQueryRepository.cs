using Kyle.Members.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.DapperFramework
{
    public class UserQueryRepository : IUserQueryRepository
    {
        public async Task<UserInfo> Get()
        {
            //return new Task<UserInfo>(() => new UserInfo());
            return new UserInfo();
        }
    }
}

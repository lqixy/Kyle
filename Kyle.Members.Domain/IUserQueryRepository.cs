using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Domain
{
    public interface IUserQueryRepository
    {
        Task<UserInfo> Get();
    }
}

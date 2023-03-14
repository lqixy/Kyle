using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Kyle.Members.Domain;

namespace Kyle.Members.Domain
{
    public interface IUserQueryRepository
    {
        Task<UserInfo> Get();        
        Task<UserInfo> Get(Guid userId);

        Task<UserInfo> Get(Expression<Func<UserInfo, bool>> predicate);
    }
}

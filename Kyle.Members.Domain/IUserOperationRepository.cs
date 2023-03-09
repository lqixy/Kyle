using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kyle.Members.Domain;

namespace Kyle.Members.Domain
{
    public interface IUserOperationRepository
    {
        Task Insert(UserInfo entity);
        Task Insert(IEnumerable<UserInfo> entities);
    }
}

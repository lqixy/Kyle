using Kyle.EntityFrameworkExtensions;
using Kyle.Members.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.EntityFramework
{
    public class UserQueryRepository : EfCoreRepositoryBase<UserInfo>, IUserQueryRepository
    {
        public UserQueryRepository(MallDbContext context) : base(context)
        {
        }

        public async Task<UserInfo> Get()
        {
            return dbSet.FirstOrDefault(x => x.UserId == Guid.Parse("8FCEE2F7-DC47-456B-8686-BE1B58AB9A7E"));
        }
    }
}

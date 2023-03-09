using Kyle.EntityFrameworkExtensions;
using Kyle.Members.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<UserInfo> Get(Expression<Func<UserInfo, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate: predicate);
        }
    }
}

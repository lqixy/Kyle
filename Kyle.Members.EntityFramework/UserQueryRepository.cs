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
            return dbSet.FirstOrDefault();
        }

        public async Task<UserInfo> Get(Guid userId)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<UserInfo> Get(Expression<Func<UserInfo, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate: predicate);
        }
    }
}
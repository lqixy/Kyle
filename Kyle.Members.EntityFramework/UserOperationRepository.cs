using Kyle.EntityFrameworkExtensions;
using Kyle.Members.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.EntityFramework
{
    public class UserOperationRepository : EfCoreRepositoryBase<UserInfo>, IUserOperationRepository
    {
        public UserOperationRepository(MallDbContext context) : base(context)
        {
        }

        public async Task Insert(UserInfo entity)
        {
            await dbSet.AddAsync(entity);

            await Context.SaveChangesAsync();
        }

        public async Task Insert(IEnumerable<UserInfo> entities)
        {
            dbSet.AddRange(entities);

            await Context.SaveChangesAsync();
        }

    }
}

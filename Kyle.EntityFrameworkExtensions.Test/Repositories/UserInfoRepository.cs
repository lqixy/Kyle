using Kyle.EntityFrameworkExtensions.Test.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.EntityFrameworkExtensions.Test.Repositories
{
    public class UserInfoRepository : EfCoreRepositoryBase<UserInfo>
    {
        public UserInfoRepository(MallDbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<UserInfo>> Get()
        {
            
            return await dbSet.ToListAsync();
        }
    }
}

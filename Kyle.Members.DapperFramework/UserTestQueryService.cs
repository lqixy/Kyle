using Dapper;
using Kyle.DapperFrameworkExtensions;
using Kyle.Extensions;
using Kyle.Members.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.DapperFramework
{
    public class UserTestQueryService : DapperRepositoryBase, IUserTestQueryService, ISingletonDependency
    {
        public UserTestQueryService(SqlDbContext sqlDbContext) : base(sqlDbContext, "")
        {
        }

        public async Task<UserInfo> Get()
        {
            return await DB.QueryFirstAsync<UserInfo>("select top 1 * from UserBaseInfo");
        }
    }
}

using Dapper;
using Kyle.DapperFrameworkExtensions;
using Kyle.Members.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.DapperFramework
{
    public class UserQueryRepository : DapperRepositoryBase, IUserQueryRepository
    {
        public UserQueryRepository(SqlDbContext sqlDbContext) : base(sqlDbContext)
        {
        }

        //public UserQueryRepository(UserDapperDbContextProvider dbContextProviderDbContext) : base(dbContextProviderDbContext)
        //{
        //}



        public async Task<UserInfo> Get()
        {
            var sql = "select top 1 * from UserBaseinfo ";

            //var result = await DB.Connection.QueryFirstAsync<UserInfo>(sql);
            var result = await DB.QueryFirstAsync<UserInfo>(sql);
            return result;
        }
    }
}

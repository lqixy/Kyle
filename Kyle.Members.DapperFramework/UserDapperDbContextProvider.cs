using Kyle.DapperFrameworkExtensions;
using Kyle.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.DapperFramework
{
    //public class UserDapperDbContextProvider : DapperDbContextProvider
    //{
    //    //public UserDapperDbContextProvider(IDbConnection dbContext, UserSqlServerDbProviderFactory factory) : base(dbContext, factory)
    //    //{
    //    //}
    //    public UserDapperDbContextProvider(IDatabase dbContext, UserSqlServerDbProviderFactory factory) : base(dbContext, factory)
    //    {
    //    }
    //}

    //public class UserSqlServerDbProviderFactory : IDbProviderFactory, ISingletonDependency
    //{
    //    public static string Name;

    //    private readonly IConnectionStringResolver _connectionStringResolver;

    //    public UserSqlServerDbProviderFactory(IConnectionStringResolver connectionStringResolver)
    //    {
    //        _connectionStringResolver = connectionStringResolver;
    //    }

    //    public string ConnectionString => _connectionStringResolver.GetConnectionString(Name);

    //    public DbProviderFactory DbFactory => SqlClientFactory.Instance;

    //}
}

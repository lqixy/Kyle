using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    public class SqlServerDbProviderFactory : IDbProviderFactory
    {
        private readonly IConnectionStringResolver _connectionStringResolver;

        public SqlServerDbProviderFactory(IConnectionStringResolver connectionStringResolver)
        {
            _connectionStringResolver = connectionStringResolver;
        }
        public string ConnectionString => _connectionStringResolver.GetConnectionString(string.Empty);

        public DbProviderFactory DbFactory => SqlClientFactory.Instance;

    }
}

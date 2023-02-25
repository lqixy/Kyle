using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    public class SqlDbContext
    {
        private readonly IConnectionStringResolver connectionStringResolver;

        public SqlDbContext(IConnectionStringResolver connectionStringResolver)
        {
            this.connectionStringResolver = connectionStringResolver;
        }

        public IDbConnection CreateConnection(string? name = null)
        {
            return new SqlConnection(connectionStringResolver.GetConnectionString(name));
        }
        //public IDbConnection CreateConnection(string name)
        //{
        //    return new SqlConnection(connectionStringResolver.GetConnectionString(name));
        //}
    }
}

using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    public class SqlDbContext:IDapperDbContext
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

    public interface IDapperDbContext
    {
        IDbConnection CreateConnection(string? name);
    }

    public class MySqlDbContext: IDapperDbContext
    {
        private readonly IConnectionStringResolver connectionStringResolver1;

        public MySqlDbContext(IConnectionStringResolver connectionStringResolver1)
        {
            this.connectionStringResolver1 = connectionStringResolver1;
        }

        public IDbConnection CreateConnection(string? name = null)
        {
            return new MySqlConnection(connectionStringResolver1.GetConnectionString(name));
        }
    }
}

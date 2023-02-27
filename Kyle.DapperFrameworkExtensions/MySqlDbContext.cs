using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;

namespace Kyle.DapperFrameworkExtensions
{
    public class MySqlDbContext : IDapperDbContext
    {
        private readonly IConnectionStringResolver connectionStringResolver;

        public MySqlDbContext(IConnectionStringResolver connectionStringResolver)
        {
            this.connectionStringResolver = connectionStringResolver;
        }

        public IDbConnection CreateConnection(string? name = null)
        {
            return new MySqlConnection(connectionStringResolver.GetConnectionString(name));
        }
    }
}
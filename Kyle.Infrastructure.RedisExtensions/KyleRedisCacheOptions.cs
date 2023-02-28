using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public class KyleRedisCacheOptions
    {
        //private const string ConnectionStringKey = "Kyle.Redis.Cache";

        //private const string DatabaseIdSettingKey = "Kyle.Redis.Cache.DatabaseId";

        private readonly IConfiguration configuration;

        public KyleRedisCacheOptions(IConfiguration configuration)
        {
            this.configuration = configuration;

            ConnectionString = GetDefaultConnectionString();
            DatabaseId = GetDefaultDatabaseId();
        }

        public string ConnectionString { get; set; }

        public int DatabaseId { get; set; }

        private int GetDefaultDatabaseId()
        {
            //var
            var databaseId = configuration["Redis:DatabaseId"];
            if (string.IsNullOrWhiteSpace(databaseId)) return -1;
            if (!int.TryParse(databaseId, out int id)) return -1;
            return id;
        }

        private string GetDefaultConnectionString()
        {
            var connStr = configuration["Redis:ConnectionString"];
            if (string.IsNullOrWhiteSpace(connStr)) return "localhost";
            return connStr;
        }

    }
}

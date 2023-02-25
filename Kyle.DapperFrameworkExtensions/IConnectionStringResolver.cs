using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    public interface IConnectionStringResolver
    {
        string GetConnectionString(string name);
    }

    public class DefaultConnectionStringResolver : IConnectionStringResolver
    {
        private readonly IConfiguration _configuration;

        public DefaultConnectionStringResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) name = "Default";

            if (_configuration.GetConnectionString(name) != null) return _configuration.GetConnectionString(name);

            throw new ArgumentException($"未找到数据库配置: {name}");
        }
    }
}

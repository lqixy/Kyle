using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.RedisDemo
{
    public class Test : ITest
    {
        private readonly IConfiguration _configuration;

        public Test(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void Foo()
        {
            Console.WriteLine(this._configuration["Redis:ConnectionString"]);
        }
    }

    public interface ITest
    {
        void Foo();
    }
}

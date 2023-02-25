using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    public interface IDatabase
    {
        IDbConnection Connection { get; }

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        /// <param name="DbConnection"></param>
        void SetConnection(IDbConnection DbConnection);
    }

    public class Database : IDatabase
    {
        public IDbConnection Connection { get; private set; }

        public void SetConnection(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}

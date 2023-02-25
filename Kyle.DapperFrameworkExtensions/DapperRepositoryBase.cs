using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    public class DapperRepositoryBase
    {
        //public IDatabase DB => _dbContextProviderDbContext.GetDbConnection();

        //private IDbContextProvider<IDatabase> _dbContextProviderDbContext { set; get; }

        //public DapperRepositoryBase(IDbContextProvider<IDatabase> dbContextProviderDbContext)
        //{
        //    _dbContextProviderDbContext = dbContextProviderDbContext;
        //}

        public IDbConnection DB => sqlDbContext.CreateConnection();

        private readonly SqlDbContext sqlDbContext;

        public DapperRepositoryBase(SqlDbContext sqlDbContext)
        {
            this.sqlDbContext = sqlDbContext;
        }
    }
}

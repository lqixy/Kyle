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

        //private IDbContextProvider _dbContextProviderDbContext { set; get; }

        //public DapperRepositoryBase(IDbContextProvider dbContextProviderDbContext)
        //{
        //    _dbContextProviderDbContext = dbContextProviderDbContext;
        //}

        public IDbConnection DB => dbContext.CreateConnection(dbName);
        private readonly string? dbName;
        private readonly IDapperDbContext dbContext;

        public DapperRepositoryBase(IDapperDbContext dbContext, string? dbName = null)
        {
            this.dbContext = dbContext;
            this.dbName = dbName;
        }


    }
}

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions.UOW
{
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IRepository
    {
        List<TEntity> GetAll(Func<TEntity, bool> expression);

        TEntity Get(Func<TEntity, bool> expression);
    }

    public class DapperRepository<TEntity> : IRepository<TEntity>
    {
        public IDatabase DB { get { return _dbContextProvider.GetDbConnection(); } }
        private IDbContextProvider<IDatabase> _dbContextProvider;

        public DapperRepository(IDbContextProvider<IDatabase> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public TEntity Get(Func<TEntity, bool> expression)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> GetAll(Func<TEntity, bool> expression)
        {
            throw new NotImplementedException();
        }
    }
}

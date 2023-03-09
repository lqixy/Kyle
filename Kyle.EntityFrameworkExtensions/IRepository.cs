using Kyle.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.EntityFrameworkExtensions
{
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IRepository
    {

    }

    //public abstract class RepositoryBase<TEntity> : IRepository<TEntity>, ITransientDependency
    //{
    //    public abstract TEntity FirstOrDefault(Func<TEntity, bool> expriesion);

    //}

    public class EfCoreRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected MallDbContext Context;
        protected DbSet<TEntity> dbSet;

        public EfCoreRepositoryBase(MallDbContext context)
        {
            Context = context;
            dbSet = Context.Set<TEntity>();
        }

        //public override TEntity FirstOrDefault(Func<TEntity, bool> predicate)
        //{
        //    return dbSet.FirstOrDefault(predicate);
        //}
    }
}

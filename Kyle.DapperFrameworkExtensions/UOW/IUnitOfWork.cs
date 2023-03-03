using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions.UOW
{
    public interface IUnitOfWork
    {
        void SaveChange();

        Task SaveChangeAsync();
    }

    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        public abstract void SaveChange();

        public abstract Task SaveChangeAsync();
    }

    //public class DapperUnitOfWork : IUnitOfWork
    //{

    //}
}

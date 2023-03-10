using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure
{
    public interface IEntity<IKey>
    {

    }

    public class Entity<TKey> : IEntity<TKey>
    {

    }
}

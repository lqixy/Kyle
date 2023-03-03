using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.EntityFrameworkExtensions
{
    public class IBaseModel<TKey>
    {
        TKey Id { get; set; }
    }

    [Serializable]
    public abstract class BaseModel<TKey> : IBaseModel<TKey>
    {
        public abstract TKey Id { get; set; }
    }

    [Serializable]
    public abstract class BaseViewModel
    {

    }
}

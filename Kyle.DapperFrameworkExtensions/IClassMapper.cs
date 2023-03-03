using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    public interface IClassMapper
    {
        string SchemaName { get; }

        string TableName { get; }

        //IList<IPropertyMap>
    }
}

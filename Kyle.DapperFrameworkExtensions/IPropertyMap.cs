using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    public interface IPropertyMap
    {
        string Name { get; }

        string ColumnName { get; }

        bool Ignored { get; }

        bool IsReadOnly { get; }

        KeyType KeyType { get; }

        PropertyInfo PropertyInfo { get; }
    }

    //public class PropertyMap : IPropertyMap
    //{
    //    public PropertyInfo PropertyInfo { get; private set; }
    //    public string Name
    //    {
    //        get { return }
    //    }
    //}


    public enum KeyType
    {
        NotAKey,

        Identity,

        Guid,

        Assigned
    }
}

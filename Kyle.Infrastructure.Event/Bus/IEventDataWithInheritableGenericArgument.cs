using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Event.Bus
{
    /// <summary>
    /// 事件数据必须实现这个接口的类
    /// </summary>
    public interface IEventDataWithInheritableGenericArgument
    {
        /// <summary>
        /// 得到的参数创建这类自创建这个类的一个新实例。
        /// </summary>
        /// <returns></returns>
        object[] GetConstructorArgs();
    }
}

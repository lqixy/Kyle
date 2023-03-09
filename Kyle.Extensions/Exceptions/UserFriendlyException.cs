using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Extensions.Exceptions
{
    public class UserFriendlyException : KyleException
    {

        public new object Data { get; private set; }
        //
        // 摘要:
        //     异常的附加信息
        public string Details { get; private set; }
        //
        // 摘要:
        //     错误代码.
        public int Code { get; set; }
        public UserFriendlyException(string message)
           : base(message)
        {
        }

        public UserFriendlyException(int code, string message)
            : this(message)
        {
            Code = code;
        }

        public UserFriendlyException(string message, string details)
            : this(message)
        {
            Details = details;
        }

        public UserFriendlyException(string message, object data)
            : this(message)
        {
            Data = data;
        }

        public UserFriendlyException(int code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        public UserFriendlyException(int code, string message, string details)
            : this(message, details)
        {
            Code = code;
        }

        public UserFriendlyException(int code, string message, object details)
            : this(message, details)
        {
            Code = code;
        }

        public UserFriendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UserFriendlyException(string message, string details, Exception innerException)
            : this(message, innerException)
        {
            Details = details;
        }

        public UserFriendlyException(string message, object data, Exception innerException)
            : this(message, innerException)
        {
            Data = data;
        }
    }
}

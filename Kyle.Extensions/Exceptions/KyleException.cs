using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Extensions.Exceptions
{
    public class KyleException : Exception
    {
        public KyleException()
        {

        }


        public KyleException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }


        /// <param name="message">Exception message</param>
        public KyleException(string message)
            : base(message)
        {

        }


        public KyleException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

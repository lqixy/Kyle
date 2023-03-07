using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RabbitMQExtensions.IO
{
    public class AsyncTaskResult
    {
        public readonly static AsyncTaskResult Success = new AsyncTaskResult(AsyncTaskStatus.Success, null);

        public AsyncTaskStatus Status { get; private set; }

        public string ErrorMessage { get; private set; }

        public AsyncTaskResult(AsyncTaskStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }
    }

    public class AsyncTaskResult<T> : AsyncTaskResult
    {
        public T Data { get; private set; }

        /// <summary>Parameterized constructor.
        /// </summary>
        public AsyncTaskResult(AsyncTaskStatus status)
            : this(status, null, default(T))
        {
        }
        /// <summary>Parameterized constructor.
        /// </summary>
        public AsyncTaskResult(AsyncTaskStatus status, T data)
            : this(status, null, data)
        {
        }
        /// <summary>Parameterized constructor.
        /// </summary>
        public AsyncTaskResult(AsyncTaskStatus status, string errorMessage)
            : this(status, errorMessage, default(T))
        {
        }
        /// <summary>Parameterized constructor.
        /// </summary>
        public AsyncTaskResult(AsyncTaskStatus status, string errorMessage, T data)
            : base(status, errorMessage)
        {
            Data = data;
        }
    }
    public enum AsyncTaskStatus
    {
        Success,
        IOException,
        Failed
    }
}

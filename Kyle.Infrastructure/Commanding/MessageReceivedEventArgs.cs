using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Commanding
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageData Message { get; set; }

        public MessageReceivedEventArgs(MessageData message)
        {
            Message = message;
        }
    }
}

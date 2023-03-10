using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Commanding
{
    public interface IMessageSubscriber : IDisposable
    {
        /// <summary>
        /// 订阅消息消费接口
        /// </summary>
        void Subscribe();

        /// <summary>
        /// 消费者处理消息后回调事件
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}

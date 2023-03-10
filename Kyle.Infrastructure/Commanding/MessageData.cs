using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Commanding
{
    public class MessageData : ApplicationMessage
    {
        public string CommandName { get; set; }

        public JToken MessageDataBody { get; set; }
    }
}

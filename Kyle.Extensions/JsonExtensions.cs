using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
        {
            var options = new JsonSerializerSettings();

            if (camelCase)
            {
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            if (indented) { options.Formatting = Formatting.Indented; }

            return JsonConvert.SerializeObject(obj, options);
        }

        public static T JsonToObject<T>(this string obj)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch
            {
                return default;
            }
        }
    }
}

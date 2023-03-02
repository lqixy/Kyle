using Kyle.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public class RedisCacheSerializer : IRedisCacheSerializer
    {

        public object Deserialize(RedisValue objbyte)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<RedisCacheItem>(objbyte);
                return JsonConvert.DeserializeObject(item.Item, item.Type);

            }
            catch
            {
                return JsonSerializationHelper.DeserializeWithType(objbyte);
            }
        }

        public string Serialize(object value, Type type)
        {
            return JsonSerializationHelper.SerializeWithType(value, type);
        }
    }

    public interface IRedisCacheSerializer
    {
        object Deserialize(RedisValue objbyte);
        string Serialize(object value, Type type);
    }

    public static class JsonSerializationHelper
    {
        private const char TypeSeperator = '|';
        public static object DeserializeWithType(string value)
        {
            var typeIndex = value.IndexOf(TypeSeperator);
            var type = Type.GetType(value.Substring(0, typeIndex));
            var serialized = value.Substring(typeIndex + 1);

            var options = new JsonSerializerSettings();
            return JsonConvert.DeserializeObject(serialized, type, options);
        }

        public static string SerializeWithType(object value, Type type)
        {
            var serialized = value.ToJsonString();

            return string.Format("{0}{1}{2}", type.AssemblyQualifiedName, TypeSeperator, serialized);

        }

        ///// <summary>
        ///// JSON 转换为字符串，可以规范统一序列化格式
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="camelCase"></param>
        ///// <param name="indented"></param>
        ///// <returns></returns>
        //public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
        //{
        //    var options = new JsonSerializerSettings();

        //    if (camelCase)
        //    {
        //        options.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //    }

        //    if (indented)
        //    {
        //        options.Formatting = Formatting.Indented;
        //    }


        //    return JsonConvert.SerializeObject(obj, options);
        //}
    }
}

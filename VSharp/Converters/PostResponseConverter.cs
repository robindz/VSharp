using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using VSharp.Models;

namespace VSharp.Converters
{
    internal class PostResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken t = JToken.Load(reader).SelectToken("next_params");
            PostPagingParams pagingParams = (PostPagingParams)t.ToObject(typeof(PostPagingParams));
            return pagingParams;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject parent = new JObject
            {
                { "next_params", JObject.FromObject(value) }
            };
            parent.WriteTo(writer);
        }
    }
}

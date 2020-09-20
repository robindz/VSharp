using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace VSharp.Converters
{
    public class ThousandsSeparatedNumberConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = (string)reader.Value;
            if (string.IsNullOrEmpty(value))
                return 0;

            value = value.Replace(",", string.Empty);
            if (objectType == typeof(int))
                return int.Parse(value);
            else
                return long.Parse(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
            t.WriteTo(writer);
        }
    }
}

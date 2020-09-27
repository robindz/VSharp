using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using VSharp.Constants;

namespace VSharp.Converters
{
    public class KstToUtcConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value.GetType() == typeof(string))
            {
                DateTime kstTime = DateTime.Parse((string)reader.Value);
                return TimeZoneInfo.ConvertTimeToUtc(kstTime, TimeZones.KST);
            } 
            else
            {
                return TimeZoneInfo.ConvertTime((DateTime)reader.Value, TimeZoneInfo.Utc);
            }            
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
            t.WriteTo(writer);
        }
    }
}

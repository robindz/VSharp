using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using VSharp.Models;

namespace VSharp.Converters
{
    internal class DecodeChannelCodeResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject o = JObject.Load(reader);
            var result = o["result"];

            return new DecodeChannelCodeResponse 
            { 
                ChannelCode = result.Value<string>("channelCode"), 
                ChannelSeq = result.Value<int>("channelSeq") 
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = (DecodeChannelCodeResponse)value;
            JObject parent = new JObject(
                new JProperty("result",
                    new JObject(
                        new JProperty("channelCode", decodeChannelCodeResponse.ChannelCode),
                        new JProperty("channelSeq", decodeChannelCodeResponse.ChannelSeq)
                    )
                )
            );
            parent.WriteTo(writer);
        }
    }
}

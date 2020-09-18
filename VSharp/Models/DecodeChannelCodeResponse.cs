using Newtonsoft.Json;
using VSharp.Converters;

namespace VSharp.Models
{
    [JsonConverter(typeof(DecodeChannelCodeResponseConverter))]
    public class DecodeChannelCodeResponse
    {
        [JsonProperty("channelSeq")]
        public int ChannelSeq { get; set; }

        [JsonProperty("channelCode")]
        public string ChannelCode { get; set; }
    }
}

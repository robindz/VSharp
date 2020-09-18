using Newtonsoft.Json;

namespace VSharp.Models
{
    public class DecodeChannelCodeResponse
    {
        [JsonProperty("channelSeq")]
        public int ChannelSeq { get; set; }

        [JsonProperty("channelCode")]
        public string ChannelCode { get; set; }
    }
}

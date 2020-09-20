using Newtonsoft.Json;
using VSharp.Converters;

namespace VSharp.Models
{
    public class Status
    {
        [JsonProperty("videoSeq")]
        public int VideoSeq { get; set; }

        [JsonProperty("status")]
        public string StatusType { get; set; }

        [JsonProperty("isOnAir")]
        public bool IsOnAir { get; set; }

        [JsonProperty("playCount")]
        [JsonConverter(typeof(ThousandsSeparatedNumberConverter))]
        public long PlayCount { get; set; }

        [JsonProperty("likeCount")]
        [JsonConverter(typeof(ThousandsSeparatedNumberConverter))]
        public long LikeCount { get; set; }

        [JsonProperty("commentCount")]
        [JsonConverter(typeof(ThousandsSeparatedNumberConverter))]
        public long CommentCount { get; set; }

        [JsonProperty("onAirStartAt")]
        public string OnAirStartAt { get; set; }
#nullable enable
        [JsonProperty("noticeText")]
        public string? NoticeText { get; set; }
#nullable disable
        [JsonProperty("type")]
        public string VideoType { get; set; }

        [JsonProperty("wasLive")]
        public bool WasLive { get; set; }
    }
}

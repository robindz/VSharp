using Newtonsoft.Json;
using System;
using VSharp.Converters;

namespace VSharp.Models
{
    public class Notice
    {
        [JsonProperty("noticeNo")]
        public int NoticeNumber { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ymdtExposeYn")]
        public string YmdtExposeYn { get; set; }

        [JsonProperty("registerYmdt")]
        [JsonConverter(typeof(KstToUtcConverter))]
        public DateTime RegisterYmdtAt { get; set; }

        [JsonProperty("displayStartYmdt")]
        [JsonConverter(typeof(KstToUtcConverter))]
        public DateTime DisplayStartYmdtAt { get; set; }

        [JsonProperty("listImageUrl")]
        public string ListImageUrl { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("plusChannelOnly")]
        public bool IsPlusChannelOnly { get; set; }

        [JsonProperty("newNoticeYn")]
        public string NewNoticeYn { get; set; }

        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }
    }
}

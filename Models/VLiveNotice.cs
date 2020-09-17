using Newtonsoft.Json;
using System;

namespace VSharp.Models
{
    // TODO: Add special converters for properties, data: http://notice.vlive.tv/notice/list.json?channel_seq=6
    public class VLiveNotice
    {
        [JsonProperty("noticeNo")]
        public int NoticeNumber { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ymdtExposeYn")]
        public string YmdtExposeYn { get; set; }

        [JsonProperty("registerYmdt")]
        public DateTime RegisterYmdtAt { get; set; }

        [JsonProperty("displayStartYmdt")]
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

using Newtonsoft.Json;
using System;
using VSharp.Converters;

namespace VSharp.Models
{
    public class Video
    {
        public string VLiveUrl
        {
            get
            {
                return $"https://www.vlive.tv/video/{VideoSeq}";
            }
        }

        [JsonProperty("videoSeq")]
        public int VideoSeq { get; set; }

        [JsonProperty("videoType")]
        public string VideoType { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("playCount")]
        public long PlayCount { get; set; }

        [JsonProperty("likeCount")]
        public long LikeCount { get; set; }

        [JsonProperty("commentCount")]
        public long CommentCount { get; set; }

        [JsonProperty("thumbnail")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("pickSortOrder")]
        public int PickSortOrder { get; set; }

        [JsonProperty("screenOrientation")]
        public string ScreenOrientation { get; set; }

        [JsonProperty("willStartAt")]
        [JsonConverter(typeof(KstToUtcConverter))]
        public DateTime WillStartAt { get; set; }

        [JsonProperty("willEndAt")]
        [JsonConverter(typeof(KstToUtcConverter))]
        public DateTime WillEndAt { get; set; }

        [JsonProperty("createdAt")]
        [JsonConverter(typeof(KstToUtcConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("upcomingYn")]
        public string UpcomingYn { get; set; }

        [JsonProperty("specialLiveYn")]
        public string SpecialLiveYn { get; set; }

        [JsonProperty("liveThumbYn")]
        public string LiveThumbYn { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("packageProductId")]
        public string PackageProductId { get; set; }

        [JsonProperty("productType")]
        public string ProductType { get; set; }

        [JsonProperty("playTime")]
        public int PlayTimeInSeconds { get; set; }

        [JsonProperty("channelPlusPublicYn")]
        public string ChannelPlusPublicYn { get; set; }

        [JsonProperty("exposeStatus")]
        public string ExposeStatus { get; set; }

        [JsonProperty("representChannelName")]
        public string RepresentChannelName { get; set; }

        [JsonProperty("representChannelProfileImg")]
        public string RepresentChannelProfileImageUrl { get; set; }

        [JsonProperty("onAirStartAt")]
        [JsonConverter(typeof(KstToUtcConverter))]
        public DateTime OnAirStartAt { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }
#nullable enable
        [JsonProperty("videoPlaylist")]
        public Playlist? Playlist { get; set; }
#nullable disable
    }
}

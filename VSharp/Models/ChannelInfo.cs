using Newtonsoft.Json;

namespace VSharp.Models
{
    public class ChannelInfo
    {
        [JsonProperty("channelSeq")]
        public int ChannelSeq { get; set; }

        [JsonProperty("basicChannelSeq")]
        public int BasicChannelSeq { get; set; }

        [JsonProperty("channelCode")]
        public string ChannelCode { get; set; }

        [JsonProperty("channelPlusType")]
        public string ChannelPlusType { get; set; }

        [JsonProperty("channelName")]
        public string ChannelName { get; set; }

        [JsonProperty("representativeColor")]
        public string RepresentativeColour { get; set; }

        [JsonProperty("channelProfileImage")]
        public string ChannelProfileImageUrl { get; set; }

        [JsonProperty("backgroundColor")]
        public string BackgroundColour { get; set; }

        [JsonProperty("channelCoverImage")]
        public string ChannelCoverImageUrl { get; set; }

        [JsonProperty("fanCount")]
        public int FanCount { get; set; }

        [JsonProperty("comment")]

        public string Comment { get; set; }
        [JsonProperty("prohibitedWordLike")]

        public string ProhibitedWordLike { get; set; }
        [JsonProperty("prohibitedWordExact")]

        public string ProhibitedWordExact { get; set; }

        [JsonProperty("snsShareImg")]
        public string SnsShareImageUrl { get; set; }

        [JsonProperty("bannerShowYn")]
        public string BannerShownYn { get; set; }

        [JsonProperty("qrcode")]
        public string QRCodeUrl { get; set; }

        [JsonProperty("upcomingShowYn")]
        public string UpcomingShowYn { get; set; }

        [JsonProperty("mediaChannel")]
        public bool MediaChannel { get; set; }

        [JsonProperty("fanclub")]
        public bool Fanclub { get; set; }
    }
}

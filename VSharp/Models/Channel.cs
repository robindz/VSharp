using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSharp.Models
{
    public class Channel
    {
        [JsonProperty("channel_seq")]
        public int ChannelSeq { get; set; }

        [JsonProperty("channel_code")]
        public string ChannelCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("channel_name")]
        public string ChannelName { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("fan_count")]
        public int FanCount { get; set; }

        [JsonProperty("channel_cover_img")]
        public string ChannelCoverImageUrl { get; set; }

        [JsonProperty("channel_profile_img")]
        public string ChannelProfileImageUrl { get; set; }

        [JsonProperty("representative_color")]
        public string RepresentativeColour { get; set; }

        [JsonProperty("background_color")]
        public string BackgroundColour { get; set; }

        [JsonProperty("is_show_banner")]
        public bool IsShowBanner { get; set; }

        [JsonProperty("is_show_upcomming")]
        public bool IsShowUpcoming { get; set; }

        [JsonProperty("media_channel")]
        public bool IsMediaChannel { get; set; }

        [JsonProperty("fanclub")]
        public bool IsFanclub { get; set; }

        [JsonProperty("gfp_ad_enabled")]
        public bool IsGfpAdEnabled { get; set; }

        [JsonProperty("banner_ad_enabled")]
        public bool IsBannerAdEnabled { get; set; }

        [JsonProperty("ad_channel_id")]
        public int AdChannelId { get; set; }

        [JsonProperty("ad_cp_id")]
        public int AdCpId { get; set; }

        [JsonProperty("agency_seq")]
        public int AgencySeq { get; set; }
#nullable enable
        [JsonProperty("vstore", NullValueHandling = NullValueHandling.Ignore)]
        public VStore? Store { get; set; }
#nullable disable
        [JsonProperty("celeb_boards")]
        public List<CelebBoard> CelebBoards { get; set; } = new List<CelebBoard>();

        [JsonProperty("fan_boards")]
        public List<FanBoard> FanBoards { get; set; } = new List<FanBoard>();

        public class VStore
        {
            [JsonProperty("vstore_seq")]
            public int VStoreSeq { get; set; }

            [JsonProperty("vstore_home_link")]
            public string VStoreHomeUrl { get; set; }
        }

        public class CelebBoard
        {
            [JsonProperty("board_id")]
            public int BoardId { get; set; }

            [JsonProperty("special_ongoing")]
            public bool IsSpecialOngoing { get; set; }
        }

        public class FanBoard
        {
            [JsonProperty("board_id")]
            public int BoardId { get; set; }

            [JsonProperty("special_ongoing")]
            public bool IsSpecialOngoing { get; set; }
        }
    }
}

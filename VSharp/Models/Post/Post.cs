using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VSharp.Models.Post
{
    public class Post
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAtEpochInMilliseconds { get; set; }

        [JsonProperty("post_id")]
        public string Id { get; set; }

        [JsonProperty("written_in")]
        public string WrittenIn { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("board_ids")]
        public List<int> BoardIds { get; set; } = new List<int>();

        [JsonProperty("emotion_count")]
        public int EmotionCount { get; set; }

        [JsonProperty("is_event")]
        public bool IsEvent { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("content_version")]
        public string ContentVersion { get; set; }

        [JsonProperty("notice_status")]
        public bool NoticeStatus { get; set; }

        [JsonProperty("is_best")]
        public bool IsBest { get; set; }

        [JsonProperty("is_visible_to_authorized_users")]
        public bool IsVisibleToAuthorizedUsers { get; set; }

        [JsonProperty("is_restricted")]
        public bool IsRestricted { get; set; }

        [JsonProperty("news_categories")]
        public List<string> NewsCategories { get; set; } = new List<string>();

        [JsonProperty("post_categories")]
        public Dictionary<string, int> PostCategories { get; set; } = new Dictionary<string, int>();

        [JsonProperty("image_list")]
        public List<PostImage> Images { get; set; } = new List<PostImage>();
#nullable enable
        [JsonProperty("reservation_info")]
        public PostReservationInfo? ReservationInfo { get; set; }
#nullable disable
        [JsonProperty("author")]
        public PostAuthor Author { get; set; }

        public class PostImage
        {
            [JsonProperty("thumb")]
            public string ThumbnailUrl { get; set; }

            [JsonProperty("width")]
            public int WidthInPixels { get; set; }

            [JsonProperty("height")]
            public int HeightInPixels { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class PostReservationInfo
        {
            [JsonProperty("timezone")]
            public string Timezone { get; set; }

            [JsonProperty("publish_at")]
            public DateTime PublishedAt { get; set; }
        }

        public class PostAuthor
        {
            [JsonProperty("user_seq")]
            public int UserSeq { get; set; }

            [JsonProperty("nickname")]
            public string Nickname { get; set; }

            [JsonProperty("profile_image")]
            public string ProfileImageUrl { get; set; }

            [JsonProperty("role")]
            public string Role { get; set; }

            [JsonProperty("is_channel_plus")]
            public bool IsChannelPlus { get; set; }

            [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
            public int? Level { get; set; }

            [JsonProperty("v_number")]
            public string VNumber { get; set; }
        }
    }
}

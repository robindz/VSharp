using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VSharp.Models
{
    public class About
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("openAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("tagContentList")]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [JsonProperty("popularCountryList")]
        public List<string> PopularCountries { get; set; } = new List<string>();

        [JsonProperty("fanCount")]
        public int FanCount { get; set; }

        [JsonProperty("videoCount")]
        public int VideoCount { get; set; }

        [JsonProperty("postCount")]
        public int PostCount { get; set; }

        [JsonProperty("videoPlayCount")]
        public long TotalVideoPlayCount { get; set; }

        [JsonProperty("videoLikeCount")]
        public long TotalVideoLikeCount { get; set; }

        [JsonProperty("videoCommentCount")]
        public long TotalVideoCommentCount { get; set; }

        public class Tag
        {
            [JsonProperty("tagSeq")]
            public string Seq { get; set; }

            [JsonProperty("tagName")]
            public string Name { get; set; }
        }
    }
}

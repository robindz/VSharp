using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSharp.Models
{
    public class ChannelVideoListResponse
    {
        [JsonProperty("totalVideoCount")]
        public int TotalVideoCount { get; set; }

        [JsonProperty("videoList")]
        public List<Video> Videos { get; set; } = new List<Video>();
    }
}

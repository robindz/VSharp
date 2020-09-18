using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSharp.Models
{
    public class UpcomingVideoListResponse
    {
        [JsonProperty("totalVideoCount")]
        public int TotalVideoCount { get; set; }

        [JsonProperty("videoList", NullValueHandling = NullValueHandling.Ignore)]
        public List<Video> Videos { get; set; } = new List<Video>();
    }
}

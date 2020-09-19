using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSharp.Models.Post
{
    public class PostListResponse
    {
        [JsonProperty("paging")]
        public PagingParams Paging { get; set; }

        [JsonProperty("data")]
        public List<Post> Posts { get; set; }

        [JsonProperty("total_count")]
        public int Total { get; set; }
    }
}

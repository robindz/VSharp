using Newtonsoft.Json;
using System.Collections.Generic;
using VSharp.Converters;
using VSharp.Models.VLivePost;

namespace VSharp.Models
{
    public class PostResponse
    {
        [JsonProperty("paging")]
        [JsonConverter(typeof(PostResponseConverter))]
        public PostPagingParams PagingParams { get; set; }

        [JsonProperty("data")]
        public List<PostData> Data { get; set; }
    }
}

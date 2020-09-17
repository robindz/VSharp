using Newtonsoft.Json;
using VSharp.Converters;
using VSharp.Models.VLivePost;

namespace VSharp.Models
{
    public class VLivePostResponse
    {
        [JsonProperty("paging")]
        [JsonConverter(typeof(PostResponseConverter))]
        public VLivePostPagingParams PagingParams { get; set; }

        [JsonProperty("data")]
        public VLivePostData[] Data { get; set; }
    }
}

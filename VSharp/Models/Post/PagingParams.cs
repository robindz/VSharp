using Newtonsoft.Json;

namespace VSharp.Models.Post
{
    public class PagingParams
    {
#nullable enable
        [JsonProperty("next_params", NullValueHandling = NullValueHandling.Ignore)]
        public NextPagingParams? Next { get; set; }
#nullable disable
    }
}

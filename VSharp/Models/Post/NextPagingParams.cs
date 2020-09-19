using Newtonsoft.Json;

namespace VSharp.Models.Post
{
    public class NextPagingParams
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("after")]
        public long AfterEpochInMilliseconds { get; set; }

        [JsonProperty("app_id")]
        public string AppId { get; set; }
    }
}

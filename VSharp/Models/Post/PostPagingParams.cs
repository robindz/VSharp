using Newtonsoft.Json;

namespace VSharp.Models
{
    public class PostPagingParams
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("after")]
        public long AfterEpochInMilliseconds { get; set; }

        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("_")]
        public long NextEpochInMilliseconds { get; set; }
    }
}

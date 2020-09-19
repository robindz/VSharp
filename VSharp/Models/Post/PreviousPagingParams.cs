using Newtonsoft.Json;

namespace VSharp.Models.Post
{
    public class PreviousPagingParams
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("before")]
        public long PreviousEpochInMilliseconds { get; set; }

        [JsonProperty("app_id")]
        public string AppId { get; set; }
    }
}

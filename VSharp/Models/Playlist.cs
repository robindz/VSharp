using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSharp.Models
{
    public class Playlist
    {
        [JsonProperty("playlistSeq")]
        public int PlaylistSeq { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("playlistType")]
        public string PlaylistType { get; set; }

        [JsonProperty("playlistViewType")]
        public string PlaylistViewType { get; set; }

        [JsonProperty("thumbnail")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("representChannelName")]
        public string RepresentChannelName { get; set; }

        [JsonProperty("videoList")]
        public List<Video> Videos { get; set; } = new List<Video>();
    }
}

using Newtonsoft.Json;

namespace VSharp.Models
{
    public class VODInfo
    {
        [JsonProperty("tId")]
        public string TransactionId { get; set; }

        [JsonProperty("transactionCreatedTime")]
        public long TransationCreatedTimeEpochInMilliseconds { get; set; }

        [JsonProperty("meta")]
        public Metadata MetadataInfo { get; set; }

        [JsonProperty("videos")]
        public Video VideoInfo { get; set; }

        [JsonProperty("streams")]
        public Stream[] Streams { get; set; }
#nullable enable
        [JsonProperty("captions")]
        public Caption? CaptionInfo { get; set; }
#nullable disable
        [JsonProperty("thumbnails")]
        public Thumbnail ThumbnailInfo { get; set; }

        public class Metadata
        {
            [JsonProperty("masterVideoId")]
            public string MasterVideoId { get; set; }

            [JsonProperty("contentId")]
            public string ContentId { get; set; }

            [JsonProperty("count")]
            public int PlayCount { get; set; }

            [JsonProperty("interfaceLang")]
            public string InterfaceLanguage { get; set; }

            [JsonProperty("url")]
            public string VideoUrl { get; set; }

            [JsonProperty("subject")]
            public string Subject { get; set; }

            [JsonProperty("cover")]
            public Cover CoverInfo { get; set; }

            [JsonProperty("countryCode")]
            public string CountryCode { get; set; }

            public class Cover
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("source")]
                public string Source { get; set; }
            }
        }

        public class Video 
        { 
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("hasPreview")]
            public bool HasPreview { get; set; }

            [JsonProperty("isPreview")]
            public bool IsPreview { get; set; }

            [JsonProperty("canAutoPlay")]
            public bool CanAutoPlay { get; set; }

            [JsonProperty("dimension")]
            public string Dimension { get; set; }

            [JsonProperty("list")]
            public VideoDetails[] Videos { get; set; }

            public class VideoDetails
            {
                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("useP2P")]
                public bool UseP2P { get; set; }

                [JsonProperty("duration")]
                public double Duration { get; set; }

                [JsonProperty("previewDuration")]
                public int PreviewDuration { get; set; }

                [JsonProperty("size")]
                public long Size { get; set; }

                [JsonProperty("type")]
                public string CodecType { get; set; }

                [JsonProperty("encodingOption")]
                public Encoding EncodingInfo { get; set; }

                [JsonProperty("bitrate")]
                public Bitrate BitrateInfo { get; set; }

                [JsonProperty("p2pMetaUrl")]
                public string P2PMetaUrl { get; set; }

                [JsonProperty("p2pUrl")]
                public string P2PUrl { get; set; }

                [JsonProperty("source")]
                public string Source { get; set; }

                [JsonProperty("sourceFrom")]
                public string SourceFrom { get; set; }

                public class Encoding
                {
                    [JsonProperty("id")]
                    public string Id { get; set; }

                    [JsonProperty("name")]
                    public string Name { get; set; }

                    [JsonProperty("profile")]
                    public string Profile { get; set; }

                    [JsonProperty("width")]
                    public int Width { get; set; }

                    [JsonProperty("height")]
                    public int Height { get; set; }

                    [JsonProperty("isEncodingComplete")]
                    public bool IsEncodingComplete { get; set; }

                    [JsonProperty("completeProgress")]
                    public int CompleteProgress { get; set; }
                }

                public class Bitrate
                {
                    [JsonProperty("video")]
                    public double Video { get; set; }

                    [JsonProperty("audio")]
                    public double Audio { get; set; }
                }
            }
        }

        public class Stream
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("enableABR")]
            public string EnableABR { get; set; }

            [JsonProperty("dimension")]
            public string Dimension { get; set; }

            [JsonProperty("keys")]
            public StreamKey[] Keys { get; set; }
            
            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("sourceFrom")]
            public string SourceFrom { get; set; }

            [JsonProperty("videos")]
            public VideoDetails[] Videos { get; set; }

            public class StreamKey
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("value")]
                public string Value { get; set; }
            }

            public class VideoDetails
            {
                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("encodingOption")]
                public Encoding EncodingInfo { get; set; }

                [JsonProperty("bitrate")]
                public Bitrate BitrateInfo { get; set; }

                [JsonProperty("source")]
                public string Source { get; set; }

                [JsonProperty("sourceFrom")]
                public string SourceFrom { get; set; }

                [JsonProperty("template")]
                public Template TemplateInfo { get; set; }

                public class Encoding
                {
                    [JsonProperty("id")]
                    public string Id { get; set; }

                    [JsonProperty("name")]
                    public string Name { get; set; }

                    [JsonProperty("width")]
                    public int Width { get; set; }

                    [JsonProperty("height")]
                    public int Height { get; set; }
                }

                public class Bitrate
                {
                    [JsonProperty("video")]
                    public double Video { get; set; }

                    [JsonProperty("audio")]
                    public double Audio { get; set; }
                }

                public class Template
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("body")]
                    public TemplateBody Body { get; set; }

                    public class TemplateBody
                    {
                        [JsonProperty("bandwidth")]
                        public long Bandwidth { get; set; }

                        [JsonProperty("resolution")]
                        public string Resolution { get; set; }

                        [JsonProperty("format")]
                        public string Format { get; set; }

                        [JsonProperty("version")]
                        public string Version { get; set; }

                        [JsonProperty("mediaSequence")]
                        public int MediaSequence { get; set; }

                        [JsonProperty("extInfos")]
                        public double[] ExtInfos { get; set; }
                    }
                }
            }
        }

        public class Caption
        {
            [JsonProperty("captionLang")]
            public string CaptionLanguage { get; set; }

            [JsonProperty("list")]
            public CaptionDetails[] Details { get; set; }

            public class CaptionDetails
            {
                [JsonProperty("language")]
                public string Language { get; set; }

                [JsonProperty("country")]
                public string Country { get; set; }

                [JsonProperty("locale")]
                public string Locale { get; set; }

                [JsonProperty("label")]
                public string Label { get; set; }

                [JsonProperty("source")]
                public string Source { get; set; }
#nullable enable
                [JsonProperty("subLabel")]
                public string? SubLabel { get; set; }
#nullable disable
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("fanName")]
                public string FanName { get; set; }
            }
        }

        public class Thumbnail
        {
            [JsonProperty("list")]
            public ThumbnailDetails[] Details { get; set; }

            [JsonProperty("sprites")]
            public Sprite[] Sprites { get; set; }

            public class ThumbnailDetails
            {
                [JsonProperty("time")]
                public double Time { get; set; }

                [JsonProperty("source")]
                public string Source { get; set; }
            }

            public class Sprite
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("totalPage")]
                public int TotalPage { get; set; }

                [JsonProperty("rowCount")]
                public int RowCount { get; set; }

                [JsonProperty("columnCount")]
                public int ColumnCount { get; set; }

                [JsonProperty("intervalType")]
                public string IntervalType { get; set; }

                [JsonProperty("interval")]
                public int Interval { get; set; }

                [JsonProperty("thumbnailWidth")]
                public int ThumbnailWidth { get; set; }

                [JsonProperty("thumbnailHeight")]
                public int ThumbnailHeight { get; set; }

                [JsonProperty("allThumbnailCount")]
                public int AllThumbnailCount { get; set; }

                [JsonProperty("sourcePatternType")]
                public string SourcePatternType { get; set; }

                [JsonProperty("source")]
                public string Source { get; set; }
            }
        }
    }
}

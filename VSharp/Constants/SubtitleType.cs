namespace VSharp.Constants
{
    public sealed class SubtitleType
    {
        public static readonly SubtitleType Official = new SubtitleType("cp");
        public static readonly SubtitleType Fanmade = new SubtitleType("fan");
        public static readonly SubtitleType Auto = new SubtitleType("auto");

        private SubtitleType(string type)
        {
            Value = type;
        }

        public string Value { get; private set; }
    }
}

using System;

namespace VSharp.Models
{
    public class SubtitlesAvailableEventArgs : EventArgs
    {
        public int VideoSeq { get; private set; }
        public string Language { get; private set; }
        public string Country { get; private set; }
        public string Locale { get; private set; }
        public string Label { get; private set; }
        public string Source { get; private set; }
        public string Type { get; private set; }
        public string FanName { get; private set; }

        public SubtitlesAvailableEventArgs(int videoSeq, string language, string country, string locale, string label, string source, string type, string fanName)
        {
            VideoSeq = videoSeq;
            Language = language;
            Country = country;
            Locale = locale;
            Label = label;
            Source = source;
            Type = type;
            FanName = fanName;
        }
    }
}

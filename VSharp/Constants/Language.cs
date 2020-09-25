using System;

namespace VSharp.Constants
{
    [Serializable]
    public sealed class Language
    {
        public static readonly Language English = new Language("en", "English");
        public static readonly Language Spanish = new Language("es", "Spanish");
        public static readonly Language Indonesian = new Language("in", "Indonesian");
        public static readonly Language Vietnamese = new Language("vi", "Vietnamese");
        public static readonly Language Russian = new Language("ru", "Russian");
        public static readonly Language Thai = new Language("th", "Thai");
        public static readonly Language Japanese = new Language("ja", "Japanese");
        public static readonly Language Korean = new Language("ko", "Korean");

        private Language(string value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public string Value { get; private set; }
        public string DisplayName { get; private set; }
    }
}

using System;

namespace VSharp.Constants
{
    [Serializable]
    public sealed class Language
    {
        public static readonly Language English = new Language("en_US", "English");
        public static readonly Language Spanish = new Language("es_ES", "Spanish");
        public static readonly Language Indonesian = new Language("in_ID", "Indonesian");
        public static readonly Language Vietnamese = new Language("vi_VN", "Vietnamese");
        public static readonly Language Russian = new Language("ru_RU", "Russian");
        public static readonly Language Thai = new Language("th_TH", "Thai");
        public static readonly Language Japanese = new Language("ja_JP", "Japanese");
        public static readonly Language Korean = new Language("ko_KR", "Korean");
        public static readonly Language Italian = new Language("it_IT", "Italian");
        public static readonly Language Polish = new Language("pl_PL", "Polish");
        public static readonly Language Portuguese = new Language("pt_BR", "Portuguese");
        public static readonly Language Turkish = new Language("tr_TR", "Turkish");
        public static readonly Language ChineseSimplified = new Language("zh_CN", "Chinese");
        public static readonly Language ChineseTraditional = new Language("zh_TW", "Chinese");
        public static readonly Language Hindi = new Language("hi_IN", "Hindi");
        public static readonly Language German = new Language("de_DE", "German");
        public static readonly Language Dutch = new Language("nl_NL", "Dutch");
        public static readonly Language Arabic = new Language("ar_AE", "Arabic");
        public static readonly Language Burmese = new Language("my", "Burmese");
        public static readonly Language Filipino = new Language("tl", "Filipino");
        public static readonly Language French = new Language("fr_FR", "French");
        public static readonly Language Romanian = new Language("ro_RO", "Romanian");

        private Language(string value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public string Value { get; private set; }
        public string DisplayName { get; private set; }
    }
}

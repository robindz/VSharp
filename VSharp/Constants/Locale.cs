namespace VSharp.Constants
{
    public sealed class Locale
    {
        public static readonly Locale EN = new Locale("en");
        public static readonly Locale KO = new Locale("ko");

        private Locale(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}

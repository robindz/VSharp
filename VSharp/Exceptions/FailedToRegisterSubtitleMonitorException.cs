using static VSharp.VSharpMonitor;

namespace VSharp.Exceptions
{
    public class FailedToRegisterSubtitleMonitorException : FailedToRegisterException
    {
        public int VideoSeq { get; private set; }
        public string Language { get; private set; }
        public string Type { get; private set; }

        public FailedToRegisterSubtitleMonitorException(int videoSeq, string language, string type, Monitor monitor, string message) : base(monitor, message)
        {
            VideoSeq = videoSeq;
            Language = language;
            Type = type;
        }
    }
}

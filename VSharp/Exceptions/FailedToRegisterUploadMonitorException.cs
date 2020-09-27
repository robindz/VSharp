using static VSharp.VSharpMonitor;

namespace VSharp.Exceptions
{
    public class FailedToRegisterUploadMonitorException : FailedToRegisterException
    {
        public int ChannelSeq { get; set; }

        public FailedToRegisterUploadMonitorException(int channelSeq, Monitor monitor, string message) : base(monitor, message)
        {
            ChannelSeq = channelSeq;
        }
    }
}

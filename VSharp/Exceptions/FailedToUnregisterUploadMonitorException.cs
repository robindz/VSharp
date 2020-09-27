using static VSharp.VSharpMonitor;

namespace VSharp.Exceptions
{
    public class FailedToUnregisterUploadMonitorException : FailedToUnregisterException
    {
        public int ChannelSeq { get; set; }

        public FailedToUnregisterUploadMonitorException(int channelSeq, Monitor monitor, string message) : base(monitor, message)
        {
            ChannelSeq = channelSeq;
        }
    }
}

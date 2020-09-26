using static VSharp.VSharpMonitor;

namespace VSharp.Exceptions
{
    public class FailedToRegisterLiveMonitorException : FailedToRegisterException
    {
        public int ChannelSeq { get; set; }

        public FailedToRegisterLiveMonitorException(int channelSeq, Monitor monitor, string message) : base(monitor, message)
        {
            ChannelSeq = channelSeq;
        }
    }
}

using static VSharp.VSharpMonitor;

namespace VSharp.Exceptions
{
    public class FailedToUnregisterLiveMonitorException : FailedToUnregisterException
    {
        public int ChannelSeq { get; set; }
        
        public FailedToUnregisterLiveMonitorException(int channelSeq, Monitor monitor, string message) : base(monitor, message)
        {
            ChannelSeq = channelSeq;
        }
    }
}

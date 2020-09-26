using System;

namespace VSharp.Models.Monitoring
{
    internal class AChannelMonitorTask : AMonitorTask, IChannelMonitorTask
    {
        public int ChannelSeq { get; set; }

        public AChannelMonitorTask(int channelSeq, TimeSpan period, VSharpService service) : base(period, service)
        {
            ChannelSeq = channelSeq;
        }
    }
}

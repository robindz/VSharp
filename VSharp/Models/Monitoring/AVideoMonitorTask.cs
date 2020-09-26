using System;

namespace VSharp.Models.Monitoring
{
    internal abstract class AVideoMonitorTask : AMonitorTask, IVideoMonitorTask
    {
        public int VideoSeq { get; set; }

        public AVideoMonitorTask(int videoSeq, TimeSpan period, VSharpService service) : base(period, service)
        {
            VideoSeq = videoSeq;
        }
    }
}

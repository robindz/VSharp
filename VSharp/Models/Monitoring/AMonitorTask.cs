using System;
using System.Threading;

namespace VSharp.Models.Monitoring
{
    internal abstract class AMonitorTask : IMonitorTask
    {
        public bool Checking { get; set; }
        public Timer Timer { get; set; }
        public VSharpService Service { get; set; }

        private readonly TimeSpan _period;

        public AMonitorTask(TimeSpan period, VSharpService service)
        {
            Service = service;
            _period = period;
        }

        public void StartTask()
            => Timer.Change(TimeSpan.Zero, _period);

        public void StopTask()
            => Timer.Change(Timeout.Infinite, Timeout.Infinite);
    }
}

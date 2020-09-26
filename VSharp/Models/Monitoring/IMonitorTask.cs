using System.Threading;

namespace VSharp.Models.Monitoring
{
    internal interface IMonitorTask
    {
        bool Checking { get; set; }
        VSharpService Service { get; set; }
        Timer Timer { get; set; }

        void StartTask();
        void StopTask();
    }
}

namespace VSharp.Models.Monitoring
{
    internal interface IVideoMonitorTask : IMonitorTask
    {
        int VideoSeq { get; set; }
    }
}

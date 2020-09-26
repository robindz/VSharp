namespace VSharp.Models.Monitoring
{
    internal interface IChannelMonitorTask : IMonitorTask
    {
        int ChannelSeq { get; set; }
    }
}

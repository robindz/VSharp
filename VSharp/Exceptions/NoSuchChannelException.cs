using System;

namespace VSharp.Exceptions
{
    public class NoSuchChannelException : Exception
    {
        public NoSuchChannelException(int channelSeq) : base($"Channel with channelSeq {channelSeq} was not found.")
        {
        }
    }
}

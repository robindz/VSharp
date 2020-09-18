using System;

namespace VSharp.Exceptions
{
    public class UndecodableChannelCodeException : Exception
    {
        public UndecodableChannelCodeException(string channelCode) : base($"The channel code {channelCode} does not resolve to a known channel.")
        {
        }
    }
}

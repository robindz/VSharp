using System;
using static VSharp.VSharpMonitor;

namespace VSharp.Exceptions
{
    public abstract class FailedToUnregisterException : Exception
    {
        public Monitor Monitor { get; private set; }

        public FailedToUnregisterException(Monitor monitor, string message) : base(message)
        {
            Monitor = monitor;
        }
    }
}

using System;
using static VSharp.VSharpMonitor;

namespace VSharp.Exceptions
{
    public abstract class FailedToRegisterException : Exception
    {
        public Monitor Monitor { get; private set; }

        public FailedToRegisterException(Monitor monitor, string message) : base(message)
        {
            Monitor = monitor;
        }
    }
}

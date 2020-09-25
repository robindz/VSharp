using System;

namespace VSharp.Exceptions
{
    public class ResourceUnavailableException : Exception
    {
        public ResourceUnavailableException() : base("The resource you tried to access is not available. It either is not available yet, or you don't have sufficient permissions to access this resource.")
        {
        }
    }
}

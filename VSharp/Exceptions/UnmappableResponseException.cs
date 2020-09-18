using System;

namespace VSharp.Exceptions
{
    public class UnmappableResponseException : Exception
    {
        public UnmappableResponseException() : base("The server responded with data that could not be mapped to an object.")
        {
        }
    }
}

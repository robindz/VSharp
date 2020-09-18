using System;

namespace VSharp.Exceptions
{
    public class InternalServerErrorException : Exception
    {
#nullable enable
        public string? ErrorDate { get; set; }
#nullable disable

        public InternalServerErrorException(string errorData) : base($"The server responded with a 500 status code.\n{errorData}")
        {
            ErrorDate = errorData;
        }

        public InternalServerErrorException() : base("The server responded with a 500 status code.")
        {
        }
    }
}

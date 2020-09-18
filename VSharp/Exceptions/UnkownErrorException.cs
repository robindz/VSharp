using System;

namespace VSharp.Exceptions
{
    public class UnkownErrorException : Exception
    {
#nullable enable
        public string? ErrorData { get; private set; }
#nullable disable

        public UnkownErrorException(string errorData) : base($"An unkown error occured and your request failed to complete successfully.\n{errorData}")
        {
            ErrorData = errorData;
        }

        public UnkownErrorException() : base("An unkown error occured and your request failed to complete successfully.")
        {
        }
    }
}

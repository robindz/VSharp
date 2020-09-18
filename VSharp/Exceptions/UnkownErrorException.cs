using System;

namespace VSharp.Exceptions
{
    internal class UnkownErrorException : Exception
    {
#nullable enable
        public string? ErrorData { get; private set; }
#nullable disable

        public UnkownErrorException(string errorData) : base($"An unkown error occured and your request failed to complete successfully.")
        {
            ErrorData = errorData;
        }

        public UnkownErrorException() : base($"An unkown error occured and your request failed to complete successfully.")
        {
        }
    }
}

using System;

namespace VSharp.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string appId) : base($"Applcations with app_id {appId} have no access to this resource.")
        {
        }
    }
}

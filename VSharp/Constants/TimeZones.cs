using System;
using System.Runtime.InteropServices;

namespace VSharp.Constants
{
    public static class TimeZones
    {
        public static TimeZoneInfo KST
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
                else
                    return TimeZoneInfo.FindSystemTimeZoneById("Asia/Seoul");
            }
        }
    }
}

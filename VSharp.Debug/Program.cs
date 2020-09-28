using System;
using System.Threading.Tasks;
using VSharp.Constants;
using VSharp.Models;

namespace VSharp.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            VSharpService service = new VSharpService("app_id", Locale.EN);
            VSharpMonitor monitor = new VSharpMonitor(service);
            await Task.Delay(-1);
        }
    }
}

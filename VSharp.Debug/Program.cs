using System;
using System.Threading.Tasks;
using VSharp.Constants;

namespace VSharp.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            VSharpService service = new VSharpService("app_id", Locale.KO);
            VSharpMonitor monitor = new VSharpMonitor(service);
            Console.ReadKey();
        }
    }
}

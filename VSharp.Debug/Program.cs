using System;
using System.Threading.Tasks;
using VSharp.Constants;

namespace VSharp.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            VSharpService service = new VSharpService("8c6cc7b45d2568fb668be6e05b6e5a3b", Locale.KO);
            VSharpMonitor monitor = new VSharpMonitor(service);
            Console.ReadKey();
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VSharp.Constants;
using VSharp.Models.Post;

namespace VSharp.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            VSharpService service = new VSharpService("app_id", Locale.KO);

            VSharpMonitor monitor = new VSharpMonitor(service);
            monitor.RegisterLiveMonitor(458, 10, TimeSpan.FromSeconds(30));
            

            monitor.SubtitleAvailable += Monitor_SubtitleAvailable1;
            monitor.LiveFound += Monitor_LiveFound;
            monitor.ExceptionThrown += Monitor_ErrorOccured;

            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
        }

        private static void Monitor_LiveFound(object sender, Models.Events.LiveFoundEventArgs e)
        {
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));
        }

        private static void Monitor_SubtitleAvailable1(object sender, Models.Events.SubtitlesAvailableEventArgs e)
        {
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));
        }

        private static void Monitor_ErrorOccured(object sender, Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

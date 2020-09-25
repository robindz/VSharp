using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VSharp.Constants;

namespace VSharp.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            VSharpService service = new VSharpService("app_id");
            
            
            VSharpMonitor monitor = new VSharpMonitor(service);
            monitor.RegisterSubtitleMonitor(214499, Language.Vietnamese, SubtitleType.Official, TimeSpan.FromSeconds(1));
            monitor.RegisterSubtitleMonitor(214499, Language.Korean, SubtitleType.Official, TimeSpan.FromSeconds(1));
            monitor.RegisterSubtitleMonitor(214499, Language.Japanese, SubtitleType.Official, TimeSpan.FromSeconds(1));



            monitor.SubtitleAvailable += Monitor_SubtitleAvailable;
            monitor.ErrorOccured += Monitor_ErrorOccured;

            Console.ReadKey();
            Console.ReadKey();
        }

        private static void Monitor_SubtitleAvailable(object sender, Models.SubtitlesAvailableEventArgs e)
        {
            Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));
        }

        private static void Monitor_ErrorOccured(object sender, Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

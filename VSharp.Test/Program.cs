using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VSharp.Test
{
    public class Program
    {
        // TODO: remove PostResponseConverter
        public static async Task Main(string[] args)
        {
            VLiveService service = new VLiveService("app_id");

            Console.ReadKey();
        }
    }
}

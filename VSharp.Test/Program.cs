using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VSharp.Test
{
    public class Program
    {
        // TODO: remove PostResponseConverter
        public static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource(99999999);
            List<string> list = new List<string>();
            VLiveService service = new VLiveService("8c6cc7b45d2568fb668be6e05b6e5a3b");

            var data = await service.GetChannelVideoListAsync(6, 1, 1);
            list.Add(JsonConvert.SerializeObject(data));

            Console.ReadKey();
        }
    }
}

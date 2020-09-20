using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VSharp.Models.Post;

namespace VSharp.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource(99999999);
            List<string> list = new List<string>();
            VSharpService service = new VSharpService("app_id");

            List<Post> posts = new List<Post>();

            var status = await service.GetVideoStatusAsync(213927);
            Console.WriteLine(JsonConvert.SerializeObject(status, Formatting.Indented));
            Console.ReadKey();
        }
    }
}

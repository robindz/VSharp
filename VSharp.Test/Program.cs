using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace VSharp.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            VLiveService service = new VLiveService("app_id");
            var response = await service.DecodeChannelCodeAsync("EDBF");

            string rawData = JsonConvert.SerializeObject(response, Formatting.Indented);
            Console.WriteLine(rawData);

            Console.ReadKey();
        }
    }
}

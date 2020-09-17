using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VSharp.Models;

namespace VSharp.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string json = await client.GetStringAsync("https://api-vfan.vlive.tv/v3/board.21/posts?&app_id=x&limit=20&_=1600374693642");
            VLivePostResponse postResponse = JsonConvert.DeserializeObject<VLivePostResponse>(json);

            json = JsonConvert.SerializeObject(postResponse, Formatting.Indented);
            Console.WriteLine(json);
            Console.ReadKey();
        }
    }
}

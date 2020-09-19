using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VSharp.Models;
using VSharp.Models.Post;

namespace VSharp.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource(99999999);
            List<string> list = new List<string>();
            VLiveService service = new VLiveService("8c6cc7b45d2568fb668be6e05b6e5a3b");

            List<Post> posts = new List<Post>();

            var iterator = service.CreatePostListIterator(35, 10);
            while (iterator.HasNext())
            {
                posts.AddRange((await iterator.FetchNextAsync()).Posts);
                Console.WriteLine(posts.Count);
            }

            while (iterator.HasPrevious())
            {
                posts.AddRange((await iterator.FetchPreviousAsync()).Posts);
                Console.WriteLine(posts.Count);
            }

            posts = posts.Distinct().ToList();

            Console.ReadKey();
        }
    }
}

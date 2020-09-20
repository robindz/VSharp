using System.Threading.Tasks;
using VSharp.Models.Post;

namespace VSharp.Iterators
{
    public class PostListIterator
    {
        private readonly VSharpService _service;
        private readonly int _board;
        private readonly int _count;

        private string next;
        private bool hasNext;

        public PostListIterator(VSharpService service, int board, int count)
        {
            _service = service;
            _board = board;
            _count = count;
            hasNext = true;
        }

        public bool HasNext() => hasNext;

        public async Task<PostListResponse> FetchNextAsync()
        {
            if (!hasNext)
                return null;

            PostListResponse postListResponse;
            if (string.IsNullOrEmpty(next))
                postListResponse = await _service.GetPostListAsync(_board, _count);
            else
                postListResponse = await _service.GetPostListAfterAsync(_board, _count, next);

            if (postListResponse.Paging.Next != null)
                next = postListResponse.Paging.Next.AfterEpochInMilliseconds.ToString();

            if (postListResponse.Posts.Count < _count)
                hasNext = false;

            return postListResponse;
        }
    }
}

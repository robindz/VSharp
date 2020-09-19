using System;
using System.Threading.Tasks;
using VSharp.Models.Post;

namespace VSharp.Iterators
{
    public class PostListIterator
    {
        private readonly VLiveService _service;
        private readonly int _board;
        private readonly int _count;

        private string next;
        private string previous;
        private bool hasNext;
        private bool hasPrevious;

        public PostListIterator(VLiveService service, int board, int count)
        {
            if (board <= 0)
                throw new ArgumentException("board must be a strictly positive integer.");
            if (count <= 0)
                throw new ArgumentException("count must be a strictly positive integer.");

            _service = service;
            _board = board;
            _count = count;
            hasNext = true;
        }

        public bool HasNext() => hasNext;

        public bool HasPrevious() => hasPrevious;

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
            {
                next = postListResponse.Paging.Next.AfterEpochInMilliseconds.ToString();
                hasNext = true;
            }
            else
            {
                next = string.Empty;
                hasNext = false;
            }
            if (postListResponse.Paging.Previous != null)
            {
                previous = postListResponse.Paging.Previous.PreviousEpochInMilliseconds.ToString();
                hasPrevious = true;
            }
            else
            {
                previous = string.Empty;
                hasPrevious = false;
            }

            return postListResponse;
        }

        public async Task<PostListResponse> FetchPreviousAsync()
        {
            if (!hasPrevious)
                return null;

            PostListResponse postListResponse;
            if (string.IsNullOrEmpty(previous))
                postListResponse = await _service.GetPostListAsync(_board, _count);
            else
                postListResponse = await _service.GetPostListBeforeAsync(_board, _count, previous);

            if (postListResponse.Paging.Next != null)
            {
                next = postListResponse.Paging.Next.AfterEpochInMilliseconds.ToString();
                hasNext = true;
            }
            else
            {
                next = string.Empty;
                hasNext = false;
            }
            if (postListResponse.Paging.Previous != null)
            {
                previous = postListResponse.Paging.Previous.PreviousEpochInMilliseconds.ToString();
                hasPrevious = true;
            }
            else
            {
                previous = string.Empty;
                hasPrevious = false;
            }

            return postListResponse;
        }
    }
}

﻿using System.Threading.Tasks;
using VSharp.Models;

namespace VSharp.Iterators
{
    public class ChannelVideoListIterator
    {
        private readonly VSharpService _service;
        private readonly int _channelSeq;
        private readonly int _count;

        private int currentPage;
        private bool hasNext;

        public ChannelVideoListIterator(VSharpService service, int channelSeq, int count)
        {
            _service = service;
            _channelSeq = channelSeq;
            _count = count;
            currentPage = 1;
            hasNext = true;
        }

        public bool HasNext() => hasNext;

        public async Task<ChannelVideoListResponse> FetchNextAsync()
        {
            if (!hasNext)
                return null;

            ChannelVideoListResponse channelVideoListResponse = await _service.GetChannelVideoListAsync(_channelSeq, _count, currentPage);

            if (channelVideoListResponse.Videos.Count < _count)
                hasNext = false;

            currentPage++;

            return channelVideoListResponse;
        }
    }
}

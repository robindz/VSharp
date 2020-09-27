using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using VSharp.Constants;
using VSharp.Exceptions;
using VSharp.Models;

namespace VSharp.Testing
{
    [TestClass]
    public class GetChannelVideoListTest
    {
        private readonly VSharpService service = new VSharpService("8c6cc7b45d2568fb668be6e05b6e5a3b", Locale.EN);
        private const string TWICE_CHANNEL_CODE = "EDBF";
        private const int TWICE_CHANNEL_SEQ = 6;
        private const int INVALID_CHANNEL_SEQ = int.MaxValue;
        private const int INVALID_NEGATIVE_CHANNEL_SEQ = int.MinValue;

        [TestMethod]
        public async Task GetChannelVideoList_OK()
        {
            int count = 5;
            ChannelVideoListResponse channelVideoListResponse = await service.GetChannelVideoListAsync(TWICE_CHANNEL_SEQ, count, 1);
            Assert.AreEqual(channelVideoListResponse.Videos.Count, count);
            Assert.AreEqual(channelVideoListResponse.ChannelInfo.ChannelCode, TWICE_CHANNEL_CODE);
            Assert.AreEqual(channelVideoListResponse.ChannelInfo.ChannelSeq, TWICE_CHANNEL_SEQ);
        }

        [TestMethod]
        public async Task GetChannelVideoList_InternalServerErrorException_By_Invalid_ChannelSeq()
        {
            int count = 5;
            await Assert.ThrowsExceptionAsync<InternalServerErrorException>(async () =>
            {
                ChannelVideoListResponse channelVideoListResponse = await service.GetChannelVideoListAsync(INVALID_CHANNEL_SEQ, count, 1);
            });
        }

        [TestMethod]
        public async Task GetChannelVideoList_ArgumentException_By_Negative_Invalid_ChannelSeq()
        {
            int count = 5;
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                ChannelVideoListResponse channelVideoListResponse = await service.GetChannelVideoListAsync(INVALID_NEGATIVE_CHANNEL_SEQ, count, 1);
            });
        }

        [TestMethod]
        public async Task GetChannelVideoList_ArgumentException_By_Negative_Count()
        {
            int count = int.MinValue, page = 1;
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                ChannelVideoListResponse channelVideoListResponse = await service.GetChannelVideoListAsync(INVALID_NEGATIVE_CHANNEL_SEQ, count, page);
            });
        }

        [TestMethod]
        public async Task GetChannelVideoList_ArgumentException_By_Negative_Page()
        {
            int count = 5, page = -1;
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                ChannelVideoListResponse channelVideoListResponse = await service.GetChannelVideoListAsync(INVALID_NEGATIVE_CHANNEL_SEQ, count, page);
            });
        }

        [TestMethod]
        public async Task GetChannelVideoList_TaskCanceledException()
        {
            CancellationTokenSource cts = new CancellationTokenSource(100);
            int count = 5;
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () =>
            {
                ChannelVideoListResponse channelVideoListResponse = await service.GetChannelVideoListAsync(TWICE_CHANNEL_SEQ, count, 1, cts.Token);
            });
        }
    }
}

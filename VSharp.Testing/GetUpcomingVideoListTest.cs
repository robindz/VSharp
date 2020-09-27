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
    public class GetUpcomingVideoListTest
    {
        private readonly VSharpService service = new VSharpService("8c6cc7b45d2568fb668be6e05b6e5a3b", Locale.EN);
        private const int TWICE_CHANNEL_SEQ = 6;
        private const int ARIRANG_CHANNEL_SEQ = 967;
        private const int INVALID_CHANNEL_SEQ = int.MaxValue;
        private const int INVALID_NEGATIVE_CHANNEL_SEQ = int.MinValue;

        [TestMethod]
        public async Task GetUpcomingVideoList_EMPTY_OK()
        {
            UpcomingVideoListResponse upcomingVideoListResponse = await service.GetUpcomingVideoListAsync(TWICE_CHANNEL_SEQ, 10, 1);
            Assert.AreEqual(upcomingVideoListResponse.TotalVideoCount, 0);
            Assert.AreEqual(upcomingVideoListResponse.Videos.Count, 0);
        }

        [TestMethod]
        public async Task GetUpcomingVideoList_ARIRANG_OK()
        {
            UpcomingVideoListResponse upcomingVideoListResponse = await service.GetUpcomingVideoListAsync(ARIRANG_CHANNEL_SEQ, 5, 1);
            Assert.AreEqual(upcomingVideoListResponse.TotalVideoCount, 5);
            Assert.AreEqual(upcomingVideoListResponse.Videos.Count, 5);
        }

        [TestMethod]
        public async Task GetUpcomingVideoList_InternalServerErrorException_By_Invalid_Channel_Seq()
        {
            await Assert.ThrowsExceptionAsync<InternalServerErrorException>(async () => {
                UpcomingVideoListResponse upcomingVideoListResponse = await service.GetUpcomingVideoListAsync(INVALID_CHANNEL_SEQ, 10, 1);
            });
        }

        [TestMethod]
        public async Task GetUpcomingVideoList_ArgumentException_By_Negative_Invalid_Channel_Seq()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => {
                UpcomingVideoListResponse upcomingVideoListResponse = await service.GetUpcomingVideoListAsync(INVALID_NEGATIVE_CHANNEL_SEQ, 10, 1);
            });
        }

        [TestMethod]
        public async Task GetUpcomingVideoList_ArgumentException_By_Negative_Count()
        {
            int count = int.MinValue, page = 1;
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                UpcomingVideoListResponse channelVideoListResponse = await service.GetUpcomingVideoListAsync(INVALID_NEGATIVE_CHANNEL_SEQ, count, page);
            });
        }

        [TestMethod]
        public async Task GetUpcomingVideoList_ArgumentException_By_Negative_Page()
        {
            int count = 5, page = -1;
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                UpcomingVideoListResponse channelVideoListResponse = await service.GetUpcomingVideoListAsync(INVALID_NEGATIVE_CHANNEL_SEQ, count, page);
            });
        }

        [TestMethod]
        public async Task GetUpcomingVideoList_TaskCanceledException()
        {
            CancellationTokenSource cts = new CancellationTokenSource(100);
            int count = 5;
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () =>
            {
                UpcomingVideoListResponse channelVideoListResponse = await service.GetUpcomingVideoListAsync(TWICE_CHANNEL_SEQ, count, 1, cts.Token);
            });
        }
    }
}

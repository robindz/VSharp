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
    public class GetChannelTest
    {
        private readonly VSharpService service = new VSharpService("8c6cc7b45d2568fb668be6e05b6e5a3b", Locale.EN);
        private const string TWICE_CHANNEL_CODE = "EDBF";
        private const int TWICE_CHANNEL_SEQ = 6;
        private const int INVALID_CHANNEL_SEQ = int.MaxValue;
        private const int INVALID_NEGATIVE_CHANNEL_SEQ = int.MinValue;

        [TestMethod]
        public async Task GetChannel_OK()
        {
            Channel channel = await service.GetChannelAsync(TWICE_CHANNEL_SEQ);
            Assert.AreEqual(channel.ChannelCode, TWICE_CHANNEL_CODE);
            Assert.AreEqual(channel.ChannelSeq, TWICE_CHANNEL_SEQ);
        }

        [TestMethod]
        public async Task GetChannel_NoSuchChannelException()
        {
            await Assert.ThrowsExceptionAsync<NoSuchChannelException>(async () =>
            {
                Channel channel = await service.GetChannelAsync(INVALID_CHANNEL_SEQ);
            });
        }

        [TestMethod]
        public async Task GetChannel_ArgumentException_By_Negative_ChannelSeq()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                Channel channel = await service.GetChannelAsync(INVALID_NEGATIVE_CHANNEL_SEQ);
            });
        }

        [TestMethod]
        public async Task GetChannel_TaskCanceledException()
        {
            CancellationTokenSource cts = new CancellationTokenSource(100);
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => {
                Channel channel = await service.GetChannelAsync(TWICE_CHANNEL_SEQ, cts.Token);
            });
        }
    }
}

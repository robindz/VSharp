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
    public class DecodeChannelCodeTests
    {
        private readonly VSharpService service = new VSharpService("8c6cc7b45d2568fb668be6e05b6e5a3b", Locale.EN);
        private const string TWICE_CHANNEL_CODE = "EDBF";
        private const string ARGUMENT_EXCEPTION_CHANNEL_CODE = "+-*/*%(){}|_@&!";
        private const string UNDECODABLE_CHANNEL_CODE = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
        private const int TWICE_CHANNEL_SEQ = 6;

        [TestMethod]
        public async Task DecodeChannelCode_OK()
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await service.DecodeChannelCodeAsync(TWICE_CHANNEL_CODE);
            Assert.AreEqual(TWICE_CHANNEL_CODE, decodeChannelCodeResponse.ChannelCode);
            Assert.AreEqual(TWICE_CHANNEL_SEQ, decodeChannelCodeResponse.ChannelSeq);
        }

        [TestMethod]
        public async Task DecodeChannelCode_ArgumentException_By_Invalid_Channel_Code()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => {
                DecodeChannelCodeResponse decodeChannelCodeResponse = await service.DecodeChannelCodeAsync(ARGUMENT_EXCEPTION_CHANNEL_CODE);
            });
        }

        [TestMethod]
        public async Task DecodeChannelCode_UndecodableChannelCodeException()
        {
            await Assert.ThrowsExceptionAsync<UndecodableChannelCodeException>(async () => {
                DecodeChannelCodeResponse decodeChannelCodeResponse = await service.DecodeChannelCodeAsync(UNDECODABLE_CHANNEL_CODE);
            });
        }

        [TestMethod]
        public async Task DecodeChannelCode_TaskCanceledException()
        {
            CancellationTokenSource cts = new CancellationTokenSource(100);
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => {
                DecodeChannelCodeResponse decodeChannelCodeResponse = await service.DecodeChannelCodeAsync(TWICE_CHANNEL_CODE, cts.Token);
            });
        }
    }
}

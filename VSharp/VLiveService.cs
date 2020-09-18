using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VSharp.Exceptions;
using VSharp.Models;

namespace VSharp
{
    public class VLiveService
    {
        private readonly HttpClient _http;
        private readonly string _appId;
        private readonly string _userAgent;
        private const string _decodeChannelCodeEndpoint = "http://api.vfan.vlive.tv/vproxy/channelplus/decodeChannelCode?app_id={0}&channelCode={1}";

        public VLiveService(string appId)
        {
            _appId = appId;
            _userAgent = "VSharp VLiveService 1.0.0";

            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("User-Agent", _userAgent);
        }

        public VLiveService(string appId, string userAgent)
        {
            _appId = appId;
            _userAgent = userAgent;

            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("User-Agent", _userAgent);
        }

        public async Task<DecodeChannelCodeResponse> DecodeChannelCodeAsync(string channelCode)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_decodeChannelCodeEndpoint, _appId, channelCode));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(_appId);
                }
                else
                {
                    string errorData = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(errorData) || string.IsNullOrWhiteSpace(errorData))
                        throw new UnkownErrorException();
                    else
                        throw new UnkownErrorException(errorData);
                }
            }

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
            {
                throw new UndecodableChannelCodeException(channelCode);
            }

            DecodeChannelCodeResponse decodeChannelCodeResponse = JsonConvert.DeserializeObject<DecodeChannelCodeResponse>(responseText);
            return decodeChannelCodeResponse;
        }
    }
}

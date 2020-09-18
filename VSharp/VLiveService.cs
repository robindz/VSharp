using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
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
        private const string _channelInfoEndpoint = "https://api-vfan.vlive.tv/v2/channel.{0}?app_id={1}&fields=channel_seq,channel_code,type,channel_name,comment,fan_count,channel_cover_img,channel_profile_img,representative_color,background_color,celeb_boards,fan_boards,is_show_banner,vstore,is_show_upcoming,media_channel,banner,gfp_ad_enabled,banner_ad_enabled,ad_channel_id,ad_cp_id,fanclub,agency_seq,channel_announce";
        private const string _channelVideoListEndpoint = "https://api-vfan.vlive.tv/vproxy/channelplus/getChannelVideoList?app_id={0}&channelSeq={1}&maxNumOfRows={2}&pageNo={3}";
        private const string _upcomingVideoListEndpoint = "http://api.vfan.vlive.tv/vproxy/channelplus/getUpcomingVideoList?app_id={0}&channelSeq={1}&maxNumOfRows={2}";

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

        #region DecodeChannelCode
        public async Task<DecodeChannelCodeResponse> DecodeChannelCodeAsync(string channelCode)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_decodeChannelCodeEndpoint, _appId, channelCode));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UndecodableChannelCodeException(channelCode);

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null 
            || (responseTextDynamic != null && responseTextDynamic["result"] == null))
                throw new UnkownErrorException();

            DecodeChannelCodeResponse decodeChannelCodeResponse = JsonConvert.DeserializeObject<DecodeChannelCodeResponse>(responseTextDynamic["result"].ToString());
            if (decodeChannelCodeResponse == null)
                throw new UnmappableResponseException();

            return decodeChannelCodeResponse;
        }

        public async Task<DecodeChannelCodeResponse> DecodeChannelCodeAsync(string channelCode, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_decodeChannelCodeEndpoint, _appId, channelCode));
            HttpResponseMessage response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UndecodableChannelCodeException(channelCode);

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null 
            || (responseTextDynamic != null && responseTextDynamic["result"] == null))
                throw new UnkownErrorException();

            DecodeChannelCodeResponse decodeChannelCodeResponse = JsonConvert.DeserializeObject<DecodeChannelCodeResponse>(responseTextDynamic["result"].ToString());
            if (decodeChannelCodeResponse == null)
                throw new UnmappableResponseException();

            return decodeChannelCodeResponse;
        }
        #endregion

        #region GetChannelInfo
        public async Task<Channel> GetChannelAsync(string channelCode)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode);
            return await GetChannelAsync(decodeChannelCodeResponse.ChannelSeq);
        }

        public async Task<Channel> GetChannelAsync(string channelCode, CancellationToken cancellationToken)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode, cancellationToken);
            return await GetChannelAsync(decodeChannelCodeResponse.ChannelSeq, cancellationToken);
        }

        public async Task<Channel> GetChannelAsync(int channelSeq)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_channelInfoEndpoint, channelSeq, _appId));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic != null 
             && responseTextDynamic["error"] != null 
             && responseTextDynamic["error"]["error_code"] != null 
             && (int)responseTextDynamic["error"]["error_code"] == 1001)
                throw new NoSuchChannelException(channelSeq);

            Channel channel = JsonConvert.DeserializeObject<Channel>(responseText);
            if (channel == null)
                throw new UnmappableResponseException();

            return channel;
        }

        public async Task<Channel> GetChannelAsync(int channelSeq, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_channelInfoEndpoint, channelSeq, _appId));
            HttpResponseMessage response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic != null 
             && responseTextDynamic["error"] != null 
             && responseTextDynamic["error"]["error_code"] != null 
             && (int)responseTextDynamic["error"]["error_code"] == 1001)
                throw new NoSuchChannelException(channelSeq);

            Channel channel = JsonConvert.DeserializeObject<Channel>(responseText);
            if (channel == null)
                throw new UnmappableResponseException();

            return channel;
        }
        #endregion

        #region GetChannelVideoList
        public async Task<ChannelVideoListResponse> GetChannelVideoListAsync(string channelCode, int maximumVideos, int page)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode);
            return await GetChannelVideoListAsync(decodeChannelCodeResponse.ChannelSeq, maximumVideos, page);
        }

        public async Task<ChannelVideoListResponse> GetChannelVideoListAsync(string channelCode, int maximumVideos, int page, CancellationToken cancellationToken)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode, cancellationToken);
            return await GetChannelVideoListAsync(decodeChannelCodeResponse.ChannelSeq, maximumVideos, page, cancellationToken);
        }

        public async Task<ChannelVideoListResponse> GetChannelVideoListAsync(int channelSeq, int maximumVideos, int page)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_channelVideoListEndpoint, _appId, channelSeq, maximumVideos, page));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["result"] == null))
                throw new UnkownErrorException();

            ChannelVideoListResponse channelVideoListResponse = JsonConvert.DeserializeObject<ChannelVideoListResponse>(responseTextDynamic["result"].ToString());
            if (channelVideoListResponse == null)
                throw new UnmappableResponseException();

            return channelVideoListResponse;
        }

        public async Task<ChannelVideoListResponse> GetChannelVideoListAsync(int channelSeq, int maximumVideos, int page, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_channelVideoListEndpoint, _appId, channelSeq, maximumVideos, page));
            HttpResponseMessage response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["result"] == null))
                throw new UnkownErrorException();

            ChannelVideoListResponse channelVideoListResponse = JsonConvert.DeserializeObject<ChannelVideoListResponse>(responseTextDynamic["result"].ToString());
            if (channelVideoListResponse == null)
                throw new UnmappableResponseException();

            return channelVideoListResponse;
        }
        #endregion

        #region GetUpcomingVideoList
        public async Task<UpcomingVideoListResponse> GetUpcomingVideoListAsync(string channelCode, int maximumVideos)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode);
            return await GetUpcomingVideoListAsync(decodeChannelCodeResponse.ChannelSeq, maximumVideos);
        }

        public async Task<UpcomingVideoListResponse> GetUpcomingVideoListAsync(string channelCode, int maximumVideos, CancellationToken cancellationToken)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode, cancellationToken);
            return await GetUpcomingVideoListAsync(decodeChannelCodeResponse.ChannelSeq, maximumVideos, cancellationToken);
        }

        public async Task<UpcomingVideoListResponse> GetUpcomingVideoListAsync(int channelSeq, int maximumVideos)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_upcomingVideoListEndpoint, _appId, channelSeq, maximumVideos));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["result"] == null))
                throw new UnkownErrorException();

            UpcomingVideoListResponse upcomingVideoListResponse = JsonConvert.DeserializeObject<UpcomingVideoListResponse>(responseTextDynamic["result"].ToString());
            if (upcomingVideoListResponse == null)
                throw new UnmappableResponseException();

            return upcomingVideoListResponse;
        }

        public async Task<UpcomingVideoListResponse> GetUpcomingVideoListAsync(int channelSeq, int maximumVideos, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_upcomingVideoListEndpoint, _appId, channelSeq, maximumVideos));
            HttpResponseMessage response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["result"] == null))
                throw new UnkownErrorException();

            UpcomingVideoListResponse upcomingVideoListResponse = JsonConvert.DeserializeObject<UpcomingVideoListResponse>(responseTextDynamic["result"].ToString());
            if (upcomingVideoListResponse == null)
                throw new UnmappableResponseException();

            return upcomingVideoListResponse;
        }
        #endregion

        private async Task HandleNonSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException(_appId);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                string errorData = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(errorData) || string.IsNullOrWhiteSpace(errorData))
                    throw new InternalServerErrorException();
                else
                    throw new InternalServerErrorException(errorData);
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
    }
}

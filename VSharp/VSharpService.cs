using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VSharp.Exceptions;
using VSharp.Iterators;
using VSharp.Models;
using VSharp.Models.Post;

namespace VSharp
{
    public class VSharpService
    {
        private readonly HttpClient _http;
        private readonly string _appId;
        private readonly string _userAgent;
        private const string _decodeChannelCodeEndpoint = "http://api.vfan.vlive.tv/vproxy/channelplus/decodeChannelCode?app_id={0}&channelCode={1}";
        private const string _channelInfoEndpoint = "https://api-vfan.vlive.tv/v2/channel.{0}?app_id={1}&fields=channel_seq,channel_code,type,channel_name,comment,fan_count,channel_cover_img,channel_profile_img,representative_color,background_color,celeb_boards,fan_boards,is_show_banner,vstore,is_show_upcoming,media_channel,banner,gfp_ad_enabled,banner_ad_enabled,ad_channel_id,ad_cp_id,fanclub,agency_seq,channel_announce";
        private const string _channelVideoListEndpoint = "https://api-vfan.vlive.tv/vproxy/channelplus/getChannelVideoList?app_id={0}&channelSeq={1}&maxNumOfRows={2}&pageNo={3}";
        private const string _upcomingVideoListEndpoint = "http://api.vfan.vlive.tv/vproxy/channelplus/getUpcomingVideoList?app_id={0}&channelSeq={1}&maxNumOfRows={2}&pageNo={3}";
        private const string _noticeListEndpoint = "http://notice.vlive.tv/notice/list.json?channel_seq={0}";
        private const string _postListEndpoint = "http://api.vfan.vlive.tv/v3/board.{0}/posts?app_id={1}&limit={2}";
        private const string _postListAfterEndpoint = "http://api.vfan.vlive.tv/v3/board.{0}/posts?app_id={1}&limit={2}&after={3}";
        private const string _postListBeforeEndpoint = "http://api.vfan.vlive.tv/v3/board.{0}/posts?app_id={1}&limit={2}&previous={3}";
        private const string _aboutEndpoint = "https://api-vfan.vlive.tv/vproxy/channel/{0}/about?app_id={1}";
        private const string _statusEndpoint = "https://www.vlive.tv/video/status?videoSeq={0}";

        public VSharpService(string appId)
        {
            _appId = appId;
            _userAgent = "VSharp VLive API Wrapper";

            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("User-Agent", _userAgent);
        }

        public VSharpService(string appId, string userAgent)
        {
            _appId = appId;
            _userAgent = userAgent;

            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("User-Agent", _userAgent);
        }

        #region DecodeChannelCode
        public async Task<DecodeChannelCodeResponse> DecodeChannelCodeAsync(string channelCode)
        {
            ValidateChannelCode(channelCode);

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
            ValidateChannelCode(channelCode);

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
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));

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
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));

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
        public async Task<ChannelVideoListResponse> GetChannelVideoListAsync(string channelCode, int count, int page)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode);
            return await GetChannelVideoListAsync(decodeChannelCodeResponse.ChannelSeq, count, page);
        }

        public async Task<ChannelVideoListResponse> GetChannelVideoListAsync(string channelCode, int count, int page, CancellationToken cancellationToken)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode, cancellationToken);
            return await GetChannelVideoListAsync(decodeChannelCodeResponse.ChannelSeq, count, page, cancellationToken);
        }

        public async Task<ChannelVideoListResponse> GetChannelVideoListAsync(int channelSeq, int count, int page)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));
            ValidateStrictlyPostiveInteger(count, nameof(count));
            ValidateStrictlyPostiveInteger(page, nameof(page));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_channelVideoListEndpoint, _appId, channelSeq, count, page));
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

        public async Task<ChannelVideoListResponse> GetChannelVideoListAsync(int channelSeq, int count, int page, CancellationToken cancellationToken)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));
            ValidateStrictlyPostiveInteger(count, nameof(count));
            ValidateStrictlyPostiveInteger(page, nameof(page));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_channelVideoListEndpoint, _appId, channelSeq, count, page));
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

        public ChannelVideoListIterator CreateChannelVideoListIterator(int channelSeq, int count)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));
            ValidateStrictlyPostiveInteger(count, nameof(count));
            return new ChannelVideoListIterator(this, channelSeq, count);
        }
        #endregion

        #region GetUpcomingVideoList
        public async Task<UpcomingVideoListResponse> GetUpcomingVideoListAsync(string channelCode, int count, int page)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode);
            return await GetUpcomingVideoListAsync(decodeChannelCodeResponse.ChannelSeq, count, page);
        }

        public async Task<UpcomingVideoListResponse> GetUpcomingVideoListAsync(string channelCode, int count, int page, CancellationToken cancellationToken)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode, cancellationToken);
            return await GetUpcomingVideoListAsync(decodeChannelCodeResponse.ChannelSeq, count, page, cancellationToken);
        }

        public async Task<UpcomingVideoListResponse> GetUpcomingVideoListAsync(int channelSeq, int count, int page)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));
            ValidateStrictlyPostiveInteger(count, nameof(count));
            ValidateStrictlyPostiveInteger(page, nameof(page));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_upcomingVideoListEndpoint, _appId, channelSeq, count, page));
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

        public async Task<UpcomingVideoListResponse> GetUpcomingVideoListAsync(int channelSeq, int count, int page, CancellationToken cancellationToken)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));
            ValidateStrictlyPostiveInteger(count, nameof(count));
            ValidateStrictlyPostiveInteger(page, nameof(page));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_upcomingVideoListEndpoint, _appId, channelSeq, count, page));
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

        public UpcomingVideoListIterator CreateUpcomingVideoListIterator(int channelSeq, int count)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));
            ValidateStrictlyPostiveInteger(count, nameof(count));
            return new UpcomingVideoListIterator(this, channelSeq, count);
        }
        #endregion

        #region GetNoticeList
        public async Task<List<Notice>> GetNoticesAsync(string channelCode)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode);
            return await GetNoticesAsync(decodeChannelCodeResponse.ChannelSeq);
        }

        public async Task<List<Notice>> GetNoticesAsync(string channelCode, CancellationToken cancellationToken)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode, cancellationToken);
            return await GetNoticesAsync(decodeChannelCodeResponse.ChannelSeq, cancellationToken);
        }

        public async Task<List<Notice>> GetNoticesAsync(int channelSeq)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_noticeListEndpoint, channelSeq));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["data"] == null))
                throw new UnkownErrorException();

            List<Notice> notices = JsonConvert.DeserializeObject<List<Notice>>(responseTextDynamic["data"].ToString());
            if (notices == null)
                throw new UnmappableResponseException();

            return notices;
        }

        public async Task<List<Notice>> GetNoticesAsync(int channelSeq, CancellationToken cancellationToken)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_noticeListEndpoint, channelSeq));
            HttpResponseMessage response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["data"] == null))
                throw new UnkownErrorException();

            List<Notice> notices = JsonConvert.DeserializeObject<List<Notice>>(responseTextDynamic["data"].ToString());
            if (notices == null)
                throw new UnmappableResponseException();

            return notices;
        }
        #endregion

        #region GetPostList
        public async Task<PostListResponse> GetPostListAsync(int board, int count)
        {
            ValidateStrictlyPostiveInteger(board, nameof(board));
            ValidateStrictlyPostiveInteger(count, nameof(count));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_postListEndpoint, board, _appId, count));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["data"] == null))
                throw new UnkownErrorException();

            PostListResponse postListResponse = JsonConvert.DeserializeObject<PostListResponse>(responseText);
            if (postListResponse == null)
                throw new UnmappableResponseException();

            return postListResponse;
        }

        public async Task<PostListResponse> GetPostListAsync(int board, int count, CancellationToken cancellationToken)
        {
            ValidateStrictlyPostiveInteger(board, nameof(board));
            ValidateStrictlyPostiveInteger(count, nameof(count));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_postListEndpoint, board, _appId, count));
            HttpResponseMessage response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["data"] == null))
                throw new UnkownErrorException();

            PostListResponse postListResponse = JsonConvert.DeserializeObject<PostListResponse>(responseText);
            if (postListResponse == null)
                throw new UnmappableResponseException();

            return postListResponse;
        }

        public async Task<PostListResponse> GetPostListAfterAsync(int board, int count, string after)
        {
            ValidateStrictlyPostiveInteger(board, nameof(board));
            ValidateStrictlyPostiveInteger(count, nameof(count));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_postListAfterEndpoint, board, _appId, count, after));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["data"] == null))
                throw new UnkownErrorException();

            PostListResponse postListResponse = JsonConvert.DeserializeObject<PostListResponse>(responseText);
            if (postListResponse == null)
                throw new UnmappableResponseException();

            return postListResponse;
        }

        public async Task<PostListResponse> GetPostListAfterAsync(int board, int count, string after, CancellationToken cancellationToken)
        {
            ValidateStrictlyPostiveInteger(board, nameof(board));
            ValidateStrictlyPostiveInteger(count, nameof(count));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_postListAfterEndpoint, board, _appId, count, after));
            HttpResponseMessage response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic == null
            || (responseTextDynamic != null && responseTextDynamic["data"] == null))
                throw new UnkownErrorException();

            PostListResponse postListResponse = JsonConvert.DeserializeObject<PostListResponse>(responseText);
            if (postListResponse == null)
                throw new UnmappableResponseException();

            return postListResponse;
        }

        public PostListIterator CreatePostListIterator(int board, int count) 
        {
            ValidateStrictlyPostiveInteger(board, nameof(board));
            ValidateStrictlyPostiveInteger(count, nameof(count));
            return new PostListIterator(this, board, count); 
        }
        #endregion

        #region GetAboutInfo
        public async Task<About> GetAboutInfoAsync(string channelCode)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode);
            return await GetAboutInfoAsync(decodeChannelCodeResponse.ChannelSeq);
        }

        public async Task<About> GetAboutInfoAsync(string channelCode, CancellationToken cancellationToken)
        {
            DecodeChannelCodeResponse decodeChannelCodeResponse = await DecodeChannelCodeAsync(channelCode, cancellationToken);
            return await GetAboutInfoAsync(decodeChannelCodeResponse.ChannelSeq, cancellationToken);
        }

        public async Task<About> GetAboutInfoAsync(int channelSeq)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_aboutEndpoint, channelSeq, _appId));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic != null && responseTextDynamic["code"] != null && (int)responseTextDynamic["code"] == 9101)
                throw new NoSuchChannelException(channelSeq);
            else if (responseTextDynamic != null && responseTextDynamic["code"] != null && (int)responseTextDynamic["code"] == 9999)
                throw new InternalServerErrorException(responseText);
            else if (responseTextDynamic == null
                 || (responseTextDynamic != null && responseTextDynamic["result"] == null))
                throw new UnkownErrorException();

            About about = JsonConvert.DeserializeObject<About>(responseTextDynamic["result"].ToString());
            if (about == null)
                throw new UnmappableResponseException();

            return about;
        }

        public async Task<About> GetAboutInfoAsync(int channelSeq, CancellationToken cancellationToken)
        {
            ValidateStrictlyPostiveInteger(channelSeq, nameof(channelSeq));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_aboutEndpoint, channelSeq, _appId));
            HttpResponseMessage response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            dynamic responseTextDynamic = JsonConvert.DeserializeObject<dynamic>(responseText);
            if (responseTextDynamic != null && responseTextDynamic["code"] != null && (int)responseTextDynamic["code"] == 9101)
                throw new NoSuchChannelException(channelSeq);
            else if (responseTextDynamic != null && responseTextDynamic["code"] != null && (int)responseTextDynamic["code"] == 9999)
                throw new InternalServerErrorException(responseText);
            else if (responseTextDynamic == null
                 || (responseTextDynamic != null && responseTextDynamic["result"] == null))
                throw new UnkownErrorException();

            About about = JsonConvert.DeserializeObject<About>(responseTextDynamic["result"].ToString());
            if (about == null)
                throw new UnmappableResponseException();

            return about;
        }
        #endregion

        #region GetStatus
        public async Task<Status> GetVideoStatusAsync(int videoSeq)
        {
            ValidateStrictlyPostiveInteger(videoSeq, nameof(videoSeq));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_statusEndpoint, videoSeq));
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

            Status status = JsonConvert.DeserializeObject<Status>(responseTextDynamic["result"].ToString(), new JsonSerializerSettings
            {
                Culture = new System.Globalization.CultureInfo("en-US")
            });
            if (status == null)
                throw new UnmappableResponseException();

            return status;
        }
        #endregion

        private async Task HandleNonSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
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

        private void ValidateStrictlyPostiveInteger(int value, string argumentName)
        {
            if (value <= 0)
                throw new ArgumentException($"{argumentName} must be a strictly positive integer.");
        }

        private void ValidateChannelCode(string channelCode)
        {
            if (!Regex.IsMatch(channelCode, "^[a-zA-Z0-9]+$"))
                throw new ArgumentException("channelCode must be a combination of alphanumeric characters.");
        }
    }
}

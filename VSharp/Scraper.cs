using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VSharp.Exceptions;

namespace VSharp
{
    internal class Scraper
    {
        private readonly string _appId;
        private readonly HttpClient _http;
        private readonly Regex regex = new Regex("vlive\\.video\\.init.+\\n.{4}(?<id>.+)\\\",\\n.{4}(?<key>.+)\\\"");
        private const string _vliveUrl = "https://www.vlive.tv/video/{0}";

        public Scraper(HttpClient http, string appId)
        {
            _http = http;
            _appId = appId;
        }

        public async Task<(string Key, string VideoId)> ScrapeCredentialsAsync(int videoSeq)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_vliveUrl, videoSeq));
            HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            MatchCollection mc = regex.Matches(responseText);
            foreach (Match m in mc)
            {
                if (m.Groups["id"] != null && m.Groups["key"] != null)
                {
                    return (m.Groups["key"].Value, m.Groups["id"].Value);
                }
            }

            throw new ResourceUnavailableException();
        }

        public async Task<(string Key, string VideoId)> ScrapeCredentialsAsync(int videoSeq, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_vliveUrl, videoSeq));
            HttpResponseMessage response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                await HandleNonSuccessStatusCodeAsync(response);

            string responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseText) || string.IsNullOrWhiteSpace(responseText))
                throw new UnkownErrorException();

            MatchCollection mc = regex.Matches(responseText);
            foreach (Match m in mc)
            {
                if (m.Groups["id"] != null && m.Groups["key"] != null)
                {
                    return (m.Groups["key"].Value, m.Groups["id"].Value);
                }
            }

            throw new UnauthorizedException(_appId);
        }

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
    }
}

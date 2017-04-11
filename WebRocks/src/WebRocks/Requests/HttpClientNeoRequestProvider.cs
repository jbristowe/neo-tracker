using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRocks.Requests
{
    public class HttpClientNeoRequestProvider : INeoRequestProvider
    {
        public const string RateLimitHeader = "X-RateLimit-Limit";
        public const string RateLimitRemainingHeader = "X-RateLimit-Remaining";

        static HttpClient client = new HttpClient();

        public HttpClientNeoRequestProvider()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public NeoResponse SendGetRequest(Uri uri)
        {
            return SendGetRequestAsync(uri).Result;
        }

        public async Task<NeoResponse> SendGetRequestAsync(Uri uri)
        {
            var response = new NeoResponse();

            var responseMessage = await client.GetAsync(uri);
            responseMessage.EnsureSuccessStatusCode();
            response.RateLimitTotal = int.Parse(responseMessage.Headers.GetValues(RateLimitHeader).First());
            response.RateLimitRemaining = int.Parse(responseMessage.Headers.GetValues(RateLimitRemainingHeader).First());
            response.ResponseText = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }
    }
}

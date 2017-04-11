using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebRocks.Requests
{
    public class HttpNeoRequestProvider : INeoRequestProvider
    {
        public const string RateLimitHeader = "X-RateLimit-Limit";
        public const string RateLimitRemainingHeader = "X-RateLimit-Remaining";

        public HttpNeoRequestProvider() { }

        public NeoResponse SendGetRequest(Uri uri)
        {
            //var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            //var response = new NeoResponse();

            //using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            //{
            //    response.ResponseCode = (int)webResponse.StatusCode;

            //    if (IsSuccess(response))
            //    {
            //        SetHeaders(response, webResponse);

            //        using (var responseStream = webResponse.GetResponseStream())
            //        {
            //            using (var reader = new StreamReader(responseStream))
            //            {
            //                response.ResponseText = reader.ReadToEnd();
            //            }
            //        }
            //    }
            //}

            //return response;

            var task = SendGetRequestAsync(uri);
            task.Wait();
            return task.Result;
        }

        public async Task<NeoResponse> SendGetRequestAsync(Uri uri)
        {
            var webRequest = (HttpWebRequest)WebRequest.CreateHttp(uri);
            var response = new NeoResponse();

            using (var webResponse = (HttpWebResponse)(await webRequest.GetResponseAsync()))
            {
                response.ResponseCode = (int)webResponse.StatusCode;

                if(IsSuccess(response))
                {
                    SetHeaders(response, webResponse);

                    using (var responseStream = webResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            response.ResponseText = await reader.ReadToEndAsync();
                        }
                    }
                }
            }

            return response;
        }

        private static bool IsSuccess(NeoResponse response)
        {
            return response.ResponseCode >= 200 || response.ResponseCode < 300;
        }

        private static void SetHeaders(NeoResponse response, HttpWebResponse webResponse)
        {
            response.RateLimitTotal = Int32.Parse(webResponse.Headers[RateLimitHeader]);
            response.RateLimitRemaining = Int32.Parse(webResponse.Headers[RateLimitRemainingHeader]);
        }
    }
}

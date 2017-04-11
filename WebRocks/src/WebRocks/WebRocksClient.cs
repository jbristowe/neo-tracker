using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WebRocks.Data;
using WebRocks.Internal;
using WebRocks.Requests;

namespace WebRocks
{
    public class WebRocksClient : IWebRocksClient
    {
        public WebRocksClient(WebRocksConfiguration config, INeoRequestProvider requestProvider)
        {
            Config = config;
            RequestProvider = requestProvider;
        }

        public WebRocksConfiguration Config { get; private set; }

        public int? RateLimit { get; private set; }

        public int? RateLimitRemaining { get; private set; }

        public INeoRequestProvider RequestProvider { get; set; }

        public ApiBrowsePage GetBrowsePage(int pageNumber, int pageSize)
        {
            Uri requestUrl = Config.ApiUrl.ConstructNeowsBrowsePageUri(pageNumber, pageSize, Config.ApiKey);
            var serializer = Serializers.ApiBrowsePage;

            var response = RequestProvider.SendGetRequest(requestUrl);
            SetRateLimits(response);
            return GetObjectFromResponse<ApiBrowsePage>(response, serializer);
        }

        public async Task<ApiBrowsePage> GetBrowsePageAsync(int pageNumber, int pageSize)
        {
            Uri requestUrl = Config.ApiUrl.ConstructNeowsBrowsePageUri(pageNumber, pageSize, Config.ApiKey);
            var serializer = Serializers.ApiBrowsePage;

            var response = await RequestProvider.SendGetRequestAsync(requestUrl);
            SetRateLimits(response);
            return GetObjectFromResponse<ApiBrowsePage>(response, serializer);
        }

        public ApiFeedPage GetFeedPage(DateTime startDate)
        {
            Uri requestUrl = Config.ApiUrl.ConstructNeowsGetFeedUri(startDate, startDate.AddDays(7), Config.ApiKey);
            var serializer = Serializers.ApiFeedPage;

            var response = RequestProvider.SendGetRequest(requestUrl);
            SetRateLimits(response);
            return GetObjectFromResponse<ApiFeedPage>(response, serializer);
        }

        public NearEarthObject GetObjectById(int id)
        {
            Uri requestUrl = Config.ApiUrl.ConstructNeowsGetByIdUri(id, Config.ApiKey);
            var serializer = Serializers.NearEarthObject;

            var response = RequestProvider.SendGetRequest(requestUrl);
            SetRateLimits(response);
            return GetObjectFromResponse<NearEarthObject>(response, serializer);
        }

        public NeoStats GetCurrentStats()
        {
            Uri requestUrl = Config.ApiUrl.ConstructNeowsStatsUri(Config.ApiKey);
            var serializer = Serializers.NeoStats;

            var response = RequestProvider.SendGetRequest(requestUrl);
            SetRateLimits(response);
            return GetObjectFromResponse<NeoStats>(response, serializer);
        }

        private static T GetObjectFromResponse<T>(NeoResponse response, DataContractJsonSerializer serializer)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response.ResponseText)))
            {
                var obj = (T)serializer.ReadObject(ms);
                return obj;
            }
        }

        private void SetRateLimits(NeoResponse response)
        {
            RateLimit = response.RateLimitTotal;
            RateLimitRemaining = response.RateLimitRemaining;
        }
    }
}

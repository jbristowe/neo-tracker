using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRocks.Requests;

namespace WebRocks
{
    public class WebRocksConfiguration
    {
        public const string DefaultUrl = "https://api.nasa.gov/neo/rest/v1/";
        public const string DefaultKey = "DEMO_KEY";

        public WebRocksConfiguration()
            : this(DefaultUrl, DefaultKey)
        {
        }

        public WebRocksConfiguration(string apiKey)
            : this(DefaultUrl, apiKey)
        {
        }

        public WebRocksConfiguration(string apiUrl, string apiKey)
        {
            ApiUrl = new Uri(apiUrl);
            ApiKey = apiKey;
        }

        public string ApiKey { get; }

        public Uri ApiUrl { get; }
    }
}

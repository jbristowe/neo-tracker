using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebRocks;
using WebRocks.Requests;
using Xunit;

namespace WebRocksTests
{
    public class ClientTests
    {
        public class ResourceRequestProvider : INeoRequestProvider
        {
            public NeoResponse SendGetRequest(Uri uri)
            {
                string lastSegment = uri.Segments.Last().ToLowerInvariant();
                var assembly = Assembly.GetExecutingAssembly();
                Stream resourceStream = null;

                switch (lastSegment)
                {
                    case "browse":
                        resourceStream = assembly.GetManifestResourceStream("WebRocksTests.Resources.BrowseResponse.json");
                        break;
                    case "feed":
                        resourceStream = assembly.GetManifestResourceStream("WebRocksTests.Resources.FeedResponse.json");
                        break;
                    default:
                        resourceStream = assembly.GetManifestResourceStream("WebRocksTests.Resources.GetByIdResponse.json");
                        break;
                }

                using (var reader = new StreamReader(resourceStream))
                {
                    var response = new NeoResponse()
                    {
                        RateLimitRemaining = 25,
                        RateLimitTotal = 30,
                        ResponseCode = 200,
                        ResponseText = reader.ReadToEnd(),
                    };
                    return response;
                }
            }

            public Task<NeoResponse> SendGetRequestAsync(Uri uri)
            {
                throw new NotImplementedException();
            }
        }


        [Fact]
        public void WebRocksClient_FormatsGetBrowsePage_PageSize()
        {
            var client = new WebRocksClient(new WebRocksConfiguration("DEMO_KEY"), new ResourceRequestProvider());

            var page = client.GetBrowsePage(0, 20);

            Assert.Equal(0, page.Page.Number);
            Assert.Equal(20, page.Page.Size);
        }

        [Fact]
        public void WebRocksClient_FormatsGetObjectById_ObjectParameters()
        {
            var client = new WebRocksClient(new WebRocksConfiguration("DEMO_KEY"), new ResourceRequestProvider());
            client.RequestProvider = new ResourceRequestProvider();

            var neo = client.GetObjectById(3542519);

            Assert.Equal("2315020", neo.NEOReferenceId);
        }

        [Fact]
        public void WebRocksClient_FormatsGetFeedPage_FeedData()
        {
            var client = new WebRocksClient(new WebRocksConfiguration("DEMO_KEY"), new ResourceRequestProvider());
            client.RequestProvider = new ResourceRequestProvider();

            var page = client.GetFeedPage(DateTime.Now.AddYears(-1));

            Assert.Equal(17, page.ElementCount);
            Assert.Equal("https://api.nasa.gov/neo/rest/v1/feed?start_date=2015-09-08&end_date=2015-09-15&api_key=DEMO_KEY", page.Links.Next);
            Assert.Equal("https://api.nasa.gov/neo/rest/v1/feed?start_date=2015-08-31&end_date=2015-09-07&api_key=DEMO_KEY", page.Links.Prev);
            Assert.Equal("https://api.nasa.gov/neo/rest/v1/feed?start_date=2015-09-07&end_date=2015-09-08&api_key=DEMO_KEY", page.Links.Self);
        }
    }
}

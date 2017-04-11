using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRocks.Internal
{
    public static class UriExtension
    {
        public static Uri ConstructNeowsBrowsePageUri(this Uri baseUri, int pageNumber, int pageSize, string apiKey)
        {
            return new Uri(baseUri, String.Format("neo/browse?page={0}&size={1}&api_key={2}", pageNumber, pageSize, apiKey));
        }

        public static Uri ConstructNeowsGetByIdUri(this Uri baseUri, int objectId, string apiKey)
        {
            return new Uri(baseUri, String.Format("neo/{0}?api_key={1}", objectId, apiKey));
        }

        public static Uri ConstructNeowsGetFeedUri(this Uri baseUri, DateTime startDate, DateTime endDate, string apiKey)
        {
            return new Uri(baseUri, String.Format("feed?start_date={0:yyyy-MM-dd}&end_date={1:yyyy-MM-dd}&api_key={2}", startDate, endDate, apiKey));
        }

        public static Uri ConstructNeowsStatsUri(this Uri baseUri, string apiKey)
        {
            return new Uri(baseUri, String.Format("stats?api_key={0}", apiKey));
        }
    }
}

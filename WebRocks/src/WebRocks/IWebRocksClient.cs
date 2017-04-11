using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRocks.Data;

namespace WebRocks
{
    public interface IWebRocksClient
    {
        int? RateLimit { get; }
        int? RateLimitRemaining { get; }

        ApiFeedPage GetFeedPage(DateTime startDate);
        ApiBrowsePage GetBrowsePage(int pageNumber, int pageSize);
        NearEarthObject GetObjectById(int id);
        NeoStats GetCurrentStats();
    }
}

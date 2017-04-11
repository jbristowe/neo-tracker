using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRocks.Requests
{
    public interface INeoRequestProvider
    {
        NeoResponse SendGetRequest(Uri uri);

        Task<NeoResponse> SendGetRequestAsync(Uri uri);
    }
}

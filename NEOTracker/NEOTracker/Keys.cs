using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEOTracker
{
    public class Keys
    {
        // https://api.nasa.gov/index.htm
        public static string NASAAPIKEY { get; } = "";

        // http://uwpcommunitytoolkit.readthedocs.io/en/master/services/Facebook/
        public static string FBAppId { get; } = "";

        // http://uwpcommunitytoolkit.readthedocs.io/en/master/services/Twitter/
        public static string TwitterConsumerKey { get; } = "";
        public static string TwitterSecretKey { get; } = "";
        public static string TwitterCallbackUri { get; } = "";
    }
}

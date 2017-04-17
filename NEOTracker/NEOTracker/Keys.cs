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
        public static string NASAAPIKEY { get; } = "vgcZgGZoDdIUZh64k1NlX0VMwjMT8WjWTgP1d3Yg";

        // http://uwpcommunitytoolkit.readthedocs.io/en/master/services/Facebook/
        public static string FBAppId { get; } = "796538790499530";

        // http://uwpcommunitytoolkit.readthedocs.io/en/master/services/Twitter/
        public static string TwitterConsumerKey { get; } = "Q92BjqOPMuMKYS9tUiEvhMeYE";
        public static string TwitterSecretKey { get; } = "7vgE2HqbkuKXcJnvRrtOMfS0xQdKLMqCLfbBegzOs4sVevyGOa";
        public static string TwitterCallbackUri { get; } = "http://neotracker.metulev.com";
    }
}

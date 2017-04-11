using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WebRocks.Data;

namespace WebRocks.Internal
{
    internal static class Serializers
    {
        internal static DataContractJsonSerializer ApiBrowsePage { get; } = new DataContractJsonSerializer(typeof(ApiBrowsePage));

        internal static DataContractJsonSerializer ApiFeedPage { get; } = new DataContractJsonSerializer(typeof(ApiFeedPage), new DataContractJsonSerializerSettings()
        {
            UseSimpleDictionaryFormat = true,
        });

        internal static DataContractJsonSerializer NearEarthObject { get; } = new DataContractJsonSerializer(typeof(NearEarthObject));

        internal static DataContractJsonSerializer NeoStats { get; } = new DataContractJsonSerializer(typeof(NeoStats), new DataContractJsonSerializerSettings()
        {
            DateTimeFormat = new DateTimeFormat("yyyy-MM-dd"),
        });
    }
}

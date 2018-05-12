using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cryptopia.Public.Models {
    public class RequestDataMarketHistory : RequestData {
        [JsonProperty("Data")]
        public List<MarketHistory> MarketHistory { get; set; }
    }
}

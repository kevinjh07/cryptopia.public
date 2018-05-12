using Newtonsoft.Json;

namespace Cryptopia.Public.Models {
    public class RequestDataMarket : RequestData {
        [JsonProperty("Data")]
        public Market Data { get; set; }
    }
}

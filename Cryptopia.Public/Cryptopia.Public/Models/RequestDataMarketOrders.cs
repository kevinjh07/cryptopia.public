using Newtonsoft.Json;

namespace Cryptopia.Public.Models
{
    public class RequestDataMarketOrders
    {
        [JsonProperty("Data")]
        public MarketOrders MarketOrdersData { get; set; }
    }
}

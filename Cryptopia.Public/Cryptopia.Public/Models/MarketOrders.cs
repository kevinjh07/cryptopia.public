using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cryptopia.Public.Models {
    public class MarketOrders {
        [JsonProperty("Buy")]
        public List<OrdersData> BuyOrders { get; set; }

        [JsonProperty("Sell")]
        public List<OrdersData> SellOrders { get; set; }
    }
}

using Newtonsoft.Json;

namespace Cryptopia.Public.Models {
    public class OrdersData {
        [JsonProperty("TradePairId")]
        public int TradePairId { get; set; }

        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Price")]
        public double Price { get; set; }

        [JsonProperty("Volume")]
        public double Volume { get; set; }

        [JsonProperty("Total")]
        public double Total { get; set; }

        public string VolumeFormatted {
            get {
                return string.Format("{0} {1}", Volume, Label.Substring(0, Label.IndexOf("/")));
            }
        }
    }
}

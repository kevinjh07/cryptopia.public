using Newtonsoft.Json;

namespace Cryptopia.Public.Models {
    public class Market {
        [JsonProperty("TradePairId")]
        public int TradePairId { get; set; }

        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("AskPrice")]
        public double AskPrice { get; set; }

        [JsonProperty("BidPrice")]
        public double BidPrice { get; set; }

        [JsonProperty("Low")]
        public double Low { get; set; }

        [JsonProperty("High")]
        public double High { get; set; }

        [JsonProperty("Volume")]
        public double Volume { get; set; }

        [JsonProperty("LastPrice")]
        public double LastPrice { get; set; }

        [JsonProperty("BuyVolume")]
        public double BuyVolume { get; set; }

        [JsonProperty("SellVolume")]
        public double SellVolume { get; set; }

        [JsonProperty("Change")]
        public double Change { get; set; }

        [JsonProperty("Open")]
        public double Open { get; set; }

        [JsonProperty("Close")]
        public double Close { get; set; }

        [JsonProperty("BaseVolume")]
        public double BaseVolume { get; set; }

        [JsonProperty("BuyBaseVolume")]
        public double BuyBaseVolume { get; set; }

        [JsonProperty("SellBaseVolume")]
        public double SellBaseVolume { get; set; }

        private string CoinSymbol {
            get {
                return Label.Substring(0, Label.IndexOf("/"));
            }
        }

        public string VolumeCoinSymbol {
            get {
                return string.Format("{0} {1}", Volume, CoinSymbol);
            }
        }

        public string SellVolumeCoinSymbol {
            get {
                return string.Format("{0} {1}", SellVolume, CoinSymbol);
            }
        }
    }
}

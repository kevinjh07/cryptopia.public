using Newtonsoft.Json;
using System;

namespace Cryptopia.Public.Models {
    public class MarketHistory {
        [JsonProperty("TradePairId")]
        public int TradePairId { get; set; }

        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Type")]
        public HistoryType Type { get; set; }

        [JsonProperty("Price")]
        public double Price { get; set; }

        [JsonProperty("Amount")]
        public double Amount { get; set; }

        [JsonProperty("Total")]
        public double Total { get; set; }

        [JsonProperty("Timestamp")]
        public int Timestamp { get; set; }

        public string AmountFormatted {
            get {
                return string.Format("{0} {1}", Amount, Label.Substring(0, Label.IndexOf("/")));
            }
        }

        public DateTime TimestampDateTime {
            get {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Timestamp).ToLocalTime();
            }
        }
    }
}

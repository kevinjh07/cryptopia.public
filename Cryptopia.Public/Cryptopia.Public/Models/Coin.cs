using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cryptopia.Public.Models {
    public class Coin : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null) {
            if (!string.IsNullOrWhiteSpace(propertyName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static readonly string CryptopiaCoinImagesURL = "https://www.cryptopia.co.nz/Content/Images/Coins/{0}-medium.png";

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("Algorithm")]
        public string Algorithm { get; set; }

        [JsonProperty("WithdrawFee")]
        public double WithdrawFee { get; set; }

        [JsonProperty("MinWithdraw")]
        public double MinWithdraw { get; set; }

        [JsonProperty("MaxWithdraw")]
        public double MaxWithdraw { get; set; }

        [JsonProperty("MinBaseTrade")]
        public double MinBaseTrade { get; set; }

        [JsonProperty("IsTipEnabled")]
        public bool IsTipEnabled { get; set; }

        [JsonProperty("MinTip")]
        public string MinTip { get; set; }

        [JsonProperty("DepositConfirmations")]
        public string DepositConfirmations { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("StatusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty("ListingStatus")]
        public string ListingStatus { get; set; }

        public string Image {
            get {
                return string.Format(CryptopiaCoinImagesURL, Symbol);
            }
        }
    }
}

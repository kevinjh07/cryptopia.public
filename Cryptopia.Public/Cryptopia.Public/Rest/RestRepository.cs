using Cryptopia.Public.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptopia.Public.Rest {
    public class RestRepository : IRestRepository {
        private static readonly string GetCurrenciesURL = "https://www.cryptopia.co.nz/api/GetCurrencies";
        private static readonly string GetMarketOrdersURL = "https://www.cryptopia.co.nz/api/GetMarketOrders/{0}_BTC";
        private static readonly string GetMarketHistoryURL = "https://www.cryptopia.co.nz/api/GetMarketHistory/{0}_BTC";
        private static readonly string GetMarketURL = "https://www.cryptopia.co.nz/api/GetMarket/{0}_BTC";

        public async Task<List<Coin>> GetCoins() {
            List<Coin> coins = null;
            using (var client = new HttpClient()) {
                var json = await client.GetStringAsync(GetCurrenciesURL);
                var requestData = JsonConvert.DeserializeObject<RequestDataCurrencies>(json);
                coins = requestData.Data;
            }
            return coins;
        }

        public async Task<MarketOrders> GetMarketOrdersData(string coinSymbol) {
            MarketOrders marketData = null;
            using (var client = new HttpClient()) {
                var json = await client.GetStringAsync(string.Format(GetMarketOrdersURL, coinSymbol));
                var requestData = JsonConvert.DeserializeObject<RequestDataMarketOrders>(json);
                marketData = requestData.MarketOrdersData;
            }
            return marketData;
        }

        public async Task<List<MarketHistory>> GetMarketHistory(string coinSymbol) {
            List<MarketHistory> marketHistory = null;
            using (var client = new HttpClient()) {
                var json = await client.GetStringAsync(string.Format(GetMarketHistoryURL, coinSymbol));
                var requestData = JsonConvert.DeserializeObject<RequestDataMarketHistory>(json);
                marketHistory = requestData.MarketHistory;
            }
            return marketHistory;
        }

        public async Task<Market> GetMarket(string coinSymbol) {
            Market market = null;
            using (var client = new HttpClient()) {
                var json = await client.GetStringAsync(string.Format(GetMarketURL, coinSymbol));
                var requestData = JsonConvert.DeserializeObject<RequestDataMarket>(json);
                market = requestData.Data;
            }
            return market;
        }

        private async static Task<T> GetData<T>(string url) {
            string json = null;
            using (var client = new HttpClient()) {
                json = await client.GetStringAsync(GetMarketOrdersURL);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

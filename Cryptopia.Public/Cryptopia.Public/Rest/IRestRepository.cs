using Cryptopia.Public.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptopia.Public.Rest
{
    public interface IRestRepository
    {
        Task<List<Coin>> GetCoins();
        Task<MarketOrders> GetMarketOrdersData(string coinSymbol);
        Task<List<MarketHistory>> GetMarketHistory(string coinSymbol);
        Task<Market> GetMarket(string coinSymbol);
    }
}

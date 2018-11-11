using Cryptopia.Public.Models;
using Cryptopia.Public.Rest;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Extended;
using System.Linq;

namespace Cryptopia.Public.ViewModels {
    public class MarketHistoryPageViewModel : ViewModelBase {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IRestRepository _restRepository;
        public DelegateCommand RefreshCommand { get; private set; }

        private InfiniteScrollCollection<MarketHistory> marketHistories;
        public InfiniteScrollCollection<MarketHistory> MarketHistories {
            get { return marketHistories; }
            set { SetProperty(ref marketHistories, value); }
        }

        private List<MarketHistory> SourceList { get; set; }

        public string CoinSymbol { get; set; }

        private const int PageSize = 10;

        public MarketHistoryPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,
            IRestRepository restRepository) : base(navigationService) {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _restRepository = restRepository;
            SourceList = new List<MarketHistory>();
            MarketHistories = new InfiniteScrollCollection<MarketHistory> {
                OnLoadMore = async () => {
                    var page = MarketHistories.Count / PageSize;
                    return await Task.Run(() => LoadMarketHistory(page));
                },
                OnCanLoadMore = () => MarketHistories.Count < SourceList.Count
            };
            RefreshCommand = new DelegateCommand(async () => await GetMarketHistory());
        }

        public async override void OnNavigatingTo(NavigationParameters parameters) {
            var coin = (Coin)parameters["SelectedCoin"];
            CoinSymbol = coin.Symbol;
            await GetMarketHistory();
        }

        private async Task GetMarketHistory() {
            try {
                IsBusy = true;
                var marketHistories = await _restRepository.GetMarketHistory(CoinSymbol);
                SourceList.Clear();
                MarketHistories.Clear();
                SourceList.AddRange(marketHistories);
                MarketHistories.AddRange(LoadMarketHistory(0));
            } catch (Exception e) {
                await _pageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally {
                IsBusy = false;
            }
        }

        private List<MarketHistory> LoadMarketHistory(int pageIndex) {
            return SourceList.Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
    }
}

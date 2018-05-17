using Cryptopia.Public.Models;
using Cryptopia.Public.Rest;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Cryptopia.Public.ViewModels {
    public class MarketHistoryPageViewModel : ViewModelBase {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IRestRepository _restRepository;
        public DelegateCommand RefreshCommand { get; private set; }

        private ObservableCollection<MarketHistory> marketHistories;
        public ObservableCollection<MarketHistory> MarketHistories {
            get { return marketHistories; }
            set { SetProperty(ref marketHistories, value); }
        }

        public string CoinSymbol { get; set; }

        public MarketHistoryPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,
            IRestRepository restRepository) : base(navigationService) {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _restRepository = restRepository;
            MarketHistories = new ObservableCollection<MarketHistory>();
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
                MarketHistories.Clear();
                var marketHistories = await _restRepository.GetMarketHistory(CoinSymbol);
                foreach (var history in marketHistories) {
                    MarketHistories.Add(history);
                }
            } catch (Exception e) {
                await _pageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally {
                IsBusy = false;
            }
        }
    }
}

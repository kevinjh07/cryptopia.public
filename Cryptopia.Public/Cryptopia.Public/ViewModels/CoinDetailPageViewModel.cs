using System;
using Cryptopia.Public.Models;
using Cryptopia.Public.Rest;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace Cryptopia.Public.ViewModels {
    public class CoinDetailPageViewModel : ViewModelBase {
        private readonly INavigationService _navigationService;
        private readonly IRestRepository _restRepository;
        private readonly IPageDialogService _pageDialogService;
        public DelegateCommand MarketOrdersCommand { get; private set; }
        public DelegateCommand MarketHistoryCommand { get; private set; }

        private Coin coin;
        public Coin Coin {
            get { return coin; }
            set { SetProperty(ref coin, value); }
        }

        private Market market;
        public Market Market {
            get { return market; }
            set { SetProperty(ref market, value); }
        }

        public CoinDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, 
            IRestRepository restRepository) : base(navigationService) {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _restRepository = restRepository;
            MarketOrdersCommand = new DelegateCommand(ShowMarketOrdersPage);
            MarketHistoryCommand = new DelegateCommand(ShowMarketHistoryPage);
        }

        private async void ShowMarketOrdersPage() {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedCoin", Coin);
            await _navigationService.NavigateAsync("MarketOrdersPage", parameters);
        }

        private async void ShowMarketHistoryPage() {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedCoin", Coin);
            await _navigationService.NavigateAsync("MarketHistoryPage", parameters);
        }

        public override void OnNavigatingTo(NavigationParameters parameters) {
            Coin = (Coin)parameters["SelectedCoin"];
            GetMarket();
        }

        private async void GetMarket() {
            try {
                IsBusy = true;
                Market = await _restRepository.GetMarket(Coin.Symbol);
            } catch (Exception e) {
                await _pageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally {
                IsBusy = false;
            }
        }
    }
}

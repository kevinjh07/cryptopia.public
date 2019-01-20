using System;
using System.Threading.Tasks;
using Cryptopia.Public.Models;
using Cryptopia.Public.Rest;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace Cryptopia.Public.ViewModels
{
    public class CoinDetailPageViewModel : ViewModelBase
    {
        private readonly IRestRepository RestRepository;
        private readonly IPageDialogService PageDialogService;
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
            IRestRepository restRepository) : base(navigationService)
        {
            PageDialogService = pageDialogService;
            RestRepository = restRepository;
            MarketOrdersCommand = new DelegateCommand(async () => await ShowMarketOrdersPage());
            MarketHistoryCommand = new DelegateCommand(async () => await ShowMarketHistoryPage());
        }

        private async Task ShowMarketOrdersPage()
        {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedCoin", Coin);
            await NavigationService.NavigateAsync("MarketOrdersPage", parameters);
        }

        private async Task ShowMarketHistoryPage()
        {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedCoin", Coin);
            await NavigationService.NavigateAsync("MarketHistoryPage", parameters);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            Coin = parameters.GetValue<Coin>("SelectedCoin");
            Task.Run(async () => await GetMarket());
        }

        private async Task GetMarket()
        {
            IsBusy = true;
            try
            {
                Market = await RestRepository.GetMarket(Coin.Symbol);
            } catch (Exception e)
            {
                Crashes.TrackError(e);
                await PageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally
            {
                IsBusy = false;
            }
        }
    }
}

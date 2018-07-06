using Cryptopia.Public.Models;
using Cryptopia.Public.Rest;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptopia.Public.ViewModels {
    public class MainPageViewModel : ViewModelBase {
        private readonly IPageDialogService _pageDialogService;
        private readonly INavigationService _navigationService;
        private readonly IRestRepository _restRepository;
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand ItemTappedCommand { get; private set; }

        private string searchText;
        public string SearchText {
            get { return searchText; }
            set { SetProperty(ref searchText, value); }
        }

        private Coin selectedCoin;
        public Coin SelectedCoin {
            get { return selectedCoin; }
            set { SetProperty(ref selectedCoin, value); }
        }

        private ObservableCollection<Coin> coins;
        public ObservableCollection<Coin> Coins {
            get { return coins; }
            set { SetProperty(ref coins, value); }
        }

        private List<Coin> SourceCoins { get; set; }

        private static readonly int BitcoinId = 1;

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, 
            IRestRepository restRepository) : base(navigationService) {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _restRepository = restRepository;
            Coins = new ObservableCollection<Coin>();
            SourceCoins = new List<Coin>();
            SearchCommand = new DelegateCommand(Search);
            ItemTappedCommand = new DelegateCommand(ShowCoinDetails);
            RefreshCommand = new DelegateCommand(async () => await GetCoins());
            RefreshCommand.Execute();
        }

        public override void OnNavigatedFrom(NavigationParameters parameters) {
            SelectedCoin = null;
        }

        private async void ShowCoinDetails() {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedCoin", SelectedCoin);
            await _navigationService.NavigateAsync("CoinDetailPage", parameters);
        }

        private void Search() {
            Coins.Clear();
            if (SearchText != null && SearchText.Trim() != string.Empty) {
                Coins = new ObservableCollection<Coin>(SourceCoins.Where(c =>
                    c.Name.Trim().ToLower().Contains(SearchText.ToLower())
                    || c.Symbol.Trim().ToLower().Contains(SearchText.ToLower())));
            } else {
                Coins = new ObservableCollection<Coin>(SourceCoins);
            }
        }

        private async Task GetCoins() {
            try {
                IsBusy = true;
                var coins = await _restRepository.GetCoins();
                coins.Remove(coins.First(c => BitcoinId.Equals(c.Id)));
                Coins.Clear();
                SourceCoins.Clear();
                SourceCoins.AddRange(coins);
                foreach (var coin in coins) {
                    Coins.Add(coin);
                }
            } catch (Exception e) {
                await _pageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally {
                IsBusy = false;
            }
        }
    }
}

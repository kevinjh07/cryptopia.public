using Cryptopia.Public.Models;
using Cryptopia.Public.Rest;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Extended;

namespace Cryptopia.Public.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService PageDialogService;
        private readonly IRestRepository RestRepository;

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

        private InfiniteScrollCollection<Coin> coins;
        public InfiniteScrollCollection<Coin> Coins {
            get { return coins; }
            set { SetProperty(ref coins, value); }
        }

        private List<Coin> SourceCoins { get; set; }
        private const int BitcoinId = 1;
        private const int PageSize = 10;

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,
            IRestRepository restRepository) : base(navigationService)
        {
            PageDialogService = pageDialogService;
            RestRepository = restRepository;
            SourceCoins = new List<Coin>();
            Coins = new InfiniteScrollCollection<Coin>
            {
                OnLoadMore = OnLoadList(),
                OnCanLoadMore = () => Coins.Count < SourceCoins.Count
            };
            SearchCommand = new DelegateCommand(Search);
            ItemTappedCommand = new DelegateCommand(async () => await ShowCoinDetails());
            RefreshCommand = new DelegateCommand(async () => await GetCoins());
            RefreshCommand.Execute();
        }

        private Func<Task<IEnumerable<Coin>>> OnLoadList()
        {
            return async () =>
            {
                var page = Coins.Count / PageSize;
                return string.IsNullOrWhiteSpace(SearchText) ? await Task.Run(() => LoadCoins(page)) : null;
            };
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            SelectedCoin = null;
        }

        private async Task ShowCoinDetails()
        {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedCoin", SelectedCoin);
            await NavigationService.NavigateAsync("CoinDetailPage", parameters);
        }

        private void Search()
        {
            Coins.Clear();
            Coins.AddRange((SearchText != null && SearchText.Trim() != string.Empty) ? SourceCoins.Where(c =>
                c.Name.Trim().ToLower().Contains(SearchText.ToLower())
                || c.Symbol.Trim().ToLower().Contains(SearchText.ToLower())) : LoadCoins(0));
        }

        private async Task GetCoins()
        {
            IsBusy = true;
            try
            {
                var result = await RestRepository.GetCoins();
                result.Remove(result.First(c => BitcoinId.Equals(c.Id)));
                Coins.Clear();
                SourceCoins.Clear();
                SourceCoins.AddRange(result);
                Coins.AddRange(LoadCoins(0));
                if (!string.IsNullOrWhiteSpace(SearchText))
                    Search();
            } catch (Exception e)
            {
                Crashes.TrackError(e);
                await PageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally
            {
                IsBusy = false;
            }
        }

        private List<Coin> LoadCoins(int pageIndex)
        {
            return SourceCoins.Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
    }
}

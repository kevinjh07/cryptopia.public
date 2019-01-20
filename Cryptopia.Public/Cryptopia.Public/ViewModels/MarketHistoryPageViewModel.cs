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
using Microsoft.AppCenter.Crashes;

namespace Cryptopia.Public.ViewModels
{
    public class MarketHistoryPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService PageDialogService;
        private readonly IRestRepository RestRepository;
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
            IRestRepository restRepository) : base(navigationService)
        {
            PageDialogService = pageDialogService;
            RestRepository = restRepository;
            SourceList = new List<MarketHistory>();
            MarketHistories = new InfiniteScrollCollection<MarketHistory>
            {
                OnLoadMore = async () =>
                {
                    var page = MarketHistories.Count / PageSize;
                    return await Task.Run(() => LoadMarketHistory(page));
                },
                OnCanLoadMore = () => MarketHistories.Count < SourceList.Count
            };
            RefreshCommand = new DelegateCommand(async () => await GetMarketHistory());
        }

        public async override void OnNavigatingTo(NavigationParameters parameters)
        {
            CoinSymbol = parameters.GetValue<Coin>("SelectedCoin").Symbol;
            await GetMarketHistory();
        }

        private async Task GetMarketHistory()
        {
            IsBusy = true;
            SourceList.Clear();
            MarketHistories.Clear();
            try
            {
                SourceList.AddRange(await RestRepository.GetMarketHistory(CoinSymbol));
                MarketHistories.AddRange(LoadMarketHistory(0));
            } catch (Exception e)
            {
                Crashes.TrackError(e);
                await PageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally
            {
                IsBusy = false;
            }
        }

        private List<MarketHistory> LoadMarketHistory(int pageIndex)
        {
            return SourceList.Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
    }
}

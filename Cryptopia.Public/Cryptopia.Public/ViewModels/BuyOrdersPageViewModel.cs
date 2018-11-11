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
    public class BuyOrdersPageViewModel : ViewModelBase {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IRestRepository _restRepository;
        public DelegateCommand RefreshCommand { get; private set; }

        private InfiniteScrollCollection<OrdersData> buyOrders;
        public InfiniteScrollCollection<OrdersData> BuyOrders {
            get { return buyOrders; }
            set { SetProperty(ref buyOrders, value); }
        }

        public List<OrdersData> SourceList { get; set; }

        private Coin coin;
        public Coin Coin {
            get { return coin; }
            set { SetProperty(ref coin, value); }
        }

        private const int PageSize = 10;

        public BuyOrdersPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IRestRepository restRepository)
            : base(navigationService) {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _restRepository = restRepository;
            SourceList = new List<OrdersData>();
            BuyOrders = new InfiniteScrollCollection<OrdersData> {
                OnLoadMore = async () => {
                    var page = BuyOrders.Count / PageSize;
                    return await Task.Run(() => LoadBuyOrders(page));
                },
                OnCanLoadMore = () => BuyOrders.Count < SourceList.Count
            };
            RefreshCommand = new DelegateCommand(async () => await GetMarketDataOrders());
        }

        private async Task GetMarketDataOrders() {
            try {
                IsBusy = true;
                var marketOrders = await _restRepository.GetMarketOrdersData(Coin.Symbol);
                SourceList.Clear();
                BuyOrders.Clear();
                SourceList.AddRange(marketOrders.BuyOrders);
                BuyOrders.AddRange(LoadBuyOrders(0));
            } catch (Exception e) {
                await _pageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally {
                IsBusy = false;
            }
        }

        public async override void OnNavigatingTo(NavigationParameters parameters) {
            try {
                IsBusy = true;
                Coin = (Coin)parameters["SelectedCoin"];
                await GetMarketDataOrders();
            } finally {
                IsBusy = false;
            }
        }

        private List<OrdersData> LoadBuyOrders(int pageIndex) {
            return SourceList.Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
    }
}

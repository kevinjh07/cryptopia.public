
using Cryptopia.Public.Models;
using Cryptopia.Public.Rest;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Cryptopia.Public.ViewModels {
    public class BuyOrdersPageViewModel : ViewModelBase {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IRestRepository _restRepository;
        public DelegateCommand NavigateBackCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }

        private ObservableCollection<OrdersData> buyOrders;
        public ObservableCollection<OrdersData> BuyOrders {
            get { return buyOrders; }
            set { SetProperty(ref buyOrders, value); }
        }

        private bool isBusy;
        public bool IsBusy {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private Coin coin;
        public Coin Coin {
            get { return coin; }
            set { SetProperty(ref coin, value); }
        }

        public BuyOrdersPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IRestRepository restRepository)
            : base(navigationService) {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _restRepository = restRepository;
            BuyOrders = new ObservableCollection<OrdersData>();
            NavigateBackCommand = new DelegateCommand(async () => await _navigationService.GoBackAsync());
            RefreshCommand = new DelegateCommand(async () => await GetMarketDataOrders());
        }

        private async Task GetMarketDataOrders() {
            try {
                IsBusy = true;
                var marketOrders = await _restRepository.GetMarketOrdersData(Coin.Symbol);
                BuyOrders.Clear();
                foreach (var order in marketOrders.BuyOrders) {
                    BuyOrders.Add(order);
                }
            } catch (Exception e) {
                await _pageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally {
                IsBusy = false;
            }
        }

        public override void OnNavigatingTo(NavigationParameters parameters) {
            try {
                IsBusy = true;
                Coin = (Coin)parameters["SelectedCoin"];
                var marketordersData = (MarketOrders)parameters["MarketOrdersData"];
                if (Coin == null || marketordersData == null) {
                    NavigateBackCommand.Execute();
                }
                BuyOrders.Clear();
                foreach (var order in marketordersData.BuyOrders) {
                    BuyOrders.Add(order);
                }
            } finally {
                IsBusy = false;
            }
        }
    }
}

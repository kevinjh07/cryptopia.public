using Cryptopia.Public.Models;
using Cryptopia.Public.ViewModels;
using Prism.Commands;
using Prism.Navigation;

namespace CryptopiaPublic.ViewModels {
    public class MarketOrdersPageViewModel : ViewModelBase {
        private readonly INavigationService _navigationService;
        public DelegateCommand NavigateBackCommand { get; private set; }

        private Coin coin;
        public Coin Coin {
            get { return coin; }
            set { SetProperty(ref coin, value); }
        }

        public MarketOrdersPageViewModel(INavigationService navigationService) 
            : base(navigationService) {
            _navigationService = navigationService;
            NavigateBackCommand = new DelegateCommand(async () => await _navigationService.GoBackAsync());
        }

        public override void OnNavigatingTo(NavigationParameters parameters) {
            Coin = (Coin)parameters["SelectedCoin"];
            if (Coin == null) {
                NavigateBackCommand.Execute();
            }
        }
    }
}

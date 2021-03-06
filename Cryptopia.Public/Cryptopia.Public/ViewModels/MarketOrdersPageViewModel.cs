﻿using Cryptopia.Public.Models;
using Prism.Navigation;

namespace Cryptopia.Public.ViewModels
{
    public class MarketOrdersPageViewModel : ViewModelBase
    {
        public string CoinSymbol { get; set; }

        public MarketOrdersPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            CoinSymbol = parameters.GetValue<Coin>("SelectedCoin").Symbol;
        }
    }
}

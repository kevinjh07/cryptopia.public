﻿using Cryptopia.Public.Models;
using Cryptopia.Public.Rest;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms.Extended;
using Microsoft.AppCenter.Crashes;

namespace Cryptopia.Public.ViewModels
{
    public class SellOrdersPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService PageDialogService;
        private readonly IRestRepository RestRepository;
        public DelegateCommand RefreshCommand { get; private set; }

        private InfiniteScrollCollection<OrdersData> sellOrders;
        public InfiniteScrollCollection<OrdersData> SellOrders {
            get { return sellOrders; }
            set { SetProperty(ref sellOrders, value); }
        }

        private Coin coin;
        public Coin Coin {
            get { return coin; }
            set { SetProperty(ref coin, value); }
        }

        private List<OrdersData> SourceList { get; set; }
        private const int PageSize = 10;

        public SellOrdersPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,
            IRestRepository restRepository) : base(navigationService)
        {
            PageDialogService = pageDialogService;
            RestRepository = restRepository;
            SourceList = new List<OrdersData>();
            SellOrders = new InfiniteScrollCollection<OrdersData>
            {
                OnLoadMore = async () =>
                {
                    var page = SellOrders.Count / PageSize;
                    return await Task.Run(() => LoadSellOrders(page));
                },
                OnCanLoadMore = () => SellOrders.Count < SourceList.Count
            };
            RefreshCommand = new DelegateCommand(async () => await GetMarketDataOrders());
        }

        private async Task GetMarketDataOrders()
        {
            try
            {
                IsBusy = true;
                var marketOrders = await RestRepository.GetMarketOrdersData(Coin.Symbol);
                SourceList.Clear();
                SellOrders.Clear();
                SourceList.AddRange(marketOrders.SellOrders);
                SellOrders.AddRange(LoadSellOrders(0));
            } catch (Exception e)
            {
                Crashes.TrackError(e);
                await PageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
            } finally
            {
                IsBusy = false;
            }
        }

        public async override void OnNavigatingTo(NavigationParameters parameters)
        {
            IsBusy = true;
            try
            {
                Coin = parameters.GetValue<Coin>("SelectedCoin");
                await GetMarketDataOrders();
            } finally
            {
                IsBusy = false;
            }
        }

        private List<OrdersData> LoadSellOrders(int pageIndex)
        {
            return SourceList.Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
    }
}

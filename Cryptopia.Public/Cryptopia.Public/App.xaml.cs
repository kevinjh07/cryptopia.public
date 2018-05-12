using Prism;
using Prism.Unity;
using Cryptopia.Public.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cryptopia.Public.Rest;
using Prism.Ioc;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Cryptopia.Public {
    public partial class App : PrismApplication {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized() {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterSingleton<IRestRepository, RestRepository>();
            containerRegistry.RegisterForNavigation<CoinDetailPage>();
            containerRegistry.RegisterForNavigation<MarketOrdersPage>();
            containerRegistry.RegisterForNavigation<BuyOrdersPage>();
            containerRegistry.RegisterForNavigation<SellOrdersPage>();
            containerRegistry.RegisterForNavigation<MarketHistoryPage>();
        }
    }
}

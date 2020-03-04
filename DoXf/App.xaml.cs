using Xamarin.Essentials;
using Prism.Ioc;
using Prism;
using Prism.DryIoc;

namespace DoXf
{
    [AutoRegisterForNavigation]
    public partial class App : PrismApplication
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        public static bool UseMockDataStore = true;

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync($"{nameof(AppShell)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Dialog
            //containerRegistry.RegisterDialog<DemoDialog, DemoDialogViewModel>();

            // Pages
            //containerRegistry.RegisterForNavigation<NavigationPage>();
            //containerRegistry.RegisterForNavigation<AboutPage, AboutViewModel>();
            //containerRegistry.RegisterForNavigation<AppShell>();

            // Interface
            //containerRegistry.Register(typeof(IDashboardService), typeof(DashboardService));

            //if (UseMockDataStore)
            //    DependencyService.Register<MockDataStore>();
            //else
            //    DependencyService.Register<AzureDataStore>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

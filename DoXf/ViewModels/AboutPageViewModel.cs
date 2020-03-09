using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using DoXf.Pages;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DoXf.ViewModels
{
    public class AboutPageViewModel : BaseViewModel
    {
        public ICommand OpenWebCommand { get; }
        public ICommand OpenPageCommand { get; }

        public AboutPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
            OpenPageCommand = new DelegateCommand(OnOpenPage);
        }

        private void OnOpenPage()
        {
            NavigateAsync($"{nameof(SkiaSharpPage)}");
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            Debug.WriteLine("InitializeAsync AboutPage:" + parameters);

            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var mode = parameters.GetNavigationMode();
            Debug.WriteLine("OnNavigatedTo AboutPage: " + parameters + " Mode: " + mode);

        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            Debug.WriteLine("OnNavigatedFrom AboutPage:" + parameters);
        }
    }
}
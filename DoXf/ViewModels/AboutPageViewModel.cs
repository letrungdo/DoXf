using System.Windows.Input;
using DoXf.Views;
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
    }
}
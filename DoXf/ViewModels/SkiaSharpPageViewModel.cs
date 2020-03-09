using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace DoXf.ViewModels
{
    public class SkiaSharpPageViewModel : BaseViewModel
    {
        public ICommand TouchCommand { get; set; }

        public SkiaSharpPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Title = "SkiaSharpPage";
            TouchCommand = new DelegateCommand<string>(OnTouch);
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            Debug.WriteLine("InitializeAsync SkiaSharpPage:" + parameters);
            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Debug.WriteLine("OnNavigatedTo SkiaSharpPage:" + parameters);

            base.OnNavigatedTo(parameters);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            Debug.WriteLine("OnNavigatedFrom SkiaSharpPage:" + parameters);

            base.OnNavigatedFrom(parameters);
        }

        public override void Destroy()
        {
            Debug.WriteLine("Destroy SkiaSharpPage:");

            base.Destroy();
        }

        protected override void OnBack(BackMode? mode)
        {
            // todo edit params
            BackParams = new NavigationParameters();
            BackParams.Add("BackParams", "hahaha");
            base.OnBack(mode);
        }

        private void OnTouch(string obj)
        {
            switch (obj)
            {
                case "Left":
                    //NavigateAsync($"{nameof(SkiaSharpPage)}");
                    PageDialogService.DisplayAlertAsync(null, "Left", "Ok");
                    break;
                case "Right":
                    //NavigateAsync($"{nameof(AboutPage)}");
                    PageDialogService.DisplayAlertAsync(null, "Right", "Ok");

                    break;
            }
        }
    }
}

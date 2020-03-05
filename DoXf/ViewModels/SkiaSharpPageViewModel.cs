using System;
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
            TouchCommand = new DelegateCommand<string>(OnTouch);
            BackCommand = new DelegateCommand(OnBack);
        }

        private void OnBack()
        {
            GoBackAsync();
        }

        private void OnTouch(string obj)
        {
            switch(obj)
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

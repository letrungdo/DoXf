using Prism.Mvvm;
using Prism.Navigation;
using Prism.AppModel;
using Prism.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoXf.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware, IDestructible, IPageLifecycleAware, IApplicationLifecycleAware
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService PageDialogService { get; private set; }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public BaseViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
        }

        private bool _isRunning;
        public Task<INavigationResult> NavigateAsync(string pageName, INavigationParameters param = null, bool? useModal = null, bool animated = true)
        {
            var task = new TaskCompletionSource<INavigationResult>();
            // Block multi tap
            if (_isRunning)
                return task.Task;

            _isRunning = true;

            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await NavigationService.NavigateAsync(pageName, param, useModal, animated);
                if (!result.Success)
                {
                    _isRunning = false;
                }
                task.SetResult(result);
            });
            return task.Task;
        }

        public Task<INavigationResult> GoBackAsync(INavigationParameters param = null, bool? useModal = null, bool anim = true)
        {
            var task = new TaskCompletionSource<INavigationResult>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await NavigationService.GoBackAsync(param, useModal, anim);
                task.SetResult(result);
            });
            return task.Task;
        }


        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {

        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void OnSleep()
        {
        }
    }
}

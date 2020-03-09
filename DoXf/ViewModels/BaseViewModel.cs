using Prism.Mvvm;
using Prism.Navigation;
using Prism.AppModel;
using Prism.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using Prism.Commands;
using System;

namespace DoXf.ViewModels
{
    public enum BackMode
    {
        Press,
        Swipe,
    }
    public class BaseViewModel : BindableBase, INavigationAware, IDestructible, IPageLifecycleAware, IApplicationLifecycleAware, IInitializeAsync
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

        public ICommand BackCommand { get; set; }
        public INavigationParameters BackParams { get; protected set; }

        public BaseViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
            BackCommand = new DelegateCommand<BackMode?>(OnBack);
        }

        protected virtual void OnBack(BackMode? mode)
        {
            // Back params default is null
            if (mode != BackMode.Swipe)
            {
                GoBackAsync(BackParams);
            }
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

        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            _isRunning = false;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
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

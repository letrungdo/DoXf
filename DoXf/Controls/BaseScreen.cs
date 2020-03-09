using DoXf.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace DoXf.Controls
{
    public class BaseScreen : ContentPage
    {
        #region ------- BindableProperty -------
        public bool UsedSwipeBack
        {
            get { return (bool)GetValue(UsedSwipeBackProperty); }
            set { SetValue(UsedSwipeBackProperty, value); }
        }
        public static readonly BindableProperty UsedSwipeBackProperty =
            BindableProperty.Create(nameof(UsedSwipeBack), typeof(bool), typeof(BaseScreen), false);
        #endregion

        public BaseScreen()
        {
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as BaseViewModel)?.BackCommand?.Execute(BackMode.Press);
            //return base.OnBackButtonPressed();
            // Don't use button back default on Android
            return true;
        }
    }
}

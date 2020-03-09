using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DoXf.Controls;
using DoXf.iOS.Renderers;
using DoXf.ViewModels;
using Foundation;
using Prism.Common;
using Prism.DryIoc;
using Prism.Navigation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BaseScreen), typeof(BaseScreenRenderer))]
namespace DoXf.iOS.Renderers
{
    public class BaseScreenRenderer : PageRenderer, IUIGestureRecognizerDelegate
    {
        private bool _usedSwipeBack;
        private bool _isSwipedBack;
        BaseScreen _baseScreen;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                _baseScreen = e.NewElement as BaseScreen;
                _usedSwipeBack = _baseScreen.UsedSwipeBack;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            // Add swipeback gesture if not yet
            if (ViewController?.NavigationController?.InteractivePopGestureRecognizer != null)
            {
                ViewController.NavigationController.InteractivePopGestureRecognizer.Delegate = this;
                if (_usedSwipeBack)
                {
                    ViewController.NavigationController.InteractivePopGestureRecognizer.Enabled = true;

                    // Fix swipeback detector zone overlap on other elements.
                    ViewController.NavigationController.InteractivePopGestureRecognizer.CancelsTouchesInView = true;
                    ViewController.NavigationController.InteractivePopGestureRecognizer.DelaysTouchesBegan = false;
                    ViewController.NavigationController.InteractivePopGestureRecognizer.DelaysTouchesEnded = false;
                }
                else
                {
                    ViewController.NavigationController.InteractivePopGestureRecognizer.Enabled = false;
                }
            }
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            var coordinator = NavigationController?.TopViewController?.GetTransitionCoordinator();
            coordinator?.NotifyWhenInteractionEndsUsingBlock(contex =>
            {
                if (contex.IsCancelled)
                {
                    _isSwipedBack = false;
                }
                else
                {
                    _isSwipedBack = true;
                }
            });
        }

        /// <summary>
        /// Gesture shoulds the begin.
        /// return TRUE is default to tell the gesture recognizer to proceed with interpreting touches, 
        /// return FALSE to prevent it from attempting to recognize its gesture.
        /// </summary>
        [Export("gestureRecognizerShouldBegin:")]
        public bool ShouldBegin(UIGestureRecognizer recognizer)
        {
            if (!_baseScreen.UsedSwipeBack)
            {
                return false;
            }
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isSwipedBack)
            {
                try
                {
                    var vm = _baseScreen.BindingContext as BaseViewModel;
                    vm?.BackCommand?.Execute(BackMode.Swipe);

                    var navStack = Xamarin.Forms.Application.Current.MainPage?.Navigation?.NavigationStack;
                    Page previousPage = navStack?.Last();

                    var segmentParameters = UriParsingHelper.GetSegmentParameters(null, vm?.BackParams ?? new NavigationParameters());
                    ((INavigationParametersInternal)segmentParameters).Add("__NavigationMode", NavigationMode.Back);

                    PageUtilities.OnNavigatedFrom(_baseScreen, segmentParameters);
                    PageUtilities.OnNavigatedTo(previousPage, segmentParameters);
                    PageUtilities.DestroyPage(_baseScreen);

                    // Prevent NavigationPageSystemGoBackBehavior
                    // https://github.com/PrismLibrary/Prism/blob/master/Source/Xamarin/Prism.Forms/Behaviors/NavigationPageSystemGoBackBehavior.cs
                    var ns = (Xamarin.Forms.Application.Current as PrismApplication).Container.Resolve(typeof(INavigationService));
                    ns.GetType()
                        .GetProperties(BindingFlags.NonPublic | BindingFlags.Static)
                        .Single(pi => pi.Name == "NavigationSource").SetValue(ns, PageNavigationSource.NavigationService);
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            base.Dispose(disposing);
        }
    }
}

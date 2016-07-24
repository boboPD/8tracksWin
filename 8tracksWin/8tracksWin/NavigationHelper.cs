using Windows.UI.Core;
namespace _8tracksWin
{
    class NavigationHelper
    {
        public static void SetTitleBarBackButton()
        {
            SystemNavigationManager navManager = SystemNavigationManager.GetForCurrentView();
            if (Pages.Shell.ContentFrame.CanGoBack)
                navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        public static void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Pages.Shell.ContentFrame != null && Pages.Shell.ContentFrame.CanGoBack && !e.Handled)
            {
                e.Handled = true;
                Pages.Shell.ContentFrame.GoBack();
            }
        }
    }
}

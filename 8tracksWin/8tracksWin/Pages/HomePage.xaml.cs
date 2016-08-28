using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Common.Search;
using Common.Model;
using Common.Configuration;
using Windows.UI.Core;
using _8tracksWin.ViewModel;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _8tracksWin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        MixCollection trendingMixes { get; set; }
        MixCollection recommendedMixes { get; set; }
        MixCollection listenlaterMixes { get; set; }
        MixCollection historyMixes { get; set; }

        public HomePage()
        {
            this.InitializeComponent();

            if (GlobalConfigs.CurrentUser == null)
                UpdateLoggedInUserViews(false);

            Loading += FetchHomePageMixSets;
            GlobalConfigs.LoggedInUserExists += LoggedInUserStatusChange;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.SetTitleBarBackButton();
        }

        private void LoggedInUserStatusChange(object sender, bool isLoggedIn)
        {
            if (isLoggedIn && !mainPivot.Items.Contains(recommendedView))       //Double check if the views are already added to prevent duplicate addition
                UpdateLoggedInUserViews(true);
            else
                UpdateLoggedInUserViews(false);

        }

        private void UpdateLoggedInUserViews(bool toAdd)
        {
            if (toAdd)
            {
                mainPivot.Items.Add(recommendedView);
                mainPivot.Items.Add(historyView);
                mainPivot.Items.Add(listenLaterView);
            }
            else
            {
                mainPivot.Items.Remove(recommendedView);
                mainPivot.Items.Remove(historyView);
                mainPivot.Items.Remove(listenLaterView);
            }
        }

        private void PopulateCollection(MixCollection coll, Mix[] mixes)
        {
            foreach (var item in mixes)
            {
                coll.Add(item);
            }
        }

        private async void FetchHomePageMixSets(FrameworkElement sender, object args)
        {
            SearchResult trendingMixesSearch = await MixSearch.FetchTrendingMixes();
            trendingMixes = new MixCollection(trendingMixesSearch);
            trendingMixesLst.MixSource = trendingMixes;

            if (GlobalConfigs.CurrentUser != null)
            {
                Task<SearchResult> fetchRecommendedMixesTask = MixSearch.FetchMixesforUser(MixSearch.ListType.RECOMMENDED, GlobalConfigs.CurrentUser.UserId);
                Task<SearchResult> fetchListenLaterMixesTask = MixSearch.FetchMixesforUser(MixSearch.ListType.LISTEN_LATER, GlobalConfigs.CurrentUser.UserId);
                Task<SearchResult> fetchRecentlyPlayedMixesTask = MixSearch.FetchMixesforUser(MixSearch.ListType.HISTORY, GlobalConfigs.CurrentUser.UserId);
                
                fetchRecentlyPlayedMixesTask.ContinueWith((res) =>
                {
                    historyMixes = new MixCollection(res.Result);
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                            {
                                historyMixesLst.MixSource = historyMixes;
                                historyMixesLst.Visibility = Visibility.Visible;
                            }
                    );

                });
                fetchListenLaterMixesTask.ContinueWith((res) =>
                {
                    listenlaterMixes = new MixCollection(res.Result);
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                            {
                                listenLaterMixesLst.MixSource = listenlaterMixes;
                                listenLaterMixesLst.Visibility = Visibility.Visible;
                            }
                    );
                    
                });
                fetchRecommendedMixesTask.ContinueWith((res) =>
                {
                    recommendedMixes = new MixCollection(res.Result);
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                            {
                                recommendedMixesLst.Visibility = Visibility.Visible;
                            }
                    );
                });
            }
        }

        private void OnMixSelected(object sender, ItemClickEventArgs e)
        {
            Mix selectedMix = (Mix)e.ClickedItem;
            this.Frame.Navigate(typeof(NowPlayingPage), selectedMix);
        }
    }
}

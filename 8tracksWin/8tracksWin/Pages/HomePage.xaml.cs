using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Common.Search;
using Common.Model;
using Common.Configuration;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _8tracksWin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        ObservableCollection<Mix> trendingMixes { get; set; }
        ObservableCollection<Mix> recommendedMixes { get; set; }
        ObservableCollection<Mix> listenlaterMixes { get; set; }
        ObservableCollection<Mix> historyMixes { get; set; }

        System.Collections.Generic.Dictionary<MixSearch.ListType, SearchResult> searchResults;

        public HomePage()
        {
            this.InitializeComponent();
            trendingMixes = new ObservableCollection<Mix>();
            recommendedMixes = new ObservableCollection<Mix>();
            listenlaterMixes = new ObservableCollection<Mix>();
            historyMixes = new ObservableCollection<Mix>();

            searchResults = new System.Collections.Generic.Dictionary<MixSearch.ListType, SearchResult>();

            recommendedMixesLst.ItemsSource = recommendedMixes;
            listenLaterMixesLst.ItemsSource = listenlaterMixes;
            historyMixesLst.ItemsSource = historyMixes;
            trendingMixesLst.ItemsSource = trendingMixes;

            if (GlobalConfigs.CurrentUser == null)
                UpdateLoggedInUserViews(false);

            Loading += fetchHomePageMixSets;
            GlobalConfigs.LoggedInUserExists += LoggedInUserStatusChange;
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

        private void PopulateCollection(ObservableCollection<Mix> coll, Mix[] mixes)
        {
            foreach (var item in mixes)
            {
                coll.Add(item);
            }
        }

        private async void fetchHomePageMixSets(FrameworkElement sender, object args)
        {
            SearchResult trendingMixes = await MixSearch.FetchTrendingMixes();
            PopulateCollection(this.trendingMixes, trendingMixes.ResultSet.mixes);
            searchResults.Add(MixSearch.ListType.TRENDING, trendingMixes);

            if (GlobalConfigs.CurrentUser != null)
            {
                Task<SearchResult> fetchRecommendedMixesTask = MixSearch.FetchMixesforUser(MixSearch.ListType.RECOMMENDED, GlobalConfigs.CurrentUser.UserId);
                Task<SearchResult> fetchListenLaterMixesTask = MixSearch.FetchMixesforUser(MixSearch.ListType.LISTEN_LATER, GlobalConfigs.CurrentUser.UserId);
                Task<SearchResult> fetchRecentlyPlayedMixesTask = MixSearch.FetchMixesforUser(MixSearch.ListType.HISTORY, GlobalConfigs.CurrentUser.UserId);

                fetchRecentlyPlayedMixesTask.ContinueWith((res) =>
                {
                    PopulateCollection(this.historyMixes, res.Result.ResultSet.mixes);
                    searchResults.Add(MixSearch.ListType.HISTORY, res.Result);
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                            {
                                historyMixesLst.Visibility = Visibility.Visible;
                            }
                    );

                });
                fetchListenLaterMixesTask.ContinueWith((res) =>
                {
                    PopulateCollection(this.listenlaterMixes, res.Result.ResultSet.mixes);
                    searchResults.Add(MixSearch.ListType.LISTEN_LATER, res.Result);
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                            {
                                listenLaterMixesLst.Visibility = Visibility.Visible;
                            }
                    );
                    
                });
                fetchRecommendedMixesTask.ContinueWith((res) =>
                {
                    PopulateCollection(this.recommendedMixes, res.Result.ResultSet.mixes);
                    searchResults.Add(MixSearch.ListType.RECOMMENDED, res.Result);
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                            {
                                recommendedMixesLst.Visibility = Visibility.Visible;
                            }
                    );
                });
            }
        }
    }
}

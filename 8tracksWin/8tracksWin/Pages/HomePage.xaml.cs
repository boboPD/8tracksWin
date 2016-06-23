using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Common.Search;
using Common.Model;
using Common.Configuration;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _8tracksWin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public ObservableCollection<Mix> trendingMixes { get; set; }
        public ObservableCollection<Mix> recommendedMixes { get; set; }
        public ObservableCollection<Mix> listenlaterMixes { get; set; }
        public ObservableCollection<Mix> historyMixes { get; set; }

        public HomePage()
        {
            this.InitializeComponent();
            trendingMixes = new ObservableCollection<Mix>();
            trendingMixesLst.ItemsSource = trendingMixes;

            if (GlobalConfigs.CurrentUser != null)
            {
                recommendedMixes = new ObservableCollection<Mix>();
                listenlaterMixes = new ObservableCollection<Mix>();
                historyMixes = new ObservableCollection<Mix>();

                recommendedMixesLst.ItemsSource = recommendedMixes;
                listenLaterMixesLst.ItemsSource = listenlaterMixes;
                historyMixesLst.ItemsSource = historyMixes;
            }

            Loading += fetchHomePageMixSets;
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
            MixSet fetchTrendingMixes = await MixSearch.FetchTrendingMixes();
            PopulateCollection(trendingMixes, fetchTrendingMixes.mixes);

            if (GlobalConfigs.CurrentUser != null)
            {
                Task<MixSet> fetchRecommendedMixesTask = MixSearch.FetchRecommendedMixes(GlobalConfigs.CurrentUser.UserId);

                await Task.WhenAll<MixSet>(fetchRecommendedMixesTask);

                PopulateCollection(recommendedMixes, fetchRecommendedMixesTask.Result.mixes);
            }
        }
    }
}

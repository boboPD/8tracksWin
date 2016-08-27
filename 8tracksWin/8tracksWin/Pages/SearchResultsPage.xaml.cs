using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Common.Search;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace _8tracksWin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchResultsPage : Page
    {
        ViewModel.MixCollection searchResults;

        public SearchResultsPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            System.Collections.Generic.List<string> tags = (System.Collections.Generic.List<string>)e.Parameter;
            SearchResult results = await MixSearch.SearchMixesByTags(tags);
            searchResults = new ViewModel.MixCollection(results);
            searchResultsView.MixSource = searchResults;
        }
    }
}

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Common.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace _8tracksWin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NowPlayingPage : Page
    {
        CurrentMixProperties mixProps;
        Mix currentMix;

        public NowPlayingPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.mainGrid.DataContext = (Mix)e.Parameter;
            currentMix = (Mix)e.Parameter;
            mixProps = await Common.Music.Play(currentMix.id, Common.Music.ChangeSongUserAction.PLAY);
            player.Source = new System.Uri(mixProps.track.track_file_stream_url);
        }
    }
}

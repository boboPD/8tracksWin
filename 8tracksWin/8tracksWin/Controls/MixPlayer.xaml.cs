using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Common;
using Common.Model;
using System.Threading.Tasks;
using System.ComponentModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace _8tracksWin.Controls
{
    public sealed partial class MixPlayer : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        public MixPlayer()
        {
            this.InitializeComponent();
        }

        private string currentMixID;

        private CurrentMixProperties currMix;

        public event PropertyChangedEventHandler PropertyChanged;

        public CurrentMixProperties CurrMix
        {
            get { return currMix; }
            private set { currMix = value; PropertyChanged(this, new PropertyChangedEventArgs("CurrMix")); }
        }


        public async Task Play(string mixId)
        {
            CurrMix = await Music.Play(mixId, Music.ChangeSongUserAction.PLAY);
            root.DataContext = CurrMix;
            mediaPlayer.Source = new System.Uri(currMix.track.track_file_stream_url);
            currentMixID = mixId;
        }

        private void mediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Playing", false);
        }

        private async void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Source = new System.Uri((await Music.Play(currentMixID, Music.ChangeSongUserAction.NEXT)).track.track_file_stream_url);
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Source = new System.Uri((await Music.Play(currentMixID, Music.ChangeSongUserAction.SKIP)).track.track_file_stream_url);
        }
    }
}

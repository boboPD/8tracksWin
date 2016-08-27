using _8tracksWin.Pages;
using Common.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace _8tracksWin.Controls
{
    public sealed partial class MixesGridView : UserControl
    {
        public MixesGridView()
        {
            this.InitializeComponent();
        }

        private void OnMixClicked(object sender, ItemClickEventArgs e)
        {
            Mix selectedMix = (Mix)e.ClickedItem;
            Shell.ContentFrame.Navigate(typeof(NowPlayingPage), selectedMix);
        }

        public object MixSource
        {
            get { return (object)GetValue(MixSourceProperty); }
            set { SetValue(MixSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MixSourceProperty =
            DependencyProperty.Register("MixSource", typeof(object), typeof(MixesGridView), null);
    }
}

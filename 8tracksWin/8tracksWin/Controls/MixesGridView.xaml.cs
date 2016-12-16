using _8tracksWin.Pages;
using Common.Model;
using Common.Exceptions;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

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

        public Visibility ListenLaterButtonVisibility
        {
            get { return (Visibility)GetValue(ListenLaterButtonVisibilityProperty); }
            set { SetValue(ListenLaterButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowListenLaterButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListenLaterButtonVisibilityProperty =
            DependencyProperty.Register("ListenLaterButtonVisibility", typeof(Visibility), typeof(MixesGridView), null);



        public Visibility LikeButtonVisibility
        {
            get { return (Visibility)GetValue(LikeButtonVisibilityProperty); }
            set { SetValue(LikeButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LikeButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LikeButtonVisibilityProperty =
            DependencyProperty.Register("LikeButtonVisibility", typeof(Visibility), typeof(MixesGridView), null);



        private void gridvw_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (gridvw.ActualWidth > 1350)
            {
                ((ItemsWrapGrid)gridvw.ItemsPanelRoot).ItemWidth = (gridvw.ActualWidth - 30) / 3;
                gridvw.Style = (Style)this.Resources["mixGridViewWide"];
            }
            else if (gridvw.ActualWidth > 900)
            {
                ((ItemsWrapGrid)gridvw.ItemsPanelRoot).ItemWidth = (gridvw.ActualWidth - 20) / 2;
                gridvw.Style = (Style)this.Resources["mixGridViewWide"];
            }
            else if (gridvw.ActualWidth > 450)
            {
                ((ItemsWrapGrid)gridvw.ItemsPanelRoot).ItemWidth = (gridvw.ActualWidth - 10);
                gridvw.Style = (Style)this.Resources["mixGridViewWide"];
            }
            else
            {
                ((ItemsWrapGrid)gridvw.ItemsPanelRoot).ItemWidth = gridvw.ActualWidth - 10;
                gridvw.Style = (Style)this.Resources["mixGridViewSmall"];
            }
        }

        private async void tglListenlater_Checked(object sender, RoutedEventArgs e)
        {
            var tgBtn = (ToggleButton)e.OriginalSource;

            try
            {
                if (tgBtn.IsChecked.Value)
                    await Common.CollectionHandler.AddMixToListenLater((string)tgBtn.Tag);
                else
                    await Common.CollectionHandler.RemoveMixFromListenLater((string)tgBtn.Tag);
            }
            catch (EditCollectionException ex)
            {
                ContentDialog exceptionDialog = new ContentDialog()
                {
                    Content = ex.Message,
                    PrimaryButtonText = "OK"
                };

                await exceptionDialog.ShowAsync();
            }
        }
    }
}

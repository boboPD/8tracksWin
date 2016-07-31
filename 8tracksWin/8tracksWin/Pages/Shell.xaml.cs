using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Common.Configuration;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace _8tracksWin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public static Frame ContentFrame { get; private set; }
        private static char searchBoxTagSeparator = '+';

        public Shell(Frame frame)
        {
            this.InitializeComponent();
            this.shellView.Content = frame;
            ContentFrame = frame;

            if (GlobalConfigs.CurrentUser != null)
            {
                btnSignIn.Content = "Sign out";
                GlobalConfigs.CurrentUser.RefreshData();
                userDetailsPanel.DataContext = new ViewModel.UserDetailsViewModel(GlobalConfigs.CurrentUser);
                userDetailsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                btnSignIn.Content = "Sign in";
            }
            
            GlobalConfigs.LoggedInUserExists += SignedInUserStatusChangeHandler;
        }

        private void btnHamburger_Click(object sender, RoutedEventArgs e)
        {
            shellView.IsPaneOpen = !shellView.IsPaneOpen;
        }

        private void SignedInUserStatusChangeHandler(object sender, bool signedInUserExists)
        {
            if(signedInUserExists)
            {
                btnSignIn.Content = "Sign out";
                userDetailsPanel.DataContext = new ViewModel.UserDetailsViewModel(GlobalConfigs.CurrentUser);
                userDetailsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                btnSignIn.Content = "Sign in";
            }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (((Windows.UI.Xaml.Controls.Primitives.ButtonBase)sender).Content.Equals("Sign in"))
                ContentFrame.Navigate(typeof(LoginPage));
            else
                Common.Authentication.Logout();
        }

        private async void searchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if(searchBox.Text.Length >= 3 && args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string[] addedTags = searchBox.Text.Split(searchBoxTagSeparator);
                string partialTag = addedTags[addedTags.Length - 1];
                System.Collections.Generic.List<Common.Model.Tag> tagset = await Common.Search.TagSearch.FetchTags(partialTag);
                searchBox.ItemsSource = tagset;
            }
        }

        private void searchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion == null)
            {
                string[] temp = searchBox.Text.TrimEnd(searchBoxTagSeparator).Split(searchBoxTagSeparator);
                System.Collections.Generic.List<string> tagsList = new System.Collections.Generic.List<string>();
                foreach (string tag in temp)
                    tagsList.Add(tag.Replace(' ','_').Trim('"'));
                Shell.ContentFrame.Navigate(typeof(SearchResultsPage), tagsList);
            }
        }

        private void searchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            searchBox.Text = searchBox.Text.Remove(searchBox.Text.LastIndexOf(searchBoxTagSeparator) + 1);
            searchBox.Text = searchBox.Text + "\"" + ((Common.Model.Tag)args.SelectedItem).name + "\"" + searchBoxTagSeparator;
        }

        private void HamburgerMenuItemClick(object sender, ItemClickEventArgs e)
        {
            string itemClicked = (string)((ListViewItem)e.ClickedItem).Content;
            switch (itemClicked)
            {
                case "Home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
                case "Liked":
                    break;
                case "Listen Later":
                    break;
                default:
                    break;
            }
        }
    }
}

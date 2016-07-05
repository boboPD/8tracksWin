using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace _8tracksWin.Pages
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        public void OnPasswordOrUsernameTextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && !string.IsNullOrEmpty(txtUsername.Text))
                btnSubmit.IsEnabled = true;
            else
                btnSubmit.IsEnabled = false;
        }

        public async void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(await Common.Authentication.Login(txtUsername.Text, txtPassword.Password)))
            {
                lblLoginFailedMessage.Visibility = Visibility.Visible;
                txtPassword.Password = string.Empty;
                txtUsername.Text = string.Empty;
            }
            else
            {
                lblLoginFailedMessage.Visibility = Visibility.Collapsed;
                Shell.ContentFrame.Navigate(typeof(HomePage));
            }
        }
    }
}

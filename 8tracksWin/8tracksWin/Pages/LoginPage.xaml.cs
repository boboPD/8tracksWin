using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace _8tracksWin.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
            await Common.Authentication.Login(txtUsername.Text, txtPassword.Password);
        }
    }
}

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// This is a mock login page for admin functionality with hardcoded username and password.  
// To use system authentication and groups to display admin to only those, you would need to use a 
// Windows Desktop App.  For UWP you could use the method for kiosk apps detailed here: 
// https://docs.microsoft.com/en-us/windows-hardware/drivers/partnerapps/create-a-kiosk-app-for-assigned-access
// under the section "Secure your information".


namespace StoreInABox.TouchscreenApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MockAdminLogon : Page
    {
        private const string _mockUserName = "admin";
        private const string _mockPassword = "P@ssw0rd";

        public MockAdminLogon()
        {
            this.InitializeComponent();
        }

        public void Login_Clicked(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text == _mockUserName && pwbPassword.Password == _mockPassword)
            {
                this.Frame.Navigate(typeof(AdminPage));
            }
            else
            {
                txtUserName.Background = new SolidColorBrush(Colors.Red);
                pwbPassword.Background = new SolidColorBrush(Colors.Red);
            }
        }
        public void Back_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}

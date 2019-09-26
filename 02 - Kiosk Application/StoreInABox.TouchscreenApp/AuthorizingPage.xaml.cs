using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoreInABox.TouchscreenApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthorizingPage : Page
    {
        public AuthorizingPage()
        {
            this.InitializeComponent();
            this.Loaded += AuthorizingPage_Loaded;
        }

        private async void AuthorizingPage_Loaded(object sender, RoutedEventArgs e)
        {
            // If the card was declined, we'd handle that here.

            // this.Frame.Navigate(typeof(OpeningDoorPage));
        }

        private void Page_Clicked(object sender, RoutedEventArgs e)
        {
            // this.Frame.Navigate(typeof(DoRememberUserPage));
        }
    }
}

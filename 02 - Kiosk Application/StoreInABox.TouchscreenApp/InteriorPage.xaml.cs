using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoreInABox.TouchscreenApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InteriorPage : Page
    {
        public InteriorPage()
        {
            this.InitializeComponent();
        }

        public void DoorClosed_Clicked(object sender, RoutedEventArgs e)
        {
            txtBasketHeader.Visibility = Visibility.Visible;
            ShelvesBasketView.Visibility = Visibility.Visible;

            DoorClosedAsync();

        }

        public async void DoorClosedAsync()
        {
            await Task.Delay(4000);

            this.Frame.Navigate(typeof(ComeAgainPage));
        }
    }
}

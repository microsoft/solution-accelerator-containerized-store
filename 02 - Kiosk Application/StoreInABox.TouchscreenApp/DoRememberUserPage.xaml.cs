using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoreInABox.TouchscreenApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoRememberUserPage : Page
    {
        public DoRememberUserPage()
        {
            this.InitializeComponent();
        }

        private void OnYes_Clicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GetPhoneNumberPage));
        }

        private async void OnNo_Clicked(object sender, RoutedEventArgs e)
        {
            //await SignalHandler.WaitForDoorToOpen();
            Frame.Navigate(typeof(MainPage));
        }

        private void Back_Clicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}

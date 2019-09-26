using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StoreInABox.TouchscreenApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
        }

        public void Page_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EnterCardPage));
        }

        public void Admin_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MockAdminLogon));
        }

        async void App_Suspending(Object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            // TODO: This is the time to save app data in case the process is terminated.
        }
    }
}

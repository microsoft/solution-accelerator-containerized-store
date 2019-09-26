using System;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoreInABox.TouchscreenApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EnterCardPage : Page
    {
        public EnterCardPage()
        {
            this.InitializeComponent();
            this.Loaded += EnterCardPage_Loaded;
        }

        private async void EnterCardPage_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings =
            Windows.Storage.ApplicationData.Current.LocalSettings;
            // Configure SmartCard
            var sc = (string)localSettings.Values["SmartCard"];
            if (sc != "None")
            {
                SmartCardReader reader = await SmartCardReader.FromIdAsync((string)localSettings.Values["SmartCard"]);
                reader.CardRemoved += Reader_CardRemoved;
                reader.CardAdded += Reader_CardAdded;
            }
            else
            {
                Button cardButton = new Button { Content = "Process Card..." };
                cardButton.Click += CardButton_Click;
                cardButton.HorizontalAlignment = HorizontalAlignment.Center;
                cardButton.VerticalAlignment = VerticalAlignment.Top;
                cardButton.Margin = new Thickness(0, 350, 0, 0);
                cardButton.FontSize = 30;
                pgGrid.Children.Add(cardButton);
            }

        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                this.Frame.Navigate(typeof(AuthorizingPage));
                await Task.Delay(3000);
                this.Frame.Navigate(typeof(InteriorPage));
            });
        }

       private void Reader_CardAdded(SmartCardReader sender, CardAddedEventArgs args)
    {
        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        () =>
        {
            // A card has been inserted into the sender SmartCardReader.
            this.Frame.Navigate(typeof(AuthorizingPage));
        });
    }

    private void Reader_CardRemoved(SmartCardReader sender, CardRemovedEventArgs args)
    {
        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        () =>
        {
        // A card has been inserted into the sender SmartCardReader.
        this.Frame.Navigate(typeof(InteriorPage));
        });
        }

    private void Back_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}

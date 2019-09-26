using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using StoreInABox.DeviceManagement_Unv;
using StoreInABox.TouchscreenApp.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoreInABox.TouchscreenApp
{

    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {

        public AdminPage()
        {
            this.InitializeComponent();

            CameraManager cameraManager = CameraManager.Current;

            //this.ViewModel = new AdminViewModel();
        }

        public void Back_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // Stop the camera manager so that all the resources and threads are cleaned up.
            // Ideally we'd implement IDisposable on the CameraManager.
            //(this.DataContext as AdminViewModel).Stop();
            (this.DataContext as AdminViewModel).SaveStore();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

    }
    
}

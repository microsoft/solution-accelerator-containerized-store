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
            (this.DataContext as AdminViewModel).SaveStore();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }

        private void RootPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Get rid of the initial border around the back button by changing control in focus
            btnAddShelf.Focus(FocusState.Programmatic);
        }
    }
    
}

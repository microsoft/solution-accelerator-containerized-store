using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using StoreInABox.Container_Unv;
using System.Threading.Tasks;

namespace StoreInABox.TouchscreenApp
{
    //public class ProductItem
    //{
    //    public string Name { get; set; }
    //    public decimal Cost
    //    {
    //        get;
    //        set;
    //    }

    //    public ProductItem Clone()
    //    {
    //        return new ProductItem { Cost = this.Cost, Name = this.Name };
    //    }
    //}

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ComeAgainPage : Page, INotifyPropertyChanged
    {
        public ComeAgainPage()
        {
            this.InitializeComponent();
            this.Loaded += ComeAgainPage_Loaded;
            this.DataContext = this;
        }

        public ObservableCollection<ProductItem> Basket { get; set; } = new ObservableCollection<ProductItem>();

        public event PropertyChangedEventHandler PropertyChanged;

        private async void ComeAgainPage_Loaded(object sender, RoutedEventArgs e)
        {
            pr_ProgressRing1.IsActive = true;

            await Task.Delay(3000);

            pr_ProgressRing1.IsActive = false;

            foreach (var shelf in ((App)Application.Current).Store.Content)
            {
                foreach (var item in shelf.Basket)
                {
                    if (this.Basket != null)
                    {
                        this.Basket.Add(item);
                    }
                }
            }
            // If they did not take anything, stop the sales process
            if (this.Basket.Count == 0)
            {
                this.Frame.Navigate(typeof(MainPage));
            }

            PriceEllipse.Visibility = Visibility.Visible;
        }

        private void Page_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DoRememberUserPage));
        }
    }
}

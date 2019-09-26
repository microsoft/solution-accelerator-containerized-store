using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using StoreInABox.Container_Unv;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.UI.Xaml;

namespace StoreInABox.TouchscreenApp.ViewModels
{
    public class InteriorViewModel : INotifyPropertyChanged
    {

        Windows.Storage.StorageFolder AppLocalFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public event PropertyChangedEventHandler PropertyChanged;

        readonly ObservableCollection<Container_Unv.ProductItem> TotalBasket = new ObservableCollection<ProductItem>();

        public Store Store
        {
            get
            {
                return ((App)Application.Current).Store;
            }
            set
            {
                ((App)Application.Current).Store = value;
            }
        }

        public Shelf SelectedShelf { get; set; }


        public ICommand ReconcileCommand { get; set; }
        public ICommand CloseDoorCommand { get; }


        public InteriorViewModel()
        {

            this.ReconcileCommand = new DelegateCommand(this.Reconcile);
            //this.CloseDoorCommand = new DelegateCommand(this.CloseDoor);
            this.SelectedShelf = ((App)Application.Current).Store?.Content?.FirstOrDefault();

            StockCount();

        }

        //private async void CloseDoor(object param)
        //{
        //    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //    async () =>
        //    {
        //        this.Frame.Navigate(typeof(AuthorizingPage));
        //        await Task.Delay(3000);
        //        this.Frame.Navigate(typeof(InteriorPage));
        //    });
        //    this.Frame.Navigate(typeof(ComeAgainPage));
        //}


        private async void Reconcile(object param)
        {
            await ((App)Application.Current).Store.UpdateBasketAsync();

            var joiningList = new List<ProductItem>();

            foreach (var shelf in ((App)Application.Current).Store.Content)
            {
                IEnumerable<ProductItem> obsCollection = shelf.Basket;
                joiningList.AddRange(obsCollection);
            }

        }

        public async void StockCount()
        {
            await ((App)Application.Current).Store.UpdateStockAsync();
        }

        public async void SaveStore()
        {
            var jsonStore = JsonConvert.SerializeObject(((App)Application.Current).Store, Formatting.Indented,
    new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            StorageFile storeJson = await AppLocalFolder.CreateFileAsync("store.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(storeJson, jsonStore);
        }


        private async void DisplaySelectShelf()
        {
            ContentDialog selectShelfDialog = new ContentDialog()
            {
                Title = "No shelf selected",
                Content = "Please select a shelf and try again.",
                CloseButtonText = "Ok"
            };

            await selectShelfDialog.ShowAsync();
        }

    }
}

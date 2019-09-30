using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using StoreInABox.DeviceManagement_Unv;
using StoreInABox.Container_Unv;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace StoreInABox.TouchscreenApp.ViewModels
{
    public class AdminViewModel : INotifyPropertyChanged
    {

        private CardManager cardManager;

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

        public ObservableCollection<Tuple<string, string>> UiAllCameras { get; set; }

        public ObservableCollection<Tuple<string, string>> ComboAllCardReaders { get; set; }

        public string PredictionKey
        {
            get
            {
                Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
                return (string) localSettings.Values["PredictionKey"];
            }
            set
            {
                Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["PredictionKey"] = (string) value;
                foreach (var shelf in Store.Content)
                {
                    shelf.PredictionKey = (string)value;
                }
            }
        }

        public string PredictionUri
        {
            get
            {
                Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
                return (string)localSettings.Values["PredictionUri"];
            }
            set
            {
                Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["PredictionUri"] = (string)value;
                foreach (var shelf in Store.Content)
                {
                    shelf.PredictionUri = (string)value;
                }
            }
        }

        public string SelectedCmbCardReader
        {
            get
            {
                Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
                return (string) localSettings.Values["SmartCard"];
            }
            set
            {
                Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["SmartCard"] = value;
            }
        }

        public CameraManager CameraManager { get { return ((App)Application.Current).CameraManager; } }
        public ICommand AddShelfCommand { get; set; }
        public ICommand RemoveShelfCommand { get; set; }
        public ICommand AddCameraCommand { get; set; }
        public ICommand RemoveCameraCommand { get; set; }
        public ICommand CaptureImagesCommand { get; set; }


        public AdminViewModel()
        {
            // Note; this is not the best way to achieve async initialization of bindable properties
            // you could run into problems with error handling using 'async void'.  For a better, but
            // too obfuscated pattern for this article, see https://msdn.microsoft.com/en-us/magazine/dn605875.aspx

            ShowSmartCardsAsync();

            UiAllCameras = CameraManager.AllCamerasMeta;

            this.AddShelfCommand = new DelegateCommand(this.AddShelf);
            this.RemoveShelfCommand = new DelegateCommand(this.RemoveShelf);
            this.AddCameraCommand = new DelegateCommand(this.AddCamera);
            this.RemoveCameraCommand = new DelegateCommand(this.RemoveCamera);
            this.CaptureImagesCommand = new DelegateCommand(this.CaptureImages);
            this.SelectedShelf = ((App)Application.Current).Store?.Content?.FirstOrDefault();

        }

        private async void ShowSmartCardsAsync()
        {
            cardManager = new CardManager();
            await cardManager.GetAllSmartCardsAsync();
            ComboAllCardReaders = cardManager.AllReadersInfo;
            ComboAllCardReaders.Add(new Tuple<string, string>("No Card reader", "None"));
        }

        public void AddShelf(object param)
        {
            ((App)Application.Current).Store.Content.Add(new Shelf() {
                Name = $"Shelf {((App)Application.Current).Store.Content.Count + 1}",
                PredictionKey = this.PredictionKey,
                PredictionUri = this.PredictionUri });

            this.SaveStore();
        }

        public void RemoveShelf(object param)
        {
            var thisShelf = param as Shelf;

                foreach (var cam in thisShelf.CameraContainer.Cameras)
                {
                    cam.IsAvailable = true;
                }

            Store.Content.Remove(thisShelf);
            this.SaveStore();
        }

        public async void CaptureImages(object param)
        {
            await CameraManager.Current.UpdateAllSnapshotsAsync();
        }

        public void AddCamera(object param)
        {
            Camera cam = param as Camera;
            
            if(this.SelectedShelf == null)
            {
                DisplaySelectShelf();
                return;
            }

            if (this.SelectedShelf.CameraContainer == null)
            {
                this.SelectedShelf.CameraContainer = new CameraContainer();
            }

            this.SelectedShelf.CameraContainer.Cameras.Add(cam);
            cam.IsAvailable = false;
            this.SaveStore();
        }

        public void RemoveCamera(object param)
        {
            var thisContentPresenter = GetNthParent(6, param as DependencyObject);
            this.SelectedShelf = ((thisContentPresenter as ContentPresenter).DataContext as Shelf);
            Camera cam = (param as ContentPresenter).DataContext as Camera;
            if (this.SelectedShelf == null)
            {
                DisplaySelectShelf();
                return;
            }
            this.SelectedShelf.CameraContainer.Cameras.Remove(cam);
            cam.IsAvailable = true;
            this.SaveStore();
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

        private DependencyObject GetNthParent(int n, DependencyObject dobj)
        {
            for (int x=0; x < n; x++)
            {
                dobj = VisualTreeHelper.GetParent(dobj);
            }
            return dobj;
        }
    }
}

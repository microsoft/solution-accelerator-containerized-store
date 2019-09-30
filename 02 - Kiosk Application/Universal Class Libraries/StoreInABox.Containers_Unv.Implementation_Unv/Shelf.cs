using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Cognitive.CustomVision.Prediction;
using StoreInABox.DeviceManagement_Unv;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace StoreInABox.Container_Unv
{
    public class Shelf : INotifyPropertyChanged
    {
        public ObservableCollection<ProductItem> Stock { get; private set; } = new ObservableCollection<ProductItem>();
        public ObservableCollection<ProductItem> NewStock { get; private set; } = new ObservableCollection<ProductItem>();

        public ObservableCollection<ProductItem> Basket { get; private set; } = new ObservableCollection<ProductItem>();

        public CameraContainer CameraContainer { get; set; } = new CameraContainer();

        public string Name { get; set; }

        public string PredictionKey { get; set; }

        public string PredictionUri { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task UpdateStockAsync()
        {
            await this.Update(true);
        }

        public async Task UpdateBasketAsync()
        {
            await this.Update(false);

            this.Basket.Clear();

            // Find the items taken but removing all the current items from a list of the origional
            // This coppies the item references to a new collection, but items point to the origional
            // Have to add this to Basket from temp to correctly generate change events
            var temp = new ObservableCollection<ProductItem>(this.Stock);
            foreach (var item in this.NewStock) temp.Remove(item);
            foreach (var item in temp) this.Basket.Add(item);
        }

        private async Task Update(bool stockTaking)
        {

            if (stockTaking)
            {
                this.Stock.Clear();
            }
            else
            {
                this.NewStock.Clear();
            }

                foreach (var camera in this?.CameraContainer?.Cameras)
                {
                    // Check to make sure stream is not shutdown

                        await camera.TakeSnapshotAsync();


                    using (var stream = new InMemoryRandomAccessStream())
                    {
                        try
                        {
                            // Create an encoder with the desired format
                            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                            encoder.SetSoftwareBitmap(camera.LastImage);

                            await encoder.FlushAsync();

                        }
                        catch (Exception ex)
                        {

                        }

                        var result = await CustomVision.Predict(stream.AsStream(), this.PredictionKey, this.PredictionUri);
                        var predictions = result.Predictions.OrderBy(p => p.Probability);


                        foreach (var prediction in predictions.Where(p => p.Probability >= 0.20))
                        {
                            if (stockTaking)
                            {
                                this.Stock.Add(ProductItems.Items.Single(p => p.Name == prediction.TagName));
                            }
                            else
                            {
                                this.NewStock.Add(ProductItems.Items.Single(p => p.Name == prediction.TagName));
                            }                            
                        }
                    }
                }
        }
    }
}

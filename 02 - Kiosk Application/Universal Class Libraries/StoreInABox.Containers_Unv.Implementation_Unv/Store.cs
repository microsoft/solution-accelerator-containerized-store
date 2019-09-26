using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace StoreInABox.Container_Unv
{
    public class Store : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public ObservableCollection<Shelf> Content { get; set; }   

        public Store()
        {
            this.Content = new ObservableCollection<Shelf>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task UpdateStockAsync()
        {
            foreach(var shelf in this.Content)
            {
                await shelf.UpdateStockAsync();
            }
        }

        public async Task UpdateBasketAsync()
        {
            try
            {
                foreach (var shelf in this.Content)
                {
                    await shelf.UpdateBasketAsync();
                }
            }
            catch(Exception e)
            {

            }
        }

        public decimal Total
        {
            get
            {
                decimal total = 0;

                foreach(var shelf in this.Content)
                {
                    total += shelf.Basket.Sum(p => p.Cost);
                }

                return total;
            }
        }
    }
}

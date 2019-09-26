using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SmartCards;

namespace StoreInABox.DeviceManagement_Unv
{
    public class CardManager : INotifyPropertyChanged
    {
        //public event EventHandler CardInserted;
        public event EventHandler CardRemoved;

        public ObservableCollection<Tuple<string, string>> AllReadersInfo { get; set; } = new ObservableCollection<Tuple<string, string>>();

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task GetAllSmartCardsAsync()
        {
            string selector = SmartCardReader.GetDeviceSelector();
            var readers = await DeviceInformation.FindAllAsync(selector);

            foreach (var reader in readers)
            {
                this.AllReadersInfo.Add(new Tuple<string,string>(reader.Name, reader.Id));
            }
        }

        public async Task SubscribeToEvents(string cardId)
        {

        }
    }

}

using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoreInABox.TouchscreenApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GetPhoneNumberPage : Page, INotifyPropertyChanged
    {
        private const string DEFAULT_TEXT = "Please enter your phone number.";

        public event PropertyChangedEventHandler PropertyChanged;

        public string PhoneNumber { get; set; }

        public GetPhoneNumberPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            PhoneNumber = DEFAULT_TEXT;
        }

        public async void Continue_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void CheckTextFormatting()
        {        
            if (PhoneNumber == DEFAULT_TEXT)
                PhoneNumber = "";

            // ###
            else if (PhoneNumber.Length >= 3 && !PhoneNumber.Contains("-"))
                PhoneNumber += "-";

            // ###-###
            else if (PhoneNumber.Length >= 7 && PhoneNumber.Count(x => x == '-') < 2)
                PhoneNumber += "-";
            else if(PhoneNumber.Length > 11)
            {
                PhoneNumber = this.PhoneNumber.Substring(0, 11);
            }
        }

        public void One_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "1";
        }

        public void Two_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "2";
        }

        public void Three_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "3";
        }

        public void Four_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "4";
        }

        public void Five_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "5";
        }

        public void Six_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "6";
        }

        public void Seven_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "7";
        }

        public void Eight_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "8";
        }

        public void Nine_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "9";
        }

        public void Zero_Clicked(object sender, RoutedEventArgs e)
        {
            CheckTextFormatting();
            PhoneNumber += "0";
        }

        public void Clear_Clicked(object sender, RoutedEventArgs e)
        {
            PhoneNumber = DEFAULT_TEXT;
        }

        public void Delete_Clicked(object sender, RoutedEventArgs e)
        {
            if (PhoneNumber.Length > 1)
            {
                if (PhoneNumber.Last() == '-')
                    PhoneNumber = PhoneNumber.Remove(PhoneNumber.Length - 1);

                PhoneNumber = PhoneNumber.Remove(PhoneNumber.Length - 1);
            }

            else if (PhoneNumber.Length == 1)
                PhoneNumber = DEFAULT_TEXT;

            else
            {
                // Text should be default text; do nothing.
            }
        }

        public void Back_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}

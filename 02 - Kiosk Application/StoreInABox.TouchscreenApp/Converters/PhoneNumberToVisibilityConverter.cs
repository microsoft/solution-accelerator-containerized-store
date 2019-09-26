using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace StoreInABox.TouchscreenApp
{
    public class PhoneNumberToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var phonenumber = (string)value;

            if(!string.IsNullOrWhiteSpace(phonenumber))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

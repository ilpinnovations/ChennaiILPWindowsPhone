using System;
using Windows.UI.Xaml.Data;

namespace Chennai_ILP.Code
{
    class ImageURLConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string relativeUrl = (string)value;
            string abs = HostelManager.BASE_URL + relativeUrl;

            return abs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

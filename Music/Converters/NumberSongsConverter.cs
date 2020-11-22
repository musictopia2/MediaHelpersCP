using CommonBasicStandardLibraries.CommonConverters;
using System;
using System.Globalization;
namespace MediaHelpersCP.Music.Converters
{
    public abstract class NumberSongsConverter : IConverterCP
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"Total Songs : {value.ToString()}";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
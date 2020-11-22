using CommonBasicStandardLibraries.CommonConverters;
using System;
using System.Globalization;
namespace MediaHelpersCP.Music.Converters
{
    public abstract class LabelSongsConverter : IConverterCP
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "0")
            {
                return "";
            }

            return $"Total Possible Songs To Choose: {value.ToString()}";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using CommonBasicStandardLibraries.CommonConverters;
using System;
using System.Globalization;
namespace MediaHelpersCP.Music.Converters
{
    public abstract class ChristmasButtonConverter : IConverterCP
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool rets = bool.Parse(value.ToString());
            if (rets == true)
            {
                return "Non Christmas Only";
            }

            return "Christmas Only";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
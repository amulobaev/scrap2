using System;
using System.Globalization;
using System.Windows.Data;
using NLog;
using Scrap.Core.Tools;

namespace Scrap.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumDescriptionConverter : IValueConverter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value == null)
            //    return string.Empty;

            Enum myEnum = null;
            try
            {
                myEnum = (Enum)value;
            }
            catch (Exception ex)
            {
                if (Logger.IsErrorEnabled)
                    Logger.Error(ex);
            }

            string description = Helpers.GetEnumDescription(myEnum);
            return description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}

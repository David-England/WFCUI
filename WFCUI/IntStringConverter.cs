using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WFCUI
{
    class IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            try
            {
                return AttemptParseToInt(value.ToString());
            }
            catch (FormatException fe)
            {
                MessageBox.Show(fe.Message, "You've got a problem....", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            catch (OverflowException)
            {
                MessageBox.Show("Stop it.", "You've got a problem....", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private object AttemptParseToInt(string intText)
        {
            if (intText == "") return null;
            return System.Convert.ToInt32(intText);
        }
    }
}
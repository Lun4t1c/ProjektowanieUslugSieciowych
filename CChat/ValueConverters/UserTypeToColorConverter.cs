using CChat.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CChatClientGUI.ValueConverters
{
    internal class UserTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((UserType)value)
            {
                case UserType.YOU:
                    return Brushes.Green;

                case UserType.OTHER_USER:
                    return Brushes.LightBlue;

                case UserType.SYSTEM:
                    return Brushes.Red;

                case UserType.MODERATOR:
                    return Brushes.Yellow;

                default:
                    return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

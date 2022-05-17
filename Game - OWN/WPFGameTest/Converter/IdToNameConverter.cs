using Game.Logic;
using Game.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Game.Converter
{
    public class IdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = MultiLogic.locals.lobby.Users.FirstOrDefault(x => x.Id == value.ToString());
            return user == null ? "" : user.Username;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            User user = MultiLogic.locals.lobby.Users.FirstOrDefault(x => x.Id == value.ToString());
            return user == null ? new SolidColorBrush(Colors.Black) : new SolidColorBrush((Color)ColorConverter.ConvertFromString(user.Color));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class BorderRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (MultiLogic.locals.lobby.Users.FirstOrDefault(x => x.Id == value.ToString()).Id == MultiLogic.locals.user.Id)
            {
                return new CornerRadius(15, 15, 0, 15);
            }
            else
            {
                return new CornerRadius(15, 15, 15, 0);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class OrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (MultiLogic.locals.lobby.Users.FirstOrDefault(x => x.Id == value.ToString()).Id == MultiLogic.locals.user.Id)
            {
                return HorizontalAlignment.Right;
            }
            else
            {
                return HorizontalAlignment.Left;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class ColumnSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (MultiLogic.locals.lobby.Users.FirstOrDefault(x => x.Id == value.ToString()).Id == MultiLogic.locals.user.Id)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ParticleEditor.Data;
using ParticleEditor.ViewModels.ParameterTabs;

namespace ParticleEditor.Helpers
{
    public class EnumToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.ToObject(targetType, value);
        }
    }

    public class StringToFloat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return float.Parse(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((float)value).ToString();
        }
    }

    public class BurstConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Burst b = new Burst();
            if (float.TryParse(values[0].ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out b.Time) ==
                false)
                return new Burst();
            if (int.TryParse(values[1].ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out b.Amount) ==
                false)
                return new Burst();
            return b;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

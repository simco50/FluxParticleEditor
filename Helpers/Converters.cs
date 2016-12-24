using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using ParticleEditor.Model;
using ParticleEditor.ViewModels.ParameterTabs;

namespace ParticleEditor.Helpers
{
    public class EnumToInt : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.ToObject(targetType, value);
        }

        private static EnumToInt _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new EnumToInt();
            return _converter;
        }
    }

    public class BurstConverter : MarkupExtension, IMultiValueConverter
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
            Burst b = (Burst) value;
            object[] values = new object[] {b.Time, b.Amount};
            return values;
        }

        private static BurstConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new BurstConverter();
            return _converter;
        }
    }
}

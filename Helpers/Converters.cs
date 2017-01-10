using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using ParticleEditor.Model;
using ParticleEditor.Model.Data;
using ParticleEditor.ViewModels.ParameterTabs;
using SharpDX;
using Color = System.Windows.Media.Color;

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

    public class Vector3ToBrushConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Vector3 v = (Vector3) value;
            return new SolidColorBrush(Color.FromRgb((byte)(v.X * 255), (byte)(v.Y * 255), (byte)(v.Z * 255)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = ((SolidColorBrush) value).Color;
            return new Vector3(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f);
        }

        private static Vector3ToBrushConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new Vector3ToBrushConverter();
            return _converter;
        }
    }

    public class InvertBoolConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        private static InvertBoolConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new InvertBoolConverter();
            return _converter;
        }
    }

    public class FloatsToKeyValuePairConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            float k = float.Parse(values[0].ToString());
            float v = float.Parse(values[1].ToString());
            return new KeyValuePair<float, float>(k, v);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] {((KeyValuePair<float, float>) value).Key, ((KeyValuePair<float, float>) value).Value};
        }

        private static FloatsToKeyValuePairConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new FloatsToKeyValuePairConverter();
            return _converter;
        }
    }

    public class VectorToKeyValuePairConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                float k = float.Parse(values[0].ToString());
                float v1 = float.Parse(values[1].ToString());
                float v2 = float.Parse(values[2].ToString());
                float v3 = float.Parse(values[3].ToString());
                return new KeyValuePair<float, Vector3>(k, new Vector3(v1, v2, v3));

            }
            catch (Exception)
            {
                return new KeyValuePair<float, Vector3>();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[]
            {
                ((KeyValuePair<float, Vector3>) value).Key, ((KeyValuePair<float, Vector3>) value).Value.X,
                ((KeyValuePair<float, Vector3>) value).Value.Y, ((KeyValuePair<float, Vector3>) value).Value.Z
            };
        }

        private static VectorToKeyValuePairConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new VectorToKeyValuePairConverter();
            return _converter;
        }
    }

    public class FloatsToVector3Converter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                float x = float.Parse(values[0].ToString());
                float y = float.Parse(values[1].ToString());
                float z = float.Parse(values[2].ToString());
                return new Vector3(x, y, z);
            }
            catch (Exception)
            {
                return new Vector3();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Vector3 v = (Vector3) value;
            return new object[] {v.X, v.Y, v.Z};
        }

        private static FloatsToVector3Converter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new FloatsToVector3Converter();
            return _converter;
        }
    }
}

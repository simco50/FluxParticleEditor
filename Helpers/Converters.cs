using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using ParticleEditor.Model.Data;
using ParticleEditor.ViewModels.ParameterTabs;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace ParticleEditor.Helpers
{
    //Used for particle system sorting mode, shape and rendering mode
    public class EnumToInt : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) return (int) value;
            throw new Exception("[EnumToInt::Convert] > Conversion failed!");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) return Enum.ToObject(targetType, value);
            throw new Exception("[EnumToInt::ConvertBack] > Conversion failed!");
        }

        private static EnumToInt _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new EnumToInt();
            return _converter;
        }
    }

    //Used to add a burst (In burst tab)
    [ValueConversion(typeof(object[]), typeof(Burst))]
    public class BurstConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Burst b = new Burst();
            if (float.TryParse(values[0].ToString(), out b.Time) ==
                false)
                return new Burst();
            if (int.TryParse(values[1].ToString(), out b.Amount) ==
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

        private static BurstConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new BurstConverter();
            return _converter;
        }
    }

    //Used for color picker in animation tab
    [ValueConversion(typeof(CustomVector3), typeof(SolidColorBrush))]
    public class Vector3ToBrushConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CustomVector3 v = (CustomVector3) value;
            if(v == null) throw new Exception("[Vector3ToBrushConverter] > Conversion failed!");
            return new SolidColorBrush(Color.FromRgb((byte)(v.X * 255), (byte)(v.Y * 255), (byte)(v.Z * 255)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush c = (SolidColorBrush) value;
            if(c == null) throw new Exception("[ConvertBack] > Conversion failed!");
            return new Vector3(c.Color.R / 255.0f, c.Color.G / 255.0f, c.Color.B / 255.0f);
        }

        private static Vector3ToBrushConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new Vector3ToBrushConverter();
            return _converter;
        }
    }

    //Used to enable/disable the constant value in the animation tab
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBoolConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !(bool) value;
        }

        private static InvertBoolConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new InvertBoolConverter();
            return _converter;
        }
    }
    
    //Used for adding a float keyframe (FloatKeyframeView)
    [ValueConversion(typeof(float[]), typeof(KeyValuePair<float, float>))]
    public class FloatsToKeyValuePairConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            float key, value;
            if (float.TryParse(values[0].ToString(), out key) == false)
                return new KeyValuePair<float, float>(0, 0);
            if(float.TryParse(values[1].ToString(), out value) == false)
                return new KeyValuePair<float, float>(0, 0);
            return new KeyValuePair<float, float>(ApplicationHelper.Round(key, 2), ApplicationHelper.Round(value, 2));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] {((KeyValuePair<float, float>) value).Key, ((KeyValuePair<float, float>) value).Value};
        }

        private static FloatsToKeyValuePairConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new FloatsToKeyValuePairConverter();
            return _converter;
        }
    }

    //Used for adding a vector3 keyframe (VectorKeyframeView)
    [ValueConversion(typeof(float[]), typeof(KeyValuePair<float, Vector3>))]
    public class VectorToKeyValuePairConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            float key, x, y, z;
            if(float.TryParse(values[0].ToString(), out key) == false) return new KeyValuePair<float, Vector3>(0, new Vector3());
            if(float.TryParse(values[1].ToString(), out x) == false) return new KeyValuePair<float, Vector3>(0, new Vector3());
            if(float.TryParse(values[2].ToString(), out y) == false) return new KeyValuePair<float, Vector3>(0, new Vector3());
            if (float.TryParse(values[3].ToString(), out z) == false) return new KeyValuePair<float, Vector3>(0, new Vector3());
            return new KeyValuePair<float, Vector3>(ApplicationHelper.Round(key, 2),
                new Vector3(ApplicationHelper.Round(x, 2), ApplicationHelper.Round(y, 2), ApplicationHelper.Round(z, 2)));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[]
            {
                ((KeyValuePair<float, Vector3>) value).Key, ((KeyValuePair<float, Vector3>) value).Value
            };
        }

        private static VectorToKeyValuePairConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new VectorToKeyValuePairConverter();
            return _converter;
        }
    }

    public class NumericValueToFloat : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? v = value as double?;
            if (v != null)
                return v.Value;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) return (double)value;
            throw new Exception("[NumericValueToFloat::ConvertBack] Conversion failed!");
        }

        private static NumericValueToFloat _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new NumericValueToFloat();
            return _converter;
        }
    }

    //Used for setting the backgroundcolor of an animated property depending on if its animated
    [ValueConversion(typeof(bool), typeof(SolidColorBrush))]
    public class BoolToSolidColorBrush : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = (bool)value;
            return v
                ? new SolidColorBrush(Color.FromArgb(80, 255, 0, 0))
                : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static BoolToSolidColorBrush _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new BoolToSolidColorBrush();
            return _converter;
        }
    }
}

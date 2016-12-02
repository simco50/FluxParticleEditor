using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ParticleEditor.Data;

namespace ParticleEditor.Converters
{
    [ValueConversion(typeof(ParticleSystem.ParticleSortingMode), typeof(int))]
    public class SortingModeToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ParticleSystem.ParticleSortingMode) value;
        }
    }
}

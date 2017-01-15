using System.Windows;
using System.Windows.Controls;
using ParticleEditor.Model.Data;

namespace ParticleEditor.Views
{
    /// <summary>
    /// Interaction logic for Vector3Control.xaml
    /// </summary>
    public partial class Vector3Control : UserControl
    {
        public Vector3Control()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(CustomVector3), typeof(Vector3Control), new PropertyMetadata(new CustomVector3()));

        public CustomVector3 Value
        {
            get { return (CustomVector3) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
            "Interval", typeof(float), typeof(Vector3Control), new PropertyMetadata(1.0f));

        public float Interval
        {
            get { return (float) GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
    }
}

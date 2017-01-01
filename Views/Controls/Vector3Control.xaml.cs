using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpDX;

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
            "Value", typeof(Vector3), typeof(Vector3Control), new PropertyMetadata(default(Vector3)));

        public Vector3 Value
        {
            get { return (Vector3) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public float X
        {
            get { return Value.X; }
            set { Value = new Vector3(value, Value.Y, Value.Z); }
        }

        public float Y
        {
            get { return Value.Y; }
            set { Value = new Vector3(Value.X, value, Value.Z); }
        }

        public float Z
        {
            get { return Value.Z; }
            set { Value = new Vector3(Value.X, Value.Y, value); }
        }
    }
}

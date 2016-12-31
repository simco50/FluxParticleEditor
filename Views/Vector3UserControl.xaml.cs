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
    /// Interaction logic for Vector3UserControl.xaml
    /// </summary>
    public partial class Vector3UserControl : UserControl
    {
        public Vector3UserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty VectorProperty = DependencyProperty.Register(
            "Vector", typeof(Vector3), typeof(Vector3UserControl), new PropertyMetadata(default(Vector3)));

        public Vector3 Vector
        {
            get { return (Vector3) GetValue(VectorProperty); }
            set { SetValue(VectorProperty, value); }
        }

        public float X
        {
            get { return Vector.X; }
            set { Vector = new Vector3(value, Vector.Y, Vector.Z); }
        }

        public float Y
        {
            get { return Vector.Y; }
            set { Vector = new Vector3(Vector.X, value, Vector.Z); }
        }

        public float Z
        {
            get { return Vector.Z; }
            set { Vector = new Vector3(Vector.Z, Vector.Y, value); }
        }
    }
}

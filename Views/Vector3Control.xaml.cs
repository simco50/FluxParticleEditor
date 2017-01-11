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
using ParticleEditor.Model.Data;
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
            Value = new CustomVector3();
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(CustomVector3), typeof(Vector3Control), new PropertyMetadata(default(CustomVector3)));

        public CustomVector3 Value
        {
            get { return (CustomVector3) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}

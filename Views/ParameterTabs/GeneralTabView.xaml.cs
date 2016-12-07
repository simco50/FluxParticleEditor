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
using ParticleEditor.Data.ParticleSystem;

namespace ParticleEditor.Views.ParameterTabs
{
    /// <summary>
    /// Interaction logic for GeneralTabView.xaml
    /// </summary>
    public partial class GeneralTabView : UserControl
    {
        public GeneralTabView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ParticleSystemProperty = DependencyProperty.Register(
            "ParticleSystem", typeof(ParticleSystem), typeof(GeneralTabView), new PropertyMetadata(default(ParticleSystem)));

        public ParticleSystem ParticleSystem
        {
            get { return (ParticleSystem) GetValue(ParticleSystemProperty); }
            set { SetValue(ParticleSystemProperty, value); }
        }
    }
}

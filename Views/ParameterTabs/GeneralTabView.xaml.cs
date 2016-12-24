using System.Windows;
using System.Windows.Controls;
using ParticleEditor.Model.Data;

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

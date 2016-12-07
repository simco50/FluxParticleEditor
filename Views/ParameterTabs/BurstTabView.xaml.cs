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
using ParticleEditor.ViewModels.ParameterTabs;

namespace ParticleEditor.Views.ParameterTabs
{
    /// <summary>
    /// Interaction logic for BurstTabView.xaml
    /// </summary>
    public partial class BurstTabView : UserControl
    {
        private BurstTabViewModel _vm;

        public BurstTabView()
        {
            InitializeComponent();
            _vm = Root.DataContext as BurstTabViewModel;
        }

        public static readonly DependencyProperty ParticleSystemProperty = DependencyProperty.Register(
            "ParticleSystem", typeof(ParticleSystem), typeof(BurstTabView), new FrameworkPropertyMetadata(OnParticleSystemChanged));

        private static void OnParticleSystemChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as BurstTabView)._vm.ParticleSystem = args.NewValue as ParticleSystem;
        }

        public ParticleSystem ParticleSystem
        {
            get { return (ParticleSystem) GetValue(ParticleSystemProperty); }
            set { SetValue(ParticleSystemProperty, value); }
        }
    }
}

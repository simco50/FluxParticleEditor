using System.Windows;
using System.Windows.Controls;
using ParticleEditor.Model.Data;
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

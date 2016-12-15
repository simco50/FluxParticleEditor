using System.Windows;
using System.Windows.Controls;
using ParticleEditor.Model.Data;
using ParticleEditor.ViewModels;

namespace ParticleEditor.Views
{
    /// <summary>
    /// Interaction logic for ParticleVisualizerView.xaml
    /// </summary>
    public partial class ParticleVisualizerView : UserControl
    {
        private ParticleVisualizerViewModel _vm;

        public ParticleVisualizerView()
        {
            InitializeComponent();
            _vm = Root.DataContext as ParticleVisualizerViewModel;
        }

        public static readonly DependencyProperty ParticleSystemProperty = DependencyProperty.Register(
            "ParticleSystem", typeof(ParticleSystem), typeof(ParticleVisualizerView), new FrameworkPropertyMetadata(OnParticleSystemChanged));

        private static void OnParticleSystemChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as ParticleVisualizerView)._vm.Viewport.ParticleSystem = args.NewValue as ParticleSystem;
        }

        public ParticleSystem ParticleSystem
        {
            get { return (ParticleSystem)GetValue(ParticleSystemProperty); }
            set { SetValue(ParticleSystemProperty, value); }
        }
    }
}

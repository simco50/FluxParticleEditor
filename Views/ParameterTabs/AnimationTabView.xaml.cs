using System.Windows;
using System.Windows.Controls;
using ParticleEditor.Model.Data;
using ParticleEditor.ViewModels.ParameterTabs;

namespace ParticleEditor.Views.ParameterTabs
{
    /// <summary>
    /// Interaction logic for AnimationTabView.xaml
    /// </summary>
    public partial class AnimationTabView : UserControl
    {
        private AnimationTabViewModel _vm;

        public AnimationTabView()
        {
            InitializeComponent();
            _vm = Root.DataContext as AnimationTabViewModel;
        }

        public static readonly DependencyProperty ParticleSystemProperty = DependencyProperty.Register(
            "ParticleSystem", typeof(ParticleSystem), typeof(AnimationTabView), new FrameworkPropertyMetadata(OnParticleSystemChanged));

        private static void OnParticleSystemChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as AnimationTabView)._vm.ParticleSystem = args.NewValue as ParticleSystem;
        }

        public ParticleSystem ParticleSystem
        {
            get { return (ParticleSystem)GetValue(ParticleSystemProperty); }
            set { SetValue(ParticleSystemProperty, value); }
        }
    }
}

using System.Windows.Controls;
using ParticleEditor.Model.Data;
using ParticleEditor.ViewModels.AnimationControls;

namespace ParticleEditor.Views.AnimationControls
{
    /// <summary>
    /// Interaction logic for FloatKeyframeView.xaml
    /// </summary>
    public partial class FloatKeyframeView : UserControl
    {
        public FloatKeyframeView(KeyFramedValueFloat value)
        {
            InitializeComponent();
            FloatKeyframeViewModel vm = new FloatKeyframeViewModel();
            vm.Value = value;
            Grid.DataContext = vm;
        }
    }
}

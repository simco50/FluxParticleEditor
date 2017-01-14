using System.Windows.Controls;
using ParticleEditor.Model.Data;
using ParticleEditor.ViewModels.AnimationControls;

namespace ParticleEditor.Views.AnimationControls
{
    /// <summary>
    /// Interaction logic for VectorKeyframeView.xaml
    /// </summary>
    public partial class VectorKeyframeView : UserControl
    {
        public VectorKeyframeView(KeyFramedValueVector3 value, string name = "")
        {
            InitializeComponent();
            VectorKeyframeViewModel vm = new VectorKeyframeViewModel();
            vm.Value = value;
            Grid.DataContext = vm;
            Label_Name.Content = $"{name.ToUpper()} (Vector3)";
        }
    }
}

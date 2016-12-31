using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Model.Data;
using SharpDX;

namespace ParticleEditor.ViewModels.ParameterTabs
{
    public class AnimationTabViewModel : ViewModelBase
    {
        public ParticleSystem ParticleSystem { get; set; }

        public RelayCommand SelectColorCommand => new RelayCommand(SelectColor);
        private void SelectColor()
        {
            ColorDialog dialog = new ColorDialog();
            dialog.SolidColorOnly = true;
            dialog.AnyColor = true;
            dialog.FullOpen = true;
            dialog.Color = System.Drawing.Color.FromArgb((int)(ParticleSystem.Color.Constant.X * 255), (int)(ParticleSystem.Color.Constant.Y * 255), (int)(ParticleSystem.Color.Constant.Z * 255));
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            ParticleSystem.Color.SetConstant(new Vector3(dialog.Color.R / 255.0f, dialog.Color.G / 255.0f,
                dialog.Color.B / 255.0f));

            ParticleSystem.RaisePropertyChanged("Color");
        }
    }
}

using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Model.Data;

namespace ParticleEditor.ViewModels.ParameterTabs
{
    class GeneralTabViewModel : ViewModelBase
    {
        public bool EmitFromShellEnabled
        {
            get
            {
                return (MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.CIRCLE ||
                        MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.CONE ||
                        MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.SPHERE);
            }
        }

        public bool EmitFromVolumeEnabled
        {
            get
            {
                return (MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.CONE ||
                        MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.SPHERE);
            }
        }

        public bool AngleEnabled
        {
            get { return MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.CONE; }
        }

        public RelayCommand OnShapeChangedCommand => new RelayCommand(() =>
        {
            RaisePropertyChanged("EmitFromShellEnabled");
            RaisePropertyChanged("EmitFromVolumeEnabled");
            RaisePropertyChanged("AngleEnabled");
        });
    }
}

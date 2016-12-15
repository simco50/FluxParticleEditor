using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using DirectxWpf;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Graphics.ImageControl;

namespace ParticleEditor.ViewModels
{
    class ParticleVisualizerViewModel
    {
        public ParticleVisualizerViewModel()
        {
            Viewport = new ParticleViewport();
        }

        public ParticleViewport Viewport { get; set; }

        public RelayCommand<MouseWheelEventArgs> OnScroll
            => new RelayCommand<MouseWheelEventArgs>((args) => ParticleViewport.Camera.Zoom += (float) args.Delta / 500.0f);

        public RelayCommand OnMouseDown => new RelayCommand(()=> ParticleViewport.Camera.MouseDown = true);
        public RelayCommand OnMouseUp => new RelayCommand(()=> ParticleViewport.Camera.MouseDown = false);
        public RelayCommand OnMouseLeave => new RelayCommand(()=> ParticleViewport.Camera.MouseDown = false);

        public RelayCommand ZoomInCommand => new RelayCommand(() => ParticleViewport.Camera.Zoom += 0.2f);
        public RelayCommand ZoomOutCommand => new RelayCommand(() => ParticleViewport.Camera.Zoom -= 0.2f);
        public RelayCommand ResetCommand => new RelayCommand(() => ParticleViewport.Camera.Reset());
    }
}

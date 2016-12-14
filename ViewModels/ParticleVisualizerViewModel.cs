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
        public ParticleViewport Viewport { get; set; } = new ParticleViewport();

        public RelayCommand<MouseWheelEventArgs> OnScroll { get { return new RelayCommand<MouseWheelEventArgs>(OnMouseWheel);} }
        private void OnMouseWheel(MouseWheelEventArgs args)
        {
            Viewport.Camera.Distance -= (float)args.Delta / 500.0f;
            if (Viewport.Camera.Distance < 0)
                Viewport.Camera.Distance = 0;
            else if (Viewport.Camera.Distance > 1)
                Viewport.Camera.Distance = 1;
        }
        public RelayCommand OnMouseDown => new RelayCommand(()=>Viewport.Camera.MouseDown = true);
        public RelayCommand OnMouseUp => new RelayCommand(()=>Viewport.Camera.MouseDown = false);
        public RelayCommand OnMouseLeave => new RelayCommand(()=>Viewport.Camera.MouseDown = false);
    }
}

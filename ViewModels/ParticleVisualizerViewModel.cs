using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using DirectxWpf;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Graphics.ImageControl;
using SharpDX;

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
            =>
            new RelayCommand<MouseWheelEventArgs>((args) => ((OrbitCamera)Viewport.GraphicsContext.Camera).Zoom += (float)args.Delta / 500.0f)
            ;

        public RelayCommand<MouseEventArgs> OnMouseDown => new RelayCommand<MouseEventArgs>((args) =>
        {
            if (args.LeftButton == MouseButtonState.Pressed)
                ((OrbitCamera)Viewport.GraphicsContext.Camera).LeftMouseDown = true;
            if (args.MiddleButton == MouseButtonState.Pressed)
                ((OrbitCamera)Viewport.GraphicsContext.Camera).MiddleMouseDown = true;
        });

        public RelayCommand<MouseEventArgs> OnMouseUp => new RelayCommand<MouseEventArgs>((args) =>
        {
            if (args.LeftButton == MouseButtonState.Released)
                ((OrbitCamera)Viewport.GraphicsContext.Camera).LeftMouseDown = false;
            if (args.MiddleButton == MouseButtonState.Released)
                ((OrbitCamera)Viewport.GraphicsContext.Camera).MiddleMouseDown = false;
        });

        public RelayCommand OnMouseLeave => new RelayCommand(() =>
        {
            ((OrbitCamera)Viewport.GraphicsContext.Camera).LeftMouseDown = false;
            ((OrbitCamera)Viewport.GraphicsContext.Camera).MiddleMouseDown = false;
        });

        public RelayCommand ZoomInCommand => new RelayCommand(() => ((OrbitCamera)Viewport.GraphicsContext.Camera).Zoom += 0.2f);
        public RelayCommand ZoomOutCommand => new RelayCommand(() => ((OrbitCamera)Viewport.GraphicsContext.Camera).Zoom -= 0.2f);
        public RelayCommand ResetCommand => new RelayCommand(() => ((OrbitCamera)Viewport.GraphicsContext.Camera).Reset());
    }
}

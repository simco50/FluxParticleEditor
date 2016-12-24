using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Model.Graphics.Particles;

namespace ParticleEditor.ViewModels
{
    class ParticleVisualizerViewModel
    {
        public ParticleVisualizerViewModel()
        {
            Viewport = new ParticleViewport();
        }

        public ParticleViewport Viewport { get; set; }

        public RelayCommand<MouseWheelEventArgs> OnScroll =>
            new RelayCommand<MouseWheelEventArgs>(
                (args) => Viewport.GraphicsContext.Camera.Zoom += (float) args.Delta / 500.0f);

        public RelayCommand<MouseEventArgs> OnMouseDown => new RelayCommand<MouseEventArgs>((args) =>
        {
            if (args.LeftButton == MouseButtonState.Pressed)
                Viewport.GraphicsContext.Camera.LeftMouseDown = true;
            if (args.MiddleButton == MouseButtonState.Pressed)
                Viewport.GraphicsContext.Camera.MiddleMouseDown = true;
        });

        public RelayCommand<MouseEventArgs> OnMouseUp => new RelayCommand<MouseEventArgs>((args) =>
        {
            if (args.LeftButton == MouseButtonState.Released)
                Viewport.GraphicsContext.Camera.LeftMouseDown = false;
            if (args.MiddleButton == MouseButtonState.Released)
                Viewport.GraphicsContext.Camera.MiddleMouseDown = false;
        });

        public RelayCommand OnMouseLeave => new RelayCommand(() =>
        {
            Viewport.GraphicsContext.Camera.LeftMouseDown = false;
            Viewport.GraphicsContext.Camera.MiddleMouseDown = false;
        });

        public RelayCommand ZoomInCommand => new RelayCommand(() => Viewport.GraphicsContext.Camera.Zoom += 0.2f);
        public RelayCommand ZoomOutCommand => new RelayCommand(() => Viewport.GraphicsContext.Camera.Zoom -= 0.2f);
        public RelayCommand ResetCommand => new RelayCommand(() => Viewport.GraphicsContext.Camera.Reset());
    }
}

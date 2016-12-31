using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Annotations;
using ParticleEditor.Model.Graphics.Particles;

namespace ParticleEditor.ViewModels
{
    class ParticleVisualizerViewModel : INotifyPropertyChanged
    {
        public ParticleVisualizerViewModel()
        {
            Viewport = new ParticleViewport();
            _updateTimer = new Timer(0.016f);
            _updateTimer.Elapsed += (sender, args) => OnPropertyChanged("Timer");
            _updateTimer.Start();
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

        private Timer _updateTimer;
        public string Timer
        {
            get
            {
                if (Viewport.ParticleEmitter == null) return "";
                return $"{Viewport.ParticleEmitter.PlayTimer:00.00}/{MainViewModel.MainParticleSystem.Duration:00.00}";
            }
        }

        public RelayCommand PlayCommand => new RelayCommand(()=> Viewport.ParticleEmitter?.Play());
        public RelayCommand StopCommand => new RelayCommand(()=> Viewport.ParticleEmitter?.Stop());
        public RelayCommand PauseCommand => new RelayCommand(()=> Viewport.ParticleEmitter?.Pause());

        public RelayCommand ZoomInCommand => new RelayCommand(() => Viewport.GraphicsContext.Camera.Zoom += 0.2f);
        public RelayCommand ZoomOutCommand => new RelayCommand(() => Viewport.GraphicsContext.Camera.Zoom -= 0.2f);
        public RelayCommand ResetCommand => new RelayCommand(() => Viewport.GraphicsContext.Camera.Reset());
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

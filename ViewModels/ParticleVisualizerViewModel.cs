using System;
using System.Timers;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ParticleEditor.Helpers;
using ParticleEditor.Model.Graphics.Particles;
using SharpDX;

namespace ParticleEditor.ViewModels
{
    class ParticleVisualizerViewModel : ViewModelBase
    {
        public ParticleVisualizerViewModel()
        {
            Messenger.Default.Register<MessageData>(this, OnMessageReceived);

            Viewport = new ParticleViewport();
            Timer updateTimer = new Timer(0.016f);
            updateTimer.Elapsed += (sender, args) => RaisePropertyChanged("Timer");
            updateTimer.Elapsed += (sender, args) => RaisePropertyChanged("CameraZoom");
            updateTimer.Start();
        }

        private void OnMessageReceived(MessageData message)
        {
            switch (message.Id)
            {
                case MessageId.ParticleSystemChanged:
                    Viewport.ParticleEmitter?.OnParticleSystemChanged();
                    break;
                case MessageId.ImageChanged:
                    Viewport.ParticleEmitter?.OnImageChanged();
                    break;
                case MessageId.BurstChanged:
                    Viewport.ParticleEmitter?.Reset();
                    break;
                case MessageId.ColorChanged:
                    Viewport.GraphicsContext.RenderControl.ClearColor = (Color)message.Data;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(message.Id), message.Id, null);
            }
        }

        public float CameraZoom
        {
            get
            {
                if(Viewport.GraphicsContext.Camera != null)
                    return Viewport.GraphicsContext.Camera.Zoom;
                return 0;
            }
            set
            {
                Viewport.GraphicsContext.Camera.Zoom = value;
                RaisePropertyChanged("CameraZoom");
            }
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
    }
}

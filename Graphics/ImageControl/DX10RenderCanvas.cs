
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;
    using SharpDX;
    using SharpDX.Direct3D10;
    using SharpDX.DXGI;
    using Device = SharpDX.Direct3D10.Device1;
    using Image = System.Windows.Controls.Image;

namespace ParticleEditor.Graphics.ImageControl
{
    public class DX10RenderCanvas : Image
    {
        public static readonly DependencyProperty ViewportProperty = DependencyProperty.Register(
            "Viewport", typeof(IDX10Viewport), typeof(DX10RenderCanvas), new PropertyMetadata(default(IDX10Viewport)));

        public IDX10Viewport Viewport
        {
            get { return (IDX10Viewport) GetValue(ViewportProperty); }

            set
            {
                if (ReferenceEquals(Viewport, value))
                    return;

                Viewport?.Deinitialize();
                _sceneAttached = false;

                SetValue(ViewportProperty, value);
            }
        }

        public static readonly DependencyProperty ClearColorProperty = DependencyProperty.Register(
            "ClearColor", typeof(Color4), typeof(DX10RenderCanvas), new PropertyMetadata(default(Color4)));

        public Color4 ClearColor
        {
            get { return (Color4) GetValue(ClearColorProperty); }
            set { SetValue(ClearColorProperty, value); }
        }

        private Device _device;
        private Texture2D _renderTarget;
        private Texture2D _depthStencil;
        private RenderTargetView _renderTargetView;
        private DepthStencilView _depthStencilView;
        private DX10ImageSource _d3DSurface;
        private Stopwatch _renderTimer;
        
        private bool _sceneAttached;
        private float _lastUpdate;
        private DateTime? _lastSizeChange;  

        public DX10RenderCanvas()
        {
            _renderTimer = new Stopwatch();
            Loaded += Window_Loaded;
            Unloaded += Window_Closing;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DX10RenderCanvas.IsInDesignMode)
                return;

            StartD3D();
            StartRendering();
        }

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            if (DX10RenderCanvas.IsInDesignMode)
                return;

            StopRendering();
            EndD3D();
        }

        private void StartD3D()
        {
            _device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_0);

            _d3DSurface = new DX10ImageSource();
            _d3DSurface.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

            CreateAndBindTargets();

            Source = _d3DSurface;
        }

        private void EndD3D()
        {
            if (Viewport != null)
            {
                Viewport.Deinitialize();
                _sceneAttached = false;
            }

            _d3DSurface.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;
            Source = null;

            Disposer.RemoveAndDispose(ref _d3DSurface);
            Disposer.RemoveAndDispose(ref _renderTargetView);
            Disposer.RemoveAndDispose(ref _depthStencilView);
            Disposer.RemoveAndDispose(ref _renderTarget);
            Disposer.RemoveAndDispose(ref _depthStencil);
            Disposer.RemoveAndDispose(ref _device);
        }

        private void CreateAndBindTargets()
        {
            _d3DSurface.SetRenderTargetDX10(null);

            Disposer.RemoveAndDispose(ref _renderTargetView);
            Disposer.RemoveAndDispose(ref _depthStencilView);
            Disposer.RemoveAndDispose(ref _renderTarget);
            Disposer.RemoveAndDispose(ref _depthStencil);

            int width = Math.Max((int)base.ActualWidth, 100);
            int height = Math.Max((int)base.ActualHeight, 100);

            Texture2DDescription colordesc = new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.Shared,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1
            };

            Texture2DDescription depthdesc = new Texture2DDescription
            {
                BindFlags = BindFlags.DepthStencil,
                Format = Format.D32_Float_S8X24_UInt,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.None,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1,
            };

            _renderTarget = new Texture2D(_device, colordesc);
            _depthStencil = new Texture2D(_device, depthdesc);
            _renderTargetView = new RenderTargetView(_device, _renderTarget);
            _depthStencilView = new DepthStencilView(_device, _depthStencil);

            _d3DSurface.SetRenderTargetDX10(_renderTarget);
        }

        private void StartRendering()
        {
            if (_renderTimer.IsRunning)
                return;

            CompositionTarget.Rendering += OnRendering;
            _renderTimer.Start();
        }

        private void StopRendering()
        {
            if (!_renderTimer.IsRunning)
                return;

            CompositionTarget.Rendering -= OnRendering;
            _renderTimer.Stop();
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (!_renderTimer.IsRunning)
                return;

            var currSeconds = (float)_renderTimer.Elapsed.TotalSeconds;
            Render(currSeconds - _lastUpdate);
            _d3DSurface.InvalidateD3DImage();
            _lastUpdate = currSeconds;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            _lastSizeChange = DateTime.Now.AddMilliseconds(200);

            //CreateAndBindTargets();
            base.OnRenderSizeChanged(sizeInfo);
        }

        void Render(float deltaT)
        {
            if (_lastSizeChange.HasValue && _lastSizeChange.Value <= DateTime.Now)
            {
                _lastSizeChange = null;
                CreateAndBindTargets();
            }

            Device device = _device;
            if (device == null)
                return;

            Texture2D renderTarget = _renderTarget;
            if (renderTarget == null)
                return;

            int targetWidth = renderTarget.Description.Width;
            int targetHeight = renderTarget.Description.Height;

            device.OutputMerger.SetTargets(_depthStencilView, _renderTargetView);
            device.Rasterizer.SetViewports(new Viewport(0, 0, targetWidth, targetHeight, 0.0f, 1.0f));

            device.ClearRenderTargetView(_renderTargetView, ClearColor);
            device.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);

            if (Viewport != null)
            {
                if (!_sceneAttached)
                {
                    _sceneAttached = true;
                    Viewport.Initialize(_device, _renderTargetView, this);
                }

                Viewport.Update(deltaT);
                Viewport.Render(deltaT);
            }

            device.Flush();
        }

        private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // this fires when the screensaver kicks in, the machine goes into sleep or hibernate
            // and any other catastrophic losses of the d3d device from WPF's point of view
            if (_d3DSurface.IsFrontBufferAvailable)
                StartRendering();
            else
                StopRendering();
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                DependencyProperty prop = DesignerProperties.IsInDesignModeProperty;
                bool isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                return isDesignMode;
            }
        }
    }
}

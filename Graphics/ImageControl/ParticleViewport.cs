using System.Windows;
using DirectxWpf.Effects;
using DirectxWpf.Models;
using ParticleEditor.Data.ParticleSystem;
using ParticleEditor.Graphics;
using SharpDX;
using SharpDX.Direct3D10;
using Format = SharpDX.DXGI.Format;

namespace ParticleEditor.Graphics.ImageControl
{
    public class ParticleViewport : IDX10Viewport
    {
        private Device1 _device;
        private RenderTargetView _renderTargetView;
        private DX10RenderCanvas _renderControl;

        public ParticleEmitter ParticleEmitter { get; set; }

        public static OrbitCamera Camera { get; set; }

        public bool RenderGrid { get; set; } = true;

        private Grid _grid;

        public void Initialize(Device1 device, RenderTargetView renderTarget, DX10RenderCanvas canvasControl)
        {
            _device = device;
            _renderTargetView = renderTarget;
            _renderControl = canvasControl;

            ParticleEmitter = new ParticleEmitter(_device);
            ParticleEmitter.ParticleSystem = new ParticleSystem();
            ParticleEmitter.Intialize();

            _renderControl.ClearColor = Color.BlanchedAlmond;
            Camera = new OrbitCamera(canvasControl);

            _grid = new Grid();
            _grid.Initialize(_device);
        }

        public void Deinitialize()
        {
            
        }

        public void Update(float deltaT)
        {
            ParticleEmitter.Update(deltaT);
            Camera.Update(deltaT);

            ParticleEmitter.ViewInvVar.SetMatrix(Camera.ViewInverseMatrix);
            ParticleEmitter.ViewProjVar.SetMatrix(Camera.ViewProjectionMatrix);
        }

        public void Render(float deltaT)
        {
            if (_device == null)
                return;

            ParticleEmitter.Render();

            if(RenderGrid)
                _grid.Render();
        }
    }
}

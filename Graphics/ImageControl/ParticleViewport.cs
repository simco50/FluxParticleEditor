using System.Windows;
using DirectxWpf.Effects;
using DirectxWpf.Models;
using ParticleEditor.Data.ParticleSystem;
using ParticleEditor.Graphics;
using ParticleEditor.Helpers;
using SharpDX;
using SharpDX.Direct3D10;
using Format = SharpDX.DXGI.Format;

namespace ParticleEditor.Graphics.ImageControl
{
    public class ParticleViewport : IDX10Viewport
    {
        public GraphicsContext GraphicsContext { get; set; }
        public ParticleEmitter ParticleEmitter { get; set; }
        public bool RenderGrid { get; set; } = true;
        private Grid _grid;

        public void Initialize(Device1 device, RenderTargetView renderTarget, DX10RenderCanvas canvasControl)
        {
            GraphicsContext = new GraphicsContext();
            GraphicsContext.Device = device;
            GraphicsContext.RenderTargetView = renderTarget;
            GraphicsContext.RenderControl = canvasControl;
            OrbitCamera camera = new OrbitCamera(canvasControl);
            camera.ResetAngles = new Vector3(-MathUtil.PiOverFour, -MathUtil.PiOverFour, 0);
            camera.Reset();
            GraphicsContext.Camera = camera;

            ParticleEmitter = new ParticleEmitter(GraphicsContext);
            ParticleEmitter.ParticleSystem = new ParticleSystem();
            ParticleEmitter.Intialize();

            _grid = new Grid();
            _grid.Initialize(GraphicsContext);

            DebugLog.Log("Initialized", "Direct3D");
        }

        public void Deinitialize()
        {
            DebugLog.Log("Shutdown", "Direct3D");
        }

        public void Update(float deltaT)
        {
            ParticleEmitter.Update(deltaT);
            GraphicsContext.Camera.Update(deltaT);
        }

        public void Render(float deltaT)
        {
            if (GraphicsContext.Device == null)
                return;

            ParticleEmitter.Render();

            if(RenderGrid)
                _grid.Render();
        }
    }
}

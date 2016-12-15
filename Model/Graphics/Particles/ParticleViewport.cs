using ParticleEditor.Helpers;
using ParticleEditor.Model.Data;
using ParticleEditor.Model.ImageControl;
using SharpDX;
using SharpDX.Direct3D10;

namespace ParticleEditor.Model.Graphics.Particles
{
    public class ParticleViewport : IDX10Viewport
    {
        public GraphicsContext GraphicsContext { get; set; } = new GraphicsContext();
        public ParticleEmitter ParticleEmitter { get; set; } = null;
        public ParticleSystem ParticleSystem { get; set; } = null;
        public bool RenderGrid { get; set; } = true;
        private Grid _grid;

        public void Initialize(Device1 device, RenderTargetView renderTarget, DX10RenderCanvas canvasControl)
        {
            GraphicsContext.Device = device;
            GraphicsContext.RenderTargetView = renderTarget;
            GraphicsContext.RenderControl = canvasControl;
            OrbitCamera camera = new OrbitCamera(canvasControl);
            camera.ResetAngles = new Vector3(-MathUtil.PiOverFour, -MathUtil.PiOverFour, 0);
            camera.Reset();
            GraphicsContext.Camera = camera;

            ParticleEmitter = new ParticleEmitter(GraphicsContext);
            ParticleEmitter.ParticleSystem = ParticleSystem;
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

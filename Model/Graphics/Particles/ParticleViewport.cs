using ParticleEditor.Helpers;
using ParticleEditor.Model.Data;
using ParticleEditor.Model.ImageControl;
using ParticleEditor.ViewModels;
using SharpDX;
using SharpDX.Direct3D10;

namespace ParticleEditor.Model.Graphics.Particles
{
    public class ParticleViewport : IDX10Viewport
    {
        public GraphicsContext GraphicsContext { get; set; } = new GraphicsContext();
        public ParticleEmitter ParticleEmitter { get; set; } = null;

        public bool RenderGrid { get; set; } = true;
        private Grid _grid;

        public void Initialize(Device1 device, RenderTargetView renderTarget, DX10RenderCanvas canvasControl)
        {
            //Create the graphics context
            GraphicsContext.Device = device;
            GraphicsContext.RenderTargetView = renderTarget;
            GraphicsContext.RenderControl = canvasControl;
            OrbitCamera camera = new OrbitCamera(canvasControl);
            camera.ResetAngles = new Vector3(-MathUtil.PiOverFour, -MathUtil.PiOverFour, 0);
            camera.Reset();
            GraphicsContext.Camera = camera;

            //Grid
            _grid = new Grid();
            _grid.Initialize(GraphicsContext);

            ParticleEmitter = new ParticleEmitter(GraphicsContext);
            ParticleEmitter.Intialize();

            DebugLog.Log("Initialized", "Direct3D");
        }

        public void Deinitialize()
        {
            DebugLog.Log("Shutdown", "Direct3D");
        }

        public void Update(float deltaT)
        {
            GraphicsContext.RenderControl.ClearColor = MainViewModel.BackgroundColorDx;
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

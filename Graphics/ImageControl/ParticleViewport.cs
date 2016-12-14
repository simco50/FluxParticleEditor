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

        public void Initialize(Device1 device, RenderTargetView renderTarget, DX10RenderCanvas canvasControl)
        {
            _device = device;
            _renderTargetView = renderTarget;
            _renderControl = canvasControl;

            ParticleEmitter = new ParticleEmitter(_device);
            ParticleEmitter.ParticleSystem = new ParticleSystem();
            ParticleEmitter.Intialize();

            _renderControl.ClearColor = Color.BlanchedAlmond;
        }

        public void Deinitialize()
        {
            
        }

        public void Update(float deltaT)
        {
            ParticleEmitter.Update(deltaT);

            var viewMat = Matrix.LookAtLH(new Vector3(0, 0, -10), Vector3.Zero, Vector3.UnitY);
            var projMat = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, (float)_renderControl.ActualWidth / (float)_renderControl.ActualHeight, 0.1f, 1000f);
            ParticleEmitter.ViewInvVar.SetMatrix(Matrix.Invert(viewMat));
            ParticleEmitter.ViewProjVar.SetMatrix(viewMat * projMat);
        }

        public void Render(float deltaT)
        {
            if (_device == null)
                return;

            ParticleEmitter.Render();
        }
    }
}

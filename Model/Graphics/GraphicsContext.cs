using ParticleEditor.Model.ImageControl;
using SharpDX.Direct3D10;

namespace ParticleEditor.Model.Graphics
{
    public class GraphicsContext
    {
        public DX10RenderCanvas RenderControl { get; set; }
        public Device1 Device { get; set; }
        public OrbitCamera Camera { get; set; }
        public RenderTargetView RenderTargetView { get; set; }
    }
}
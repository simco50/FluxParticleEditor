using System;
using SharpDX.Direct3D10;
using Device = SharpDX.Direct3D10.Device1;

namespace ParticleEditor.Graphics.ImageControl
{
    public interface IDX10Viewport
    {
        void Initialize(Device device, RenderTargetView renderTarget, DX10RenderCanvas canvasControl);
        void Deinitialize();
        void Update(float deltaT);
        void Render(float deltaT);
    }
}

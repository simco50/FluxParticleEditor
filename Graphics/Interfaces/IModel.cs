using SharpDX.Direct3D;
using SharpDX.Direct3D10;
using Buffer = SharpDX.Direct3D10.Buffer;

namespace DirectxWpf.Models
{
    public interface IModel
    {
        PrimitiveTopology PrimitiveTopology { get; set; }
        int VertexStride { get; set; }
        int IndexCount { get; set; }
        Buffer IndexBuffer { get; set; }
        Buffer VertexBuffer { get; set; }

        void Create(Device1 device);
    }
}

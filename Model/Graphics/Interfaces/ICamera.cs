using SharpDX;

namespace ParticleEditor.Model.Graphics.Interfaces
{
    public interface ICamera
    {
        Matrix ViewMatrix { get; set; }
        Matrix ProjectionMatrix { get; set; }
        Matrix ViewInverseMatrix { get; set; }
        Matrix ViewProjectionMatrix { get; set; }
        Vector3 Position { get; }
        void Update(float deltaTime);
    }
}

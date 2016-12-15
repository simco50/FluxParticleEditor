using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace ParticleEditor.Graphics.ImageControl
{
    public interface ICamera
    {
        Matrix ViewMatrix { get; set; }
        Matrix ProjectionMatrix { get; set; }
        Matrix ViewInverseMatrix { get; set; }
        Matrix ViewProjectionMatrix { get; set; }
        void Update(float deltaTime);
    }
}

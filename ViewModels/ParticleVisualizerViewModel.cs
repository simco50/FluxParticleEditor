using System.Windows;
using DirectxWpf;
using ParticleEditor.Graphics.ImageControl;

namespace ParticleEditor.ViewModels
{
    class ParticleVisualizerViewModel
    {
        public IDX10Viewport Viewport { get; set; } = new ParticleViewport();
    }
}

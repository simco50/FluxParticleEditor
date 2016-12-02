using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEditor.Data
{
    public class ParticleEmitter
    {
        private ParticleSystem _particleSystem = new ParticleSystem();
        private bool _playing = false;
        private float _timer = 0.0f;
        private List<Particle> _particles = new List<Particle>();
        private int _particleCount = 0;
        private int _bufferSize = 0;
        private float _particleSpawnTimer = 0.0f;


        public void Initialize()
        {
            if (_particleSystem.PlayOnAwake)
                Play();
        }

        public void Update()
        {
            
        }

        public void Render()
        {

        }

        public void Stop()
        {
            _playing = false;
        }

        public void Play()
        {
            _playing = true;
        }

        public void Reset()
        {
            _playing = false;
            foreach (Particle p in _particles)
                p.Reset();
        }
    }
}

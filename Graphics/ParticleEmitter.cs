using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DrWPF.Windows.Data;
using ParticleEditor.Data.ParticleSystem;
using ParticleEditor.Graphics.ImageControl;
using ParticleEditor.Helpers;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D10.Buffer;
using Device1 = SharpDX.Direct3D10.Device1;

namespace ParticleEditor.Graphics
{
    public struct ParticleVertex
    {
        public Vector3 Position;
        public Vector4 Color;
        public float Size;
        public float Rotation;

        public ParticleVertex(Vector3 pos = new Vector3(), Vector4 color = new Vector4(), float size = 0.0f, float rotation = 0.0f)
        {
            Position = pos;
            Color = color;
            Size = size;
            Rotation = rotation;
        }
    }

    public class ParticleEmitter
    {
        private int _vertexStride;
        private int _indexCount;
        private Buffer _vertexBuffer;
        private int _bufferSize = 0;

        //Shader
        private EffectTechnique _technique;
        private Effect _effect;
        private InputLayout _inputLayout;

        //Shader variables
        public EffectMatrixVariable ViewProjVar { get; set; }
        public EffectMatrixVariable ViewInvVar { get; set; }
        public EffectShaderResourceVariable TextureVar { get; set; }

        private Device1 _device;
        private ShaderResourceView _textureResourceView;

        public ParticleSystem ParticleSystem { get; set; }

        //Particle system
        private bool _playing = false;
        private float _timer = 0.0f;
        private float _particleSpawnTimer = 0.0f;
        private int _particleCount = 0;
        private List<Particle> _particles;
        private IEnumerator _burstEnumerator;

        public ParticleEmitter(Device1 device)
        {
            _device = device;
        }

        public void Intialize()
        {
            _particles = new List<Particle>(ParticleSystem.MaxParticles);
            for (int i = 0; i < ParticleSystem.MaxParticles; i++)
                _particles.Add(new Particle(ParticleSystem));
            _bufferSize = ParticleSystem.MaxParticles;

            _textureResourceView = ShaderResourceView.FromFile(_device, "./Resources/ErrorTexture.jpg");

            LoadEffect();

            CreateBuffer();

            _burstEnumerator = ParticleSystem.Bursts.GetEnumerator();
            _burstEnumerator.MoveNext();

            if (ParticleSystem.PlayOnAwake)
                _playing = true;
        }

        private void SortParticles()
        {
            switch (ParticleSystem.SortingMode)
            {
                case ParticleSortingMode.FrontToBack:
                    _particles.Sort((Particle a, Particle b) =>
                    {
                        float d1 = Vector3.DistanceSquared(ParticleViewport.Camera.Position, a.VertexInfo.Position);
                        float d2 = Vector3.DistanceSquared(ParticleViewport.Camera.Position, b.VertexInfo.Position);
                        if (d1 == d2) return 0;
                        return d1 > d2 ? 1 : -1;
                    });
                    break;
                case ParticleSortingMode.BackToFront:
                    _particles.Sort((Particle a, Particle b) =>
                    {
                        float d1 = Vector3.DistanceSquared(ParticleViewport.Camera.Position, a.VertexInfo.Position);
                        float d2 = Vector3.DistanceSquared(ParticleViewport.Camera.Position, b.VertexInfo.Position);
                        if (d1 == d2) return 0;
                        return d1 < d2 ? 1 : -1;
                    });
                    break;
                case ParticleSortingMode.OldestFirst:
                    _particles.Sort((Particle a, Particle b) =>
                    {
                        if (a.Lifetime == b.Lifetime) return 0;
                        return a.Lifetime > b.Lifetime ? 1 : -1;
                    });
                    break;
                case ParticleSortingMode.YoungestFirst:
                    _particles.Sort((Particle a, Particle b) =>
                    {
                        if (a.Lifetime == b.Lifetime) return 0;
                        return a.Lifetime < b.Lifetime ? 1 : -1;
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Update(float deltaTime)
        {
            if (_playing == false)
                return;

            _timer += deltaTime;
            if (_timer >= ParticleSystem.Duration && ParticleSystem.Loop)
            {
                _timer = 0;
                _burstEnumerator = ParticleSystem.Bursts.GetEnumerator();
            }

            float emissionTime = 1.0f / ParticleSystem.Emission;
            _particleSpawnTimer += deltaTime;

            SortParticles();

            if (ParticleSystem.MaxParticles > _particles.Count)
            {
                for (int i = 0; i < ParticleSystem.MaxParticles - _particles.Count; i++)
                    _particles.Add(new Particle(ParticleSystem));
            }

            _particleCount = 0;
            DataStream vertexBufferStream = _vertexBuffer.Map(MapMode.WriteDiscard, SharpDX.Direct3D10.MapFlags.None);

            int burstParticles = 0;
            for (int i = 0; i < ParticleSystem.MaxParticles; i++)
            {
                Particle p = _particles[i];
                if (p.Active)
                {
                    p.Update(deltaTime);
                    vertexBufferStream.Write(p.VertexInfo);
                    ++_particleCount;
                }
                /*else if (_burstEnumerator.MoveNext() &&
                         _timer > (_burstEnumerator.Current as KeyValuePair<float, int>?).Value.Key &&
                         burstParticles < (_burstEnumerator.Current as KeyValuePair<float, int>?).Value.Value)
                {
                    p.Initialize();
                    vertexBufferStream.Write(p.VertexInfo);
                    ++_particleCount;
                    ++burstParticles;
                }*/
                else if (_particleSpawnTimer >= emissionTime && _timer < ParticleSystem.Duration)
                {
                    p.Initialize();
                    vertexBufferStream.Write(p.VertexInfo);
                    ++_particleCount;
                    _particleSpawnTimer -= emissionTime;
                }
            }
            if (burstParticles > 0)
                _burstEnumerator.MoveNext();

            _vertexBuffer.Unmap();
            vertexBufferStream.Dispose();
        }

        public void Render()
        {
            if (_playing == false)
                return;
            if (ParticleSystem.MaxParticles > _bufferSize)
            {
                _bufferSize = ParticleSystem.MaxParticles + 500;
                CreateBuffer();
            }

            TextureVar.SetResource(_textureResourceView);

            _device.InputAssembler.InputLayout = _inputLayout;
            _device.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            _device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, _vertexStride, 0));

            for (int i = 0; i < _technique.Description.PassCount; i++)
            {
                _technique.GetPassByIndex(i).Apply();
                _device.Draw(_particleCount, 0);
            }
        }

        private void LoadEffect()
        {
            //Shader
            CompilationResult result = ShaderBytecode.CompileFromFile("./Resources/ParticleRenderer.fx", "fx_4_0");
            if (result.HasErrors)
            {
                DebugLog.Log(result.Message, "Error compiling shader", LogSeverity.Error);
                return;
            }
            _effect = new Effect(_device, result.Bytecode);
            _technique = _effect.GetTechniqueByIndex(0);

            //Shader variables
            ViewProjVar = _effect.GetVariableBySemantic("VIEWPROJ").AsMatrix();
            ViewInvVar = _effect.GetVariableBySemantic("VIEWINV").AsMatrix();
            TextureVar = _effect.GetVariableByName("gParticleTexture").AsShaderResource();

            //Inputlayout
            InputElement[] vertexLayout =
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCOORD", 0, Format.R32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCOORD", 1, Format.R32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0)
            };
            _inputLayout = new InputLayout(_device, _technique.GetPassByIndex(0).Description.Signature, vertexLayout);
        }

        private void CreateBuffer()
        {
            _vertexStride = Marshal.SizeOf<ParticleVertex>();

            BufferDescription vertexBufferDescription;
            vertexBufferDescription.BindFlags = BindFlags.VertexBuffer;
            vertexBufferDescription.CpuAccessFlags = CpuAccessFlags.Write;
            vertexBufferDescription.OptionFlags = ResourceOptionFlags.None;
            vertexBufferDescription.Usage = ResourceUsage.Dynamic;
            vertexBufferDescription.SizeInBytes = _vertexStride * _bufferSize;
            _vertexBuffer = new Buffer(_device, vertexBufferDescription);
        }
    }
}

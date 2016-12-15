using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ParticleEditor.Helpers;
using ParticleEditor.Model.Data;
using ParticleEditor.Model.ImageControl;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using Buffer = SharpDX.Direct3D10.Buffer;

namespace ParticleEditor.Model.Graphics.Particles
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

        private List<BlendState> BlendState = new List<BlendState>();

        private GraphicsContext _context;
        private ShaderResourceView _textureResourceView;

        private ParticleSystem _particleSystem;

        public ParticleSystem ParticleSystem
        {
            get { return _particleSystem; }
            set
            {
                _particleSystem = value;
                OnParticleSystemSet();
            }
        }

        private bool _hasNext;

        //Particle system
        private bool _playing = false;
        private float _timer = 0.0f;
        private float _particleSpawnTimer = 0.0f;
        private int _particleCount = 0;
        private List<Particle> _particles;
        private IEnumerator<KeyValuePair<float, int>>  _burstEnumerator;

        public ParticleEmitter(GraphicsContext context)
        {
            _context = context;
        }

        public void Intialize()
        {
            LoadEffect();

            DebugLog.Log("Initialized", "Particle Emitter");
        }

        void OnParticleSystemSet()
        {
            _particles = new List<Particle>(ParticleSystem.MaxParticles);
            for (int i = 0; i < ParticleSystem.MaxParticles; i++)
                _particles.Add(new Particle(ParticleSystem));
            _bufferSize = ParticleSystem.MaxParticles;
            ResetBurstEnumerator();
            if (ParticleSystem.PlayOnAwake)
                _playing = true;
            _textureResourceView = ShaderResourceView.FromFile(_context.Device, ParticleSystem.ImagePath);
            CreateBuffer();

            DebugLog.Log("Changed particle system", "Particle Emitter");
        }

        private void ResetBurstEnumerator()
        {
            _burstEnumerator = ParticleSystem.Bursts.GetEnumerator() as IEnumerator<KeyValuePair<float, int>>;
            if (_burstEnumerator == null)
            {
                DebugLog.Log("Converting to IEnumerator<KeyValuePair<float, int>> failed!", "Failed typecast", LogSeverity.Error);
                return;
            }
            _hasNext = _burstEnumerator.MoveNext();
        }

        private void SortParticles()
        {
            switch (ParticleSystem.SortingMode)
            {
                case ParticleSortingMode.FrontToBack:
                    _particles.Sort((Particle a, Particle b) =>
                    {
                        float d1 = Vector3.DistanceSquared(_context.Camera.Position, a.VertexInfo.Position);
                        float d2 = Vector3.DistanceSquared(_context.Camera.Position, b.VertexInfo.Position);
                        if (d1 == d2) return 0;
                        return d1 > d2 ? 1 : -1;
                    });
                    break;
                case ParticleSortingMode.BackToFront:
                    _particles.Sort((Particle a, Particle b) =>
                    {
                        float d1 = Vector3.DistanceSquared(_context.Camera.Position, a.VertexInfo.Position);
                        float d2 = Vector3.DistanceSquared(_context.Camera.Position, b.VertexInfo.Position);
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
            if (ParticleSystem == null)
                return;
            if (_playing == false)
                return;

            _timer += deltaTime;
            if (_timer >= ParticleSystem.Duration && ParticleSystem.Loop)
            {
                _timer = 0;
                ResetBurstEnumerator();
            }

            float emissionTime = 1.0f / ParticleSystem.Emission;
            _particleSpawnTimer += deltaTime;

            SortParticles();

            if (ParticleSystem.MaxParticles > _particles.Count)
            {
                int amount = ParticleSystem.MaxParticles - _particles.Count;
                for (int i = 0; i < amount; i++)
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
                else if (_hasNext && _timer > _burstEnumerator.Current.Key && burstParticles < _burstEnumerator.Current.Value)
                {
                    p.Initialize();
                    vertexBufferStream.Write(p.VertexInfo);
                    ++_particleCount;
                    ++burstParticles;
                }
                else if (_particleSpawnTimer >= emissionTime && _timer < ParticleSystem.Duration)
                {
                    p.Initialize();
                    vertexBufferStream.Write(p.VertexInfo);
                    ++_particleCount;
                    _particleSpawnTimer -= emissionTime;
                }
            }
            if (_hasNext && _timer > _burstEnumerator.Current.Key)
                _hasNext = _burstEnumerator.MoveNext();

            _vertexBuffer.Unmap();
            vertexBufferStream.Dispose();
        }

        public void Render()
        {
            if (ParticleSystem == null)
                return;
            if (_playing == false)
                return;
            if (ParticleSystem.MaxParticles > _bufferSize)
            {
                _bufferSize = ParticleSystem.MaxParticles + 500;
                CreateBuffer();
            }

            TextureVar.SetResource(_textureResourceView);
            ViewInvVar.SetMatrix(_context.Camera.ViewInverseMatrix);
            ViewProjVar.SetMatrix(_context.Camera.ViewProjectionMatrix);

            _context.Device.InputAssembler.InputLayout = _inputLayout;
            _context.Device.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            _context.Device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, _vertexStride, 0));
            _context.Device.OutputMerger.SetBlendState(BlendState[(int) ParticleSystem.BlendMode], new Color4(1.0f), -1);

            for (int i = 0; i < _technique.Description.PassCount; i++)
            {
                _technique.GetPassByIndex(i).Apply();
                _context.Device.Draw(_particleCount, 0);
            }
        }

        private void LoadEffect()
        {
            //Shader
            CompilationResult result = ShaderBytecode.CompileFromFile("./Resources/Shaders/ParticleRenderer.fx", "fx_4_0");
            if (result.HasErrors)
            {
                DebugLog.Log(result.Message, "Error compiling shader", LogSeverity.Error);
                return;
            }
            _effect = new Effect(_context.Device, result.Bytecode);
            _technique = _effect.GetTechniqueByIndex(0);

            //Shader variables
            ViewProjVar = _effect.GetVariableBySemantic("VIEWPROJ").AsMatrix();
            if(ViewProjVar == null)
                DebugLog.Log("Variable with semantic 'VIEWPROJ' not found!", "Particle Emitter", LogSeverity.Error);
            ViewInvVar = _effect.GetVariableBySemantic("VIEWINV").AsMatrix();
            if(ViewInvVar == null)
                DebugLog.Log("Variable with semantic 'VIEWINV' not found!", "Particle Emitter", LogSeverity.Error);
            TextureVar = _effect.GetVariableByName("gParticleTexture").AsShaderResource();
            if(TextureVar == null)
                DebugLog.Log("Variable with name 'gParticleTexture' not found!", "Particle Emitter", LogSeverity.Error);

            //Inputlayout
            InputElement[] vertexLayout =
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCOORD", 0, Format.R32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCOORD", 1, Format.R32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0)
            };
            _inputLayout = new InputLayout(_context.Device, _technique.GetPassByIndex(0).Description.Signature, vertexLayout);

            BlendStateDescription desc = new BlendStateDescription();

            desc.IsBlendEnabled[0] = true;
            desc.BlendOperation = BlendOperation.Add;
            desc.SourceAlphaBlend = BlendOption.One;
            desc.DestinationAlphaBlend = BlendOption.Zero;
            desc.AlphaBlendOperation = BlendOperation.Add;
            desc.RenderTargetWriteMask[0] = ColorWriteMaskFlags.All;

            //AlphaBlending
            desc.SourceBlend = BlendOption.SourceAlpha;
            desc.DestinationBlend = BlendOption.InverseSourceAlpha;
            BlendState.Add(new BlendState(_context.Device, desc));

            //AdditiveBlending
            desc.SourceBlend = BlendOption.SourceAlpha;
            desc.DestinationBlend = BlendOption.One;
            BlendState.Add(new BlendState(_context.Device, desc));

            DebugLog.Log("Shader loaded and blend state created", "Particle Emitter");
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
            _vertexBuffer = new Buffer(_context.Device, vertexBufferDescription);
        }
    }
}

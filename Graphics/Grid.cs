using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    public struct VertexPosCol
    {
        public VertexPosCol(Vector3 pos = new Vector3(), Color color = new Color())
        {
            Position = pos;
            Color = color.ToVector4();
        }

        public Vector3 Position;
        public Vector4 Color;
    }

    public class Grid
    {
        private Buffer _vertexBuffer;
        private InputLayout _inputLayout;
        private Effect _effect;
        private EffectTechnique _technique;

        private List<VertexPosCol> _vertices = new List<VertexPosCol>();

        private EffectMatrixVariable _wvpVar;
        private int _vertexStride;

        private Device1 _device;

        public void Initialize(Device1 device)
        {
            _device = device;
            CreateGrid();
            LoadShader();
            CreateBuffer();
        }

        private void CreateGrid()
        {
            int numGridLines = 20;
            float gridSpacing = 1.0f;

            float startOffset = -((int) numGridLines / 2) * gridSpacing;
            float size = (numGridLines - 1) * gridSpacing;
            Color gridColor = Color.LightGray;
            for (int i = 0; i < numGridLines; ++i)
            {
                //VERTICAL
                float lineOffset = startOffset + gridSpacing * i;
                Vector3 vertStart = new Vector3(startOffset, 0, lineOffset);
                _vertices.Add(new VertexPosCol(vertStart, gridColor));
                vertStart.X += size;
                _vertices.Add(new VertexPosCol(vertStart, gridColor));

                //HORIZONTAL
                vertStart = new Vector3(lineOffset, 0, startOffset);
                _vertices.Add(new VertexPosCol(vertStart, gridColor));
                vertStart.Z += size;
                _vertices.Add(new VertexPosCol(vertStart, gridColor));
            }

            //*AXIS
            _vertices.Add(new VertexPosCol(new Vector3(0, 0.01f, 0), Color.DarkRed));
            _vertices.Add(new VertexPosCol(new Vector3(30, 0.01f, 0), Color.DarkRed));
            _vertices.Add(new VertexPosCol(new Vector3(0, 0.01f, 0), Color.DarkGreen));
            _vertices.Add(new VertexPosCol(new Vector3(0, 30, 0), Color.DarkGreen));
            _vertices.Add(new VertexPosCol(new Vector3(0, 0.01f, 0), Color.DarkBlue));
            _vertices.Add(new VertexPosCol(new Vector3(0, 0.01f, 30), Color.DarkBlue));
        }

        void CreateBuffer()
        {
            _vertexStride = Marshal.SizeOf<VertexPosCol>();
            BufferDescription desc = new BufferDescription();
            desc.BindFlags = BindFlags.VertexBuffer;
            desc.CpuAccessFlags = CpuAccessFlags.None;
            desc.OptionFlags = ResourceOptionFlags.None;
            desc.SizeInBytes = _vertexStride * _vertices.Count;
            desc.Usage = ResourceUsage.Default;
            _vertexBuffer = new Buffer(_device, DataStream.Create(_vertices.ToArray(), false, false), desc);
        }

        private void LoadShader()
        {

            CompilationResult result = ShaderBytecode.CompileFromFile("./Resources/DebugRenderer.fx", "fx_4_0");
            if (result.HasErrors)
            {
                DebugLog.Log(result.Message, "Failed to compile shader", LogSeverity.Error);
                return;
            }
            _effect = new Effect(_device, result.Bytecode);
            _technique = _effect.GetTechniqueByIndex(0);

            _wvpVar = _effect.GetVariableBySemantic("WORLDVIEWPROJECTION").AsMatrix();

            InputElement[] vertexLayout =
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
            };
            EffectPass passDesc = _technique.GetPassByIndex(0);
            _inputLayout = new InputLayout(_device, passDesc.Description.Signature, vertexLayout);

        }

        public void Render()
        {
            _device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, _vertexStride, 0));
            _device.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
            _device.InputAssembler.InputLayout = _inputLayout;

            _wvpVar.SetMatrix(ParticleViewport.Camera.ViewProjectionMatrix);

            for (int i = 0; i < _technique.Description.PassCount; i++)
            {
                _technique.GetPassByIndex(i).Apply();
                _device.Draw(_vertices.Count, 0);
            }
        }
    }
}

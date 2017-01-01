using System.Collections.Generic;
using System.Runtime.InteropServices;
using ParticleEditor.Helpers;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D10.Buffer;

namespace ParticleEditor.Model.Graphics
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
        //Rendering
        private Buffer _vertexBuffer;
        private InputLayout _inputLayout;
        private Effect _effect;
        private EffectTechnique _technique;

        private List<VertexPosCol> _vertices;

        private EffectMatrixVariable _wvpVar;
        private int _vertexStride;

        private GraphicsContext _context;

        public int GridLines { get; set; } = 20;
        public float GridLineSpacing { get; set; } = 0.5f;
        public float AxisLength { get; set; } = 5.0f;
        public Color GridColor { get; set; } = new Color(25, 25, 25);

        public void Initialize(GraphicsContext context)
        {
            _vertexBuffer?.Dispose();

            _context = context;
            LoadShader();
            CreateGrid();
        }

        private void CreateGrid()
        {
            _vertices = new List<VertexPosCol>();

            float startOffset = -((int) GridLines / 2) * GridLineSpacing;
            float size = (GridLines - 1) * GridLineSpacing;

            for (int i = 0; i < GridLines; ++i)
            {
                //VERTICAL
                float lineOffset = startOffset + GridLineSpacing * i;
                Vector3 vertStart = new Vector3(startOffset, 0, lineOffset);
                _vertices.Add(new VertexPosCol(vertStart, GridColor));
                vertStart.X += size;
                _vertices.Add(new VertexPosCol(vertStart, GridColor));

                //HORIZONTAL
                vertStart = new Vector3(lineOffset, 0, startOffset);
                _vertices.Add(new VertexPosCol(vertStart, GridColor));
                vertStart.Z += size;
                _vertices.Add(new VertexPosCol(vertStart, GridColor));
            }

            //*AXIS
            _vertices.Add(new VertexPosCol(new Vector3(0, 0.01f, 0), Color.DarkRed));
            _vertices.Add(new VertexPosCol(new Vector3(AxisLength, 0.01f, 0), Color.DarkRed));
            _vertices.Add(new VertexPosCol(new Vector3(0, 0.01f, 0), Color.DarkGreen));
            _vertices.Add(new VertexPosCol(new Vector3(0, AxisLength, 0), Color.DarkGreen));
            _vertices.Add(new VertexPosCol(new Vector3(0, 0.01f, 0), Color.DarkBlue));
            _vertices.Add(new VertexPosCol(new Vector3(0, 0.01f, AxisLength), Color.DarkBlue));

            _vertexBuffer?.Dispose();

            _vertexStride = Marshal.SizeOf<VertexPosCol>();
            BufferDescription desc = new BufferDescription();
            desc.BindFlags = BindFlags.VertexBuffer;
            desc.CpuAccessFlags = CpuAccessFlags.None;
            desc.OptionFlags = ResourceOptionFlags.None;
            desc.SizeInBytes = _vertexStride * _vertices.Count;
            desc.Usage = ResourceUsage.Default;
            _vertexBuffer = new Buffer(_context.Device, DataStream.Create(_vertices.ToArray(), false, false), desc);
        }

        private void LoadShader()
        {

            CompilationResult result = ShaderBytecode.CompileFromFile("./Resources/Shaders/DebugRenderer.fx", "fx_4_0");
            if (result.HasErrors)
            {
                DebugLog.Log(result.Message, "Failed to compile shader", LogSeverity.Error);
                return;
            }
            _effect = new Effect(_context.Device, result.Bytecode);
            _technique = _effect.GetTechniqueByIndex(0);

            _wvpVar = _effect.GetVariableBySemantic("WORLDVIEWPROJECTION").AsMatrix();

            InputElement[] vertexLayout =
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
            };
            EffectPass passDesc = _technique.GetPassByIndex(0);
            _inputLayout = new InputLayout(_context.Device, passDesc.Description.Signature, vertexLayout);

        }

        public void Render()
        {
            _context.Device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, _vertexStride, 0));
            _context.Device.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
            _context.Device.InputAssembler.InputLayout = _inputLayout;

            _wvpVar.SetMatrix(_context.Camera.ViewProjectionMatrix);

            for (int i = 0; i < _technique.Description.PassCount; i++)
            {
                _technique.GetPassByIndex(i).Apply();
                _context.Device.Draw(_vertices.Count, 0);
            }
        }
    }
}

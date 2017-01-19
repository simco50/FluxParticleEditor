using System.Windows.Forms;
using ParticleEditor.Helpers;
using ParticleEditor.Model.ImageControl;
using SharpDX;
using Point = System.Drawing.Point;

namespace ParticleEditor.Model.Graphics
{
    public class OrbitCamera
    {
        public float MinimumDistance { get; set; } = 2.0f;
        public float MaximumDistance { get; set; } = 25.0f;

        public Vector3 ResetAngles { get; set; } = new Vector3();
        private Vector3 _eulerAngles;

        private float _zoom = 0.5f;
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = MathUtil.Clamp(value, 0, 1); }
        }

        public bool LeftMouseDown { get; set; } = false;
        public bool MiddleMouseDown { get; set; } = false;
        public float MouseSensitivity = 0.015f;
        private Point _lastMousePos;

        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ViewInverseMatrix { get; set; }
        public Matrix ViewProjectionMatrix { get; set; }
        private Matrix _rotationInverseMatrix;

        public Vector3 Position { get { return ViewInverseMatrix.TranslationVector; } }
        private Vector3 _offset;

        private DX10RenderCanvas _canvasControl;
            
        public OrbitCamera(DX10RenderCanvas canvasControl)
        {
            _canvasControl = canvasControl;
            Reset();
            DebugLog.Log("Camera initialized", "Camera");
        }

        public void Update(float deltaTime)
        {
            Point mousePos = Cursor.Position;
            Vector2 dMouse = new Vector2(mousePos.X - _lastMousePos.X, mousePos.Y - _lastMousePos.Y);
            dMouse *= MouseSensitivity;
            if (LeftMouseDown)
            {
                _eulerAngles.Y -= dMouse.Y;
                _eulerAngles.X -= dMouse.X;
            }
            if (MiddleMouseDown)
                _offset += (Vector3)Vector3.Transform(new Vector3(dMouse.X, -dMouse.Y, 0), _rotationInverseMatrix);

            _lastMousePos = mousePos;

            float distance = MinimumDistance + (1.0f - Zoom) * (MaximumDistance - MinimumDistance);

            Matrix t2 = Matrix.Translation(_offset);

            Matrix rotationYaw = Matrix.RotationYawPitchRoll(_eulerAngles.X, 0.0f, 0.0f);
            Matrix rotationPitch = Matrix.RotationYawPitchRoll(0, _eulerAngles.Y, 0.0f);
            _rotationInverseMatrix = Matrix.Invert(rotationYaw * rotationPitch);
            Matrix translation = Matrix.Translation(new Vector3(0.0f, 0.0f, distance));

            ViewMatrix = t2 * rotationYaw * rotationPitch * translation;

            ViewInverseMatrix = Matrix.Invert(ViewMatrix);
            ProjectionMatrix = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, (float)_canvasControl.ActualWidth / (float)_canvasControl.ActualHeight, 0.1f, 1000f);
            ViewProjectionMatrix = ViewMatrix * ProjectionMatrix;
        }

        public void Reset()
        {
            _eulerAngles = ResetAngles;
            _offset = new Vector3();
            Zoom = 0.5f;
        }
    }
}

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using ParticleEditor.Annotations;
using SharpDX;
using Point = System.Drawing.Point;

namespace ParticleEditor.Graphics.ImageControl
{
    public class OrbitCamera : INotifyPropertyChanged, ICamera
    {
        public float MinimumDistance { get; set; } = 2.0f;
        public float MaximumDistance { get; set; } = 10.0f;

        private Vector3 eulerAngles = new Vector3();

        private float _zoom = 1;
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < 0)
                    _zoom = 0;
                else if (_zoom > 1)
                    _zoom = 1;
                OnPropertyChanged("Zoom");
            }
        }

        public bool MouseDown { get; set; } = false;
        public float MouseSensitivity = 0.01f;
        private Point _lastMousePos;

        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ViewInverseMatrix { get; set; }
        public Matrix ViewProjectionMatrix { get; set; }

        public Vector3 Position { get { return ViewMatrix.TranslationVector; } }

        private DX10RenderCanvas _canvasControl;
            
        public OrbitCamera(DX10RenderCanvas canvasControl)
        {
            _canvasControl = canvasControl;
        }

        public void Update(float deltaTime)
        {
            Point mousePos = Cursor.Position;
            if (MouseDown)
            {
                Vector2 dMouse = new Vector2(mousePos.X - _lastMousePos.X, mousePos.Y - _lastMousePos.Y);
                dMouse *= MouseSensitivity;

                eulerAngles.Y -= dMouse.Y;
                eulerAngles.X -= dMouse.X;
            }
            _lastMousePos = mousePos;

            float distance = MinimumDistance + (1.0f - Zoom) * (MaximumDistance - MinimumDistance);

            Matrix rotation = Matrix.RotationYawPitchRoll(eulerAngles.X, 0.0f, 0.0f);
            Matrix rotation2 = Matrix.RotationYawPitchRoll(0, eulerAngles.Y, 0.0f);
            Matrix translation = Matrix.Translation(0.0f, 0.0f, distance);

            ViewMatrix = rotation * rotation2 * translation;

            ViewInverseMatrix = Matrix.Invert(ViewMatrix);
            ProjectionMatrix = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, (float)_canvasControl.ActualWidth / (float)_canvasControl.ActualHeight, 0.1f, 1000f);
            ViewProjectionMatrix = ViewMatrix * ProjectionMatrix;
        }

        public void Reset()
        {
            eulerAngles = new Vector3();
            Zoom = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

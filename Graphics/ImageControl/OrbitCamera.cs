using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight.CommandWpf;
using SharpDX;
using Point = System.Drawing.Point;

namespace ParticleEditor.Graphics.ImageControl
{
    public class OrbitCamera
    {
        public Vector2 DistanceMinMax { get; set; } = new Vector2(1, 10);
        private Vector3 eulerAngles = new Vector3();
        private Vector3 _position = new Vector3();
        public float Distance { get; set; } = 1;
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public bool MouseDown { get; set; } = false;

        private Point _lastMousePos;

        public void Update(float deltaTime)
        {
            if (MouseDown)
            {
                Point mousePos = Cursor.Position;
                Vector2 dMouse = new Vector2(mousePos.X - _lastMousePos.X, mousePos.Y - _lastMousePos.Y);
                dMouse.Normalize();
                dMouse /= 10.0f;
                _lastMousePos = mousePos;

                eulerAngles.X += dMouse.Y;
                eulerAngles.Y += dMouse.X;
            }

            float distance = DistanceMinMax.X + Distance * (DistanceMinMax.Y - DistanceMinMax.X);

            _position.X = (float) Math.Cos(eulerAngles.Y);
            _position.Z = (float) Math.Sin(eulerAngles.Y);
            _position.Y = (float) Math.Cos(eulerAngles.X);
            _position *= distance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SharpDX;
using Point = System.Drawing.Point;

namespace ParticleEditor.ViewModels.Controls
{
    class SingleControlViewModel : ViewModelBase
    {
        private float _shownValue;
        public float Sensitivity { get; set; } = 0.2f;
        public float MaxValue { get; set; } = 100;
        public float MinValue { get; set; } = 0;

        public float ShownValue
        {
            get { return _shownValue; }
            set
            {
                _shownValue = value;
                RaisePropertyChanged("ShownValue");
            }
        }

        public RelayCommand<MouseEventArgs> OnMouseDownCommand => new RelayCommand<MouseEventArgs>(OnMouseDown);

        private Point _lastMousePos;

        private void OnMouseDown(MouseEventArgs args)
        {
            _lastMousePos = System.Windows.Forms.Cursor.Position;
        }

        public RelayCommand<MouseEventArgs> OnMouseMoveCommand => new RelayCommand<MouseEventArgs>(OnMouseMove);
        private new void OnMouseMove(MouseEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                Point currPos = System.Windows.Forms.Cursor.Position;
                float diff = currPos.X - _lastMousePos.X;
                ShownValue += diff * Sensitivity;
                _lastMousePos = currPos;
                ShownValue = MathUtil.Clamp(ShownValue, MinValue, MaxValue);
                ShownValue = (float)Math.Round(ShownValue, 2);
            }
        }
    }
}

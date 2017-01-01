using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using Point = System.Drawing.Point;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Annotations;
using SharpDX;

namespace ParticleEditor.Views
{
    /// <summary>
    /// Interaction logic for SingleControl.xaml
    /// </summary>
    public partial class SingleControl : UserControl
    {
        public SingleControl()
        {
            Sensitivity = 0.1f;
            MinValue = 0.0f;
            MaxValue = 100.0f;
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(float), typeof(SingleControl), new PropertyMetadata(default(float)));

        public float Value
        {
            get { return (float) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueNameProperty = DependencyProperty.Register(
            "ValueName", typeof(string), typeof(SingleControl), new PropertyMetadata(default(string)));

        public string ValueName
        {
            get { return (string) GetValue(ValueNameProperty); }
            set { SetValue(ValueNameProperty, value); }
        }

        public static readonly DependencyProperty SensitivityProperty = DependencyProperty.Register(
            "Sensitivity", typeof(float), typeof(SingleControl), new PropertyMetadata(default(float)));

        public float Sensitivity
        {
            get { return (float) GetValue(SensitivityProperty); }
            set { SetValue(SensitivityProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", typeof(float), typeof(SingleControl), new PropertyMetadata(default(float)));

        public float MinValue
        {
            get { return (float) GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", typeof(float), typeof(SingleControl), new PropertyMetadata(default(float)));

        public float MaxValue
        {
            get { return (float) GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
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
                Value += diff * Sensitivity;
                _lastMousePos = currPos;
                Value = MathUtil.Clamp(Value, MinValue, MaxValue);
                Value = (float)Math.Round(Value, 2);
            }
        }
    }
}

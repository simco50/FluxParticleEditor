using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media.Media3D;
using Point = System.Drawing.Point;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Annotations;
using ParticleEditor.ViewModels.Controls;
using SharpDX;

namespace ParticleEditor.Views
{
    /// <summary>
    /// Interaction logic for SingleControl.xaml
    /// </summary>
    public partial class SingleControl : UserControl
    {
        private SingleControlViewModel _vm;

        public SingleControl()
        {
            InitializeComponent();
            _vm = new SingleControlViewModel();
            Grid.DataContext = _vm;

            Sensitivity = 0.1f;
            MinValue = 0.0f;
            MaxValue = 100.0f;
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(float), typeof(SingleControl), new PropertyMetadata(OnValueChanged));

        public float Value
        {
            get { return (float) GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                _vm.ShownValue = value;
            }
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as SingleControl)._vm.ShownValue = (float)args.NewValue;
        }

        public static readonly DependencyProperty SensitivityProperty = DependencyProperty.Register(
            "Sensitivity", typeof(float), typeof(SingleControl), new PropertyMetadata(OnSensitivityChanged));

        public float Sensitivity
        {
            get { return (float) GetValue(SensitivityProperty); }
            set { SetValue(SensitivityProperty, value); }
        }
        private static void OnSensitivityChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as SingleControl)._vm.Sensitivity = (float)args.NewValue;
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", typeof(float), typeof(SingleControl), new PropertyMetadata(OnMinValueChanged));

        public float MinValue
        {
            get { return (float) GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as SingleControl)._vm.MinValue = (float)args.NewValue;
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", typeof(float), typeof(SingleControl), new PropertyMetadata(OnMaxValueChanged));

        public float MaxValue
        {
            get { return (float) GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as SingleControl)._vm.MaxValue = (float)args.NewValue;
        }

        public static readonly DependencyProperty ValueNameProperty = DependencyProperty.Register(
            "ValueName", typeof(string), typeof(SingleControl), new PropertyMetadata(default(string)));

        public string ValueName
        {
            get { return (string)GetValue(ValueNameProperty); }
            set { SetValue(ValueNameProperty, value); }
        }

        public RelayCommand OnMouseUpCommand => new RelayCommand(OnMouseUp);
        private void OnMouseUp()
        {
            Value = float.Parse(Content.Text, CultureInfo.InvariantCulture);
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ParticleEditor.Helpers;
using ParticleEditor.Model.Data;
using SharpDX;

namespace ParticleEditor.ViewModels.ParameterTabs
{
    public class AnimationTabViewModel : ViewModelBase
    {
        public ParticleSystem ParticleSystem { get; set; }

        public RelayCommand<bool> SelectColorCommand => new RelayCommand<bool>(SelectColor);
        private void SelectColor(bool active)
        {
            if (active)
                return;

            ColorDialog dialog = new ColorDialog();
            dialog.SolidColorOnly = true;
            dialog.AnyColor = true;
            dialog.FullOpen = true;
            dialog.Color = System.Drawing.Color.FromArgb((int)(ParticleSystem.Color.Constant.X * 255), (int)(ParticleSystem.Color.Constant.Y * 255), (int)(ParticleSystem.Color.Constant.Z * 255));
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            ParticleSystem.Color.SetConstant(new Vector3(dialog.Color.R / 255.0f, dialog.Color.G / 255.0f,
                dialog.Color.B / 255.0f));

            ParticleSystem.RaisePropertyChanged("Color");
        }

        public class KeyframeElement
        {
            public KeyframeElement(string n, object d)
            {
                Name = n;
                Data = d;
            }
            public string Name;
            public object Data;
        }

        public RelayCommand<FrameworkElement> OnSelectionChangedCommand => new RelayCommand<FrameworkElement>(OnSelectionChanged);
        private void OnSelectionChanged(FrameworkElement selectedItem)
        {
            Messenger.Default.Send<MessageData, TimelineViewModel>(new MessageData(MessageID.KeyframeSelected,
                new KeyframeElement(selectedItem.Name, selectedItem.Tag)));
        }
    }
}

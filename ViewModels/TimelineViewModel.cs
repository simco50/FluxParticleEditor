using System;
using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ParticleEditor.Helpers;
using ParticleEditor.Model.Data;

namespace ParticleEditor.ViewModels
{
    public class TimelineViewModel : ViewModelBase
    {
        public ParticleSystem ParticleSystem { get; set; }

        private Grid _timelineGrid;

        public TimelineViewModel(Grid timelineGrid)
        {
            _timelineGrid = timelineGrid;
            Messenger.Default.Register<MessageData>(this, OnMessageReceived);
        }

        private void OnMessageReceived(MessageData data)
        {
            Type t = data.Data.GetType();
            if (t == typeof(KeyFramedValueFloat))
            {
                _timelineGrid.Children.Clear();
                Label label = new Label();
                label.Content = "Selected KeyFramedValueFloat";
                _timelineGrid.Children.Add(label);
            }
            else if(t == typeof(KeyFramedValueVector3))
            {
                _timelineGrid.Children.Clear();
                Label label = new Label();
                label.Content = "Selected KeyFramedValueVector3";
                _timelineGrid.Children.Add(label);
            }
        }
    }
}

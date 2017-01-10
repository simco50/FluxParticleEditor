using System;
using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ParticleEditor.Helpers;
using ParticleEditor.Model.Data;
using ParticleEditor.Views.AnimationControls;

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
                _timelineGrid.Children.Add(new FloatKeyframeView(data.Data as KeyFramedValueFloat));
            }
            else if(t == typeof(KeyFramedValueVector3))
            {
                _timelineGrid.Children.Clear();
                _timelineGrid.Children.Add(new VectorKeyframeView(data.Data as KeyFramedValueVector3));
            }
        }
    }
}

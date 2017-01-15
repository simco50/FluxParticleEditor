using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ParticleEditor.Helpers;
using ParticleEditor.Model.Data;
using ParticleEditor.Views.AnimationControls;
using KeyframeElement = ParticleEditor.ViewModels.ParameterTabs.AnimationTabViewModel.KeyframeElement;

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
            SetInfoText();
        }

        private void OnMessageReceived(MessageData data)
        {
            if (data.Id == MessageId.KeyframeSelected)
            {
                KeyframeElement element = data.Data as KeyframeElement;
                if (element == null)
                {
                    DebugLog.Log("Data received is not of type 'KeyframeElement!'", "Animation", LogSeverity.Error);
                    return;
                }
                Type t = element.Data.GetType();
                if (t == typeof(KeyFramedValueFloat))
                {
                    _timelineGrid.Children.Clear();
                    FloatKeyframeView kfv = new FloatKeyframeView(element.Data as KeyFramedValueFloat, element.Name);
                    _timelineGrid.Children.Add(kfv);
                }
                else if (t == typeof(KeyFramedValueVector3))
                {
                    _timelineGrid.Children.Clear();
                    VectorKeyframeView kfv = new VectorKeyframeView(element.Data as KeyFramedValueVector3, element.Name);
                    _timelineGrid.Children.Add(kfv);
                }
                else
                    DebugLog.Log(
                        $"Invalid keyframe type '{t.Name}'. Type must be either 'KeyFramedValueFloat' or 'KeyFramedValueVector3'",
                        "Animation", LogSeverity.Error);
            }
            else if(data.Id == MessageId.ParticleSystemChanged)
            {
                SetInfoText();
            }
        }

        void SetInfoText()
        {
            _timelineGrid.Children.Clear();
            Label lbl = new Label();
            lbl.Content = "Select a property from the animation tab to start animating it.";
            lbl.VerticalContentAlignment = VerticalAlignment.Center;
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            _timelineGrid.Children.Add(lbl);
        }
    }
}

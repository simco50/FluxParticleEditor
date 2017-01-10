using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Helpers;
using ParticleEditor.Model.Data;

namespace ParticleEditor.ViewModels.AnimationControls
{
    class FloatKeyframeViewModel : ViewModelBase
    {
        public FloatKeyframeViewModel() {}

        public KeyFramedValueFloat Value { get; set; }

        public RelayCommand<KeyValuePair<float, float>> AddKeyframeCommand => new RelayCommand<KeyValuePair<float, float>>(AddKeyframe);
        private void AddKeyframe(KeyValuePair<float, float> keyframeData)
        {
            Value[keyframeData.Key] = keyframeData.Value;
        }

        public RelayCommand ClearAllCommand => new RelayCommand(ClearAll);
        private void ClearAll()
        {
            Value.SetConstant(Value.Constant);
        }

        public RelayCommand<float> SetConstantCommand => new RelayCommand<float>(SetConstant);
        private void SetConstant(float value)
        {
            Value.SetConstant(value);
        }

        public RelayCommand<float> RemoveKeyframeCommand => new RelayCommand<float>(RemoveKeyframe);
        private void RemoveKeyframe(float key)
        {
            if(Value.Keys.ContainsKey(key) == false)
                DebugLog.Log($"Keyframe with key {key} does not exist!", "Animation", LogSeverity.Warning);
            else
                Value.Remove(key);
        }
    }
}

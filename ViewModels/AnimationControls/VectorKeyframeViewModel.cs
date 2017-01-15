using System.Collections.Generic;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Helpers;
using ParticleEditor.Model.Data;
using SharpDX;

namespace ParticleEditor.ViewModels.AnimationControls
{
    class VectorKeyframeViewModel
    {
        public KeyFramedValueVector3 Value { get; set; }

        public RelayCommand<KeyValuePair<float, Vector3>> AddKeyframeCommand => new RelayCommand<KeyValuePair<float, Vector3>>(AddKeyframe);
        private void AddKeyframe(KeyValuePair<float, Vector3> keyframeData)
        {
            Value[keyframeData.Key] = keyframeData.Value;
        }

        public RelayCommand ClearAllCommand => new RelayCommand(ClearAll);
        private void ClearAll()
        {
            DebugLog.Log("Cleared animation property", "Animation");
            Value.SetConstant(Value.Constant);
        }

        public RelayCommand<CustomVector3> SetConstantCommand => new RelayCommand<CustomVector3>(SetConstant);
        private void SetConstant(CustomVector3 value)
        {
            Value.SetConstant(value);
        }

        public RelayCommand<float> RemoveKeyframeCommand => new RelayCommand<float>(RemoveKeyframe);
        private void RemoveKeyframe(float key)
        {
            if (Value.Keys.ContainsKey(key) == false)
                DebugLog.Log($"Keyframe with key {key} does not exist!", "Animation", LogSeverity.Warning);
            else
                Value.Remove(key);
        }
    }
}

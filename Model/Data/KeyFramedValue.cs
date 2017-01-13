using System;
using System.Linq;
using DrWPF.Windows.Data;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

using FloatEnumerator = DrWPF.Windows.Data.ObservableSortedDictionary<float, float>.Enumerator<float, float>;
using VectorEnumerator = DrWPF.Windows.Data.ObservableSortedDictionary<float, ParticleEditor.Model.Data.CustomVector3>.Enumerator<float, ParticleEditor.Model.Data.CustomVector3>;

namespace ParticleEditor.Model.Data
{
    public interface IKeyFramedValue<T>
    {
        void SetConstant(T value);
        void Remove(float key);
    }

    public class KeyFramedValueFloat : ObservableObject, IKeyFramedValue<float>
    {
        public KeyFramedValueFloat(float value)
        {
            Constant = value;
        }

        public void SetConstant(float value)
        {
            Keys.Clear();
            Constant = value;
            RaisePropertyChanged("IsAnimated");
        }

        public void Remove(float key)
        {
            if (Keys.ContainsKey(key))
            {
                Keys.Remove(key);
                RaisePropertyChanged("IsAnimated");
            }
        }

        public float this[float t]
        {
            get
            {
                //If the value is constant
                if (Keys.Count == 0)
                    return Constant;

                //If there is only one keyframe
                if (Keys.Count == 1)
                {
                    var keyframe = Keys.First();
                    if (t > keyframe.Key)
                        return keyframe.Value;
                    float blendA = (keyframe.Key - t) / keyframe.Key;
                    float result = (Constant * blendA) + (keyframe.Value * (1 - blendA));
                    return result;
                }

                //If there is more than 1 keyframe
                if (t < Keys.First().Key)
                    return Keys.First().Value;

                FloatEnumerator e = (FloatEnumerator)Keys.GetEnumerator();
                bool valid = e.MoveNext();

                var prev = e;
                while (valid)
                {
                    if (e.Current.Key == t)
                        return e.Current.Value;
                    if (e.Current.Key > t)
                    {
                        float length = Math.Abs(e.Current.Key - prev.Current.Key);
                        float blendA = (e.Current.Key - t) / length;
                        if (length == 0)
                            blendA = 0;
                        float result = (prev.Current.Value * blendA) + (e.Current.Value * (1 - blendA));
                        return result;
                    }
                    prev = e;
                    valid = e.MoveNext();
                }
                return Keys.Last().Value;
            }
            set
            {
                Keys[t] = value; 
                RaisePropertyChanged("IsAnimated");
            }
        }
        [JsonProperty("Keys")]
        public ObservableSortedDictionary<float, float> Keys { get; set; } = new ObservableSortedDictionary<float, float>(new KeyComparer());

        private float _constant;
        [JsonProperty("Constant")]
        public float Constant
        {
            get { return _constant; }
            set
            {
                _constant = value;
                RaisePropertyChanged("Constant");
            }
        }
        [JsonIgnore]
        public bool IsAnimated
        {
            get { return Keys.Count > 0; }
        }
    }

    public class KeyFramedValueVector3 : ObservableObject, IKeyFramedValue<CustomVector3>
    {

        public KeyFramedValueVector3(CustomVector3 value)
        {
            Constant = value;
        }

        public void SetConstant(CustomVector3 value)
        {
            Keys.Clear();
            Constant = value;
            RaisePropertyChanged("IsAnimated");
        }

        public void Remove(float key)
        {
            if (Keys.ContainsKey(key))
            {
                Keys.Remove(key);
                RaisePropertyChanged("IsAnimated");
            }
        }

        public CustomVector3 this[float t]
        {
            get
            {
                //If the value is constant
                if (Keys.Count == 0)
                    return Constant;

                //If there is only one keyframe
                if (Keys.Count == 1)
                {
                    var keyframe = Keys.First();
                    if (t > keyframe.Key)
                        return keyframe.Value;
                    float blendA = (keyframe.Key - t) / keyframe.Key;
                    CustomVector3 result = (Constant * blendA) + (keyframe.Value * (1 - blendA));
                    return result;
                }

                //If there is more than 1 keyframe
                if (t < Keys.First().Key)
                    return Keys.First().Value;

                VectorEnumerator e = (VectorEnumerator)Keys.GetEnumerator();
                bool valid = e.MoveNext();
                var prev = e;
                while (valid)
                {
                    if (e.Current.Key == t)
                        return e.Current.Value;
                    if (e.Current.Key > t)
                    {
                        float length = Math.Abs(e.Current.Key - prev.Current.Key);
                        float blendA = (e.Current.Key - t) / length;
                        if (length == 0)
                            blendA = 0;
                        CustomVector3 result = (prev.Current.Value * blendA) + (e.Current.Value * (1 - blendA));
                        return result;
                    }
                    prev = e;
                    valid = e.MoveNext();
                }
                return Keys.Last().Value;
            }
            set
            {
                Keys[t] = value; 
                RaisePropertyChanged("IsAnimated");
            }
        }

        [JsonProperty("Keys")]
        public ObservableSortedDictionary<float, CustomVector3> Keys { get; set; } = new ObservableSortedDictionary<float, CustomVector3>(new KeyComparer());

        private CustomVector3 _constant;
        [JsonProperty("Constant")]
        public CustomVector3 Constant
        {
            get { return _constant; }
            set
            {
                _constant = value;
                RaisePropertyChanged("Constant");
            }
        }

        [JsonIgnore]
        public bool IsAnimated
        {
            get { return Keys.Count > 0; }
        }
    }
}

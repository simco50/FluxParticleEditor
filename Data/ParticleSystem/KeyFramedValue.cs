using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ParticleEditor.Data.ParticleSystem
{
    public interface IKeyFramedValue<T>
    {
        void SetConstant(T value);
        void Remove(float key);
    }

    public class KeyFramedValueFloat : IKeyFramedValue<float>
    {
        public KeyFramedValueFloat(float value)
        {
            Constant = value;
        }

        public void SetConstant(float value)
        {
            Keys.Clear();
            Constant = value;
        }

        public void Remove(float key)
        {
            if (Keys.ContainsKey(key))
                Keys.Remove(key);
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

                bool valid = true;
                var e = Keys.GetEnumerator();
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
            set { Keys[t] = value; }
        }
        [JsonProperty("Keys")]
        public SortedDictionary<float, float> Keys { get; set; } = new SortedDictionary<float, float>();
        [JsonProperty("Constant")]
        public float Constant { get; set; }
    }


    public class KeyFramedValueVector3 : IKeyFramedValue<Vector3>
    {
        public KeyFramedValueVector3(Vector3 value)
        {
            Constant = value;
        }

        public void SetConstant(Vector3 value)
        {
            Keys.Clear();
            Constant = value;
        }

        public void Remove(float key)
        {
            if (Keys.ContainsKey(key))
                Keys.Remove(key);
        }

        public Vector3 this[float t]
        {
            get
            {
                //If the value is constant
                if (Keys.Count == 0)
                    return Constant;

                //If there is only one keyframe
                if (Keys.Count == 1)
                {
                    var keyframe = Keys.GetEnumerator().Current;
                    if (t > keyframe.Key)
                        return keyframe.Value;
                    float blendA = (keyframe.Key - t) / keyframe.Key;
                    Vector3 result = (Constant * blendA) + (keyframe.Value * (1 - blendA));
                    return result;
                }

                //If there is more than 1 keyframe
                if (t < Keys.First().Key)
                    return Keys.First().Value;

                bool valid = true;
                var e = Keys.GetEnumerator();
                var prev = e;
                while(valid)
                {
                    if (e.Current.Key == t)
                        return e.Current.Value;
                    if (e.Current.Key > t)
                    {
                        float length = Math.Abs(e.Current.Key - prev.Current.Key);
                        float blendA = (e.Current.Key - t) / length;
                        if (length == 0)
                            blendA = 0;
                        Vector3 result = (prev.Current.Value * blendA) + (e.Current.Value * (1 - blendA));
                        return result;
                    }
                    prev = e;
                    valid = e.MoveNext();
                }
                return Keys.Last().Value;
            }
            set { Keys[t] = value; }
        }

        [JsonProperty("Keys")]
        public SortedDictionary<float, Vector3> Keys { get; set; } = new SortedDictionary<float, Vector3>();

        [JsonProperty("Constant")]
        public Vector3 Constant { get; set; }
    }
}

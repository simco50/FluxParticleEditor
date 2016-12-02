using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ParticleEditor.Data
{
    public class KeyFramedValue<T> : SortedDictionary<float, T>
    {
        public KeyFramedValue(T value)
        {
            Add(0, value);
            Add(1, value);
        }
        public void Set(float key, T value)
        {
            if (ContainsKey(key))
                Add(key, value);
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ParticleEditor.Annotations;

namespace ParticleEditor.Helpers
{
    public class ObservableDictionary<K, V>
    {
        public ObservableCollection<ObservablePair<K, V>> Collection { get; set; } =
            new ObservableCollection<ObservablePair<K, V>>();

        public bool Add(K key, V value)
        {
            ObservablePair<K, V> v = new ObservablePair<K, V>(key, value);
            foreach (var p in Collection)
            {
                if (p.Key.Equals(key))
                    return false;
            }
            Collection.Add(v);
            return true;
        }

        public void AddOverwrite(K key, V value)
        {
            foreach (var p in Collection)
            {
                if (p.Key.Equals(key))
                {
                    p.Value = value;
                    return;
                }
            }
            Collection.Add(new ObservablePair<K, V>(key, value));
        }

        public bool Remove(K key)
        {
            foreach (var p in Collection)
            {
                if (p.Key.Equals(key))
                {
                    Collection.Remove(p);
                    return true;
                }
            }
            return false;
        }

        public V this[K k]
        {
            get
            {
                foreach (var p in Collection)
                {
                    if (p.Equals(k))
                        return p.Value;
                }
                return Collection[-1].Value;
            }
            set
            {
                AddOverwrite(k, value);
            }
        }
    }

    public class ObservablePair<K, V> : INotifyPropertyChanged
    {
        public ObservablePair()
        {
        }

        public ObservablePair(K key, V value)
        {
            Key = key;
            Value = value;
        }

        private K _key;

        public K Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        private V _value;

        public V Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
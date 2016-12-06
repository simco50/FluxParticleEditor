using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ParticleEditor.Annotations;

namespace ParticleEditor.Helpers
{
    public class ObservableDictionary<K, V> : ObservableCollection<ObservablePair<K, V>>
    {
        public ObservableDictionary()
        {
        }

        public bool AddUnique(K key, V value)
        {
            foreach (var p in Items)
            {
                if (p.Key.Equals(key))
                {
                    p.Value = value;
                    return false;
                }
            }
            Add(new ObservablePair<K, V>(key, value));
            return true;
        }

        public bool Remove(K key)
        {
            foreach (var p in Items)
            {
                if (p.Key.Equals(key))
                {
                    Remove(p);
                    return true;
                }
            }
            return false;
        }

        public V this[K k]
        {
            get
            {
                foreach (var p in Items)
                {
                    if (p.Equals(k))
                        return p.Value;
                }
                return Items[-1].Value;
            }
            set
            {
                AddUnique(k, value);
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
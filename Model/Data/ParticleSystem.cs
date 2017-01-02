using System.Collections.Generic;
using System.ComponentModel;
using DrWPF.Windows.Data;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using ParticleEditor.Helpers.UndoRedo;
using SharpDX;

namespace ParticleEditor.Model.Data
{
    public enum ParticleSortingMode
    {
        [Description("Front to back")]
        FrontToBack = 0,
        [Description("Back to front")]
        BackToFront = 1,
        [Description("Oldest first")]
        OldestFirst = 2,
        [Description("Youngest first")]
        YoungestFirst = 3,
    }

    public enum ParticleBlendMode
    {
        [Description("Alpha blending")]
        AlphaBlend = 0,
        [Description("Additive")]
        AdditiveBlend = 1,
    }

    public class ParticleSystem : ObservableObject
    {
        public ParticleSystem()
        {
        }

        [JsonProperty("Version")]
        public int Version = 2;

        //General
        private float _duration = 1.0f;
        private float _startSize = 1.0f;
        private int _maxParticles = 200;
        private int _emission = 20;
        private bool _loop = true;
        private float _lifetime = 1.0f;
        private float _lifetimeVariance = 0.0f;
        private float _startVelocity = 1.0f;
        private float _startVelocityVariance = 0.0f;
        private float _startSizeVariance = 0.0f;
        private bool _randomStartRotation = false;
        private bool _playOnAwake = true;
        private ParticleSortingMode _sortingMode = ParticleSortingMode.FrontToBack;
        private ParticleBlendMode _blendMode = ParticleBlendMode.AlphaBlend;

        [JsonProperty("Duration")]
        public float Duration
        {
            get { return _duration; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("Duration", this, _duration, value, "Duration change"));
                _duration = value;
                RaisePropertyChanged("Duration");
            }
        }

        [JsonProperty("Loop")]
        public bool Loop
        {
            get { return _loop; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("Loop", this, _loop, value, "Loop change"));
                _loop = value; 
                RaisePropertyChanged("Loop");
            }
        }

        [JsonProperty("Lifetime")]
        public float Lifetime
        {
            get { return _lifetime; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("Lifetime", this, _lifetime, value, "Lifetime change"));
                _lifetime = value;
                RaisePropertyChanged("Lifetime");
            }
        }

        [JsonProperty("LifetimeVariance")]
        public float LifetimeVariance
        {
            get { return _lifetimeVariance; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("LifetimeVariance", this, _lifetimeVariance, value, "Lifetime variance change"));
                _lifetimeVariance = value;
                RaisePropertyChanged("LifetimeVariance");
            }
        }

        [JsonProperty("StartVelocity")]
        public float StartVelocity
        {
            get { return _startVelocity; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("StartVelocity", this, _startVelocity, value, "Start velocity change"));
                _startVelocity = value;
                RaisePropertyChanged("StartVelocity");
            }
        }

        [JsonProperty("StartVelocityVariance")]
        public float StartVelocityVariance
        {
            get { return _startVelocityVariance; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("StartVelocityVariance", this, _startVelocityVariance, value, "StartVelocityVariance change"));
                _startVelocityVariance = value;
                RaisePropertyChanged("StartVelocityVariance");
            }
        }


        [JsonProperty("StartSize")]
        public float StartSize
        {
            get { return _startSize; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("StartSize", this, _startSize, value, "Start size change"));
                _startSize = value;
                RaisePropertyChanged("StartSize");
            }
        }

        [JsonProperty("StartSizeVariance")]
        public float StartSizeVariance
        {
            get { return _startSizeVariance; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("StartSizeVariance", this, _startSizeVariance, value, "Start size variance change"));
                _startSizeVariance = value;
                RaisePropertyChanged("StartSizeVariance");
            }
        }

        [JsonProperty("RandomStartRotation")]
        public bool RandomStartRotation
        {
            get { return _randomStartRotation; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("RandomStartRotation", this, _randomStartRotation, value, "Random start rotation change"));
                _randomStartRotation = value;
                RaisePropertyChanged("RandomStartRotation");
            }
        }

        [JsonProperty("PlayOnAwake")]
        public bool PlayOnAwake
        {
            get { return _playOnAwake; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("PlayOnAwake", this, _playOnAwake, value, "Play on awake change"));
                _playOnAwake = value;
                RaisePropertyChanged("PlayOnAwake");
            }
        }


        [JsonProperty("MaxParticles")]
        public int MaxParticles
        {
            get { return _maxParticles; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("MaxParticles", this, _maxParticles, value, "Max particles change"));
                _maxParticles = value;
                RaisePropertyChanged("MaxParticles");
            }
        }

        //Emission
        [JsonProperty("Emission")]
        public int Emission
        {
            get { return _emission; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("Emission", this, _emission, value, "Emission change"));
                _emission = value;
                RaisePropertyChanged("Emission");
            }
        }

        [JsonProperty("Bursts")]
        public ObservableSortedDictionary<float, int> Bursts { get; set; } =
            new ObservableSortedDictionary<float, int>(new KeyComparer());

        //Shape
        public enum ShapeType
        {
            CIRCLE = 0,
            SPHERE = 1,
            CONE = 2,
            EDGE = 3,
        };
        public class ShapeData : ObservableObject
        {
            private ShapeType _shapeType = ShapeType.CONE;
            private float _radius = 0.1f;
            private bool _emitFromShell = false;
            private bool _emitFromVolume = false;
            private float _angle = 70.0f;

            [JsonProperty("ShapeType")]
            public ShapeType ShapeType
            {
                get { return _shapeType; }
                set
                {
                    UndoManager.Instance.Add(new UndoableProperty<ShapeData>("ShapeType", this, _shapeType, value, "Shape type change"));
                    _shapeType = value;
                    RaisePropertyChanged("ShapeType");
                }
            }

            [JsonProperty("Radius")]
            public float Radius
            {
                get { return _radius; }
                set
                {
                    UndoManager.Instance.Add(new UndoableProperty<ShapeData>("Radius", this, _radius, value, "Radius change"));
                    _radius = value;
                    RaisePropertyChanged("Radius");
                }
            }

            [JsonProperty("EmitFromShell")]
            public bool EmitFromShell
            {
                get { return _emitFromShell; }
                set
                {
                    UndoManager.Instance.Add(new UndoableProperty<ShapeData>("EmitFromShell", this, _emitFromShell, value, "Emit from shell change"));
                    _emitFromShell = value;
                    RaisePropertyChanged("EmitFromShell");
                }
            }

            [JsonProperty("EmitFromVolume")]
            public bool EmitFromVolume
            {
                get { return _emitFromVolume; }
                set
                {
                    UndoManager.Instance.Add(new UndoableProperty<ShapeData>("EmitFromVolume", this, _emitFromVolume, value, "Emit from volume change"));
                    _emitFromVolume = value;
                    RaisePropertyChanged("EmitFromVolume");
                }
            }

            [JsonProperty("Angle")]
            public float Angle
            {
                get { return _angle; }
                set
                {
                    UndoManager.Instance.Add(new UndoableProperty<ShapeData>("Angle", this, _angle, value, "Angle change"));
                    _angle = value;
                    RaisePropertyChanged("Angle");
                }
            }
        };
        [JsonProperty("Shape")]
        public ShapeData Shape { get; set; } = new ShapeData();

        //Animation
        [JsonProperty("Size")]
        public KeyFramedValueFloat Size { get; set; } = new KeyFramedValueFloat(1.0f);
        [JsonProperty("Velocity")]
        public KeyFramedValueVector3 Velocity { get; set; } = new KeyFramedValueVector3(new Vector3(0, 0, 0));
        [JsonProperty("LocalVelocity")]
        public KeyFramedValueVector3 LocalVelocity { get; set; } = new KeyFramedValueVector3(new Vector3(0, 0, 0));
        [JsonProperty("Color")]
        public KeyFramedValueVector3 Color { get; set; } = new KeyFramedValueVector3(new Vector3(1.0f, 1.0f, 1.0f));
        [JsonProperty("Transparancy")]
        public KeyFramedValueFloat Transparancy { get; set; } = new KeyFramedValueFloat(1.0f);
        [JsonProperty("Rotation")]
        public KeyFramedValueFloat Rotation { get; set; } = new KeyFramedValueFloat(0.0f);

        //Rendering
        [JsonProperty("SortingMode")]
        public ParticleSortingMode SortingMode
        {
            get { return _sortingMode; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("SortingMode", this, _sortingMode, value, "Sorting mode change"));
                _sortingMode = value;
                RaisePropertyChanged("SortingMode");
            }
        }

        [JsonProperty("BlendMode")]
        public ParticleBlendMode BlendMode
        {
            get { return _blendMode; }
            set
            {
                UndoManager.Instance.Add(new UndoableProperty<ParticleSystem>("BlendMode", this, _blendMode, value, "Blend mode change"));
                _blendMode = value;
                RaisePropertyChanged("BlendMode");
            }
        }

        [JsonProperty("ImagePath")]
        public string ImagePath { get; set; } = "./Resources/DefaultParticleImage.png";
    }
}

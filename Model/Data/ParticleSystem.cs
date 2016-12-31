using System.Collections.Generic;
using System.ComponentModel;
using DrWPF.Windows.Data;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
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

        [JsonProperty("Duration")]
        public float Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                _duration = MathUtil.Clamp(_duration, 0, 20);
            }
        }

        [JsonProperty("Loop")]
        public bool Loop { get; set; } = true;

        [JsonProperty("Lifetime")]
        public float Lifetime { get; set; } = 1.0f;

        [JsonProperty("LifetimeVariance")]
        public float LifetimeVariance { get; set; } = 0.0f;

        [JsonProperty("StartVelocity")]
        public float StartVelocity { get; set; } = 1.0f;

        [JsonProperty("StartVelocityVariance")]
        public float StartVelocityVariance { get; set; } = 0.0f;

        private float _startSize = 1.0f;

        [JsonProperty("StartSize")]
        public float StartSize
        {
            get { return _startSize; }
            set
            {
                _startSize = value;
                _startSize = MathUtil.Clamp(_startSize, 0, 20);
            }
        }

        [JsonProperty("StartSizeVariance")]
        public float StartSizeVariance { get; set; } = 0.0f;

        [JsonProperty("RandomStartRotation")]
        public bool RandomStartRotation { get; set; } = false;

        [JsonProperty("PlayOnAwake")]
        public bool PlayOnAwake { get; set; } = true;

        private int _maxParticles = 200;

        [JsonProperty("MaxParticles")]
        public int MaxParticles
        {
            get { return _maxParticles; }
            set
            {
                _maxParticles = value;
                _maxParticles = MathUtil.Clamp(_maxParticles, 0, 10000);
            }
        }

        //Emission
        private int _emission = 20;

        [JsonProperty("Emission")]
        public int Emission
        {
            get { return _emission; }
            set {
                _emission = value;
                _emission = MathUtil.Clamp(_emission, 0, 1000);
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
        public class ShapeData
        {
            [JsonProperty("ShapeType")]
            public ShapeType ShapeType { get; set; } = ShapeType.CONE;
            [JsonProperty("Radius")]
            public float Radius { get; set; } = 0.1f;
            [JsonProperty("EmitFromShell")]
            public bool EmitFromShell { get; set; } = false;
            [JsonProperty("EmitFromVolume")]
            public bool EmitFromVolume { get; set; } = false;
            [JsonProperty("Angle")]
            public float Angle { get; set; } = 70.0f;
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
        public ParticleSortingMode SortingMode { get; set; } = ParticleSortingMode.FrontToBack;

        [JsonProperty("BlendMode")]
        public ParticleBlendMode BlendMode { get; set; } = ParticleBlendMode.AlphaBlend;

        [JsonProperty("ImagePath")]
        public string ImagePath { get; set; } = "D:/Personal Work/D3D Engine/FluxEngine/Resources/Textures/Smoke.png";
    }
}

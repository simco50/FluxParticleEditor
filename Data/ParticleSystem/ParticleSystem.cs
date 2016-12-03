using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ParticleEditor.Data.ParticleSystem
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

    public class ParticleSystem
    {
        public ParticleSystem()
        {}

        [JsonProperty("Version")]
        public int Version = 1;
        //General
        [JsonProperty("Duration")]
        public float Duration { get; set; } = 1.0f;
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
        [JsonProperty("StartSize")]
        public float StartSize { get; set; } = 1.0f;
        [JsonProperty("StartSizeVariance")]
        public float StartSizeVariance { get; set; } = 0.0f;
        [JsonProperty("RandomStartRotation")]
        public bool RandomStartRotation { get; set; } = false;
        [JsonProperty("PlayOnAwake")]
        public bool PlayOnAwake { get; set; } = true;
        [JsonProperty("MaxParticles")]
        public int MaxParticles { get; set; } = 100;

        //Emission
        [JsonProperty("Emission")]
        public int Emission { get; set; } = 10;
        [JsonProperty("Bursts")]
        public SortedDictionary<float, int> Bursts { get; set; } = new SortedDictionary<float, int>();

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
            public ShapeType ShapeType = ShapeType.CONE;
            [JsonProperty("Radius")]
            public float Radius = 1.0f;
            [JsonProperty("EmitFromShell")]
            public bool EmitFromShell = false;
            [JsonProperty("EmitFromVolume")]
            public bool EmitFromVolume = false;
            [JsonProperty("Angle")]
            public float Angle = 30.0f;
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
        public string ImagePath { get; set; } = "../../Resources/DefaultParticleImage.png";
    }
}

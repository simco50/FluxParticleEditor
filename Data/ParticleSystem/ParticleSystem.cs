using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ParticleEditor.Data
{
    public class ParticleSystem
    {
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
            CIRCLE,
            SPHERE,
            CONE,
            EDGE,
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
        public KeyFramedValue<float> Size { get; set; } = new KeyFramedValue<float>(1.0f);
        [JsonProperty("Velocity")]
        public KeyFramedValue<Vector3> Velocity { get; set; } = new KeyFramedValue<Vector3>(new Vector3(0, 0, 0));
        [JsonProperty("LocalVelocity")]
        public KeyFramedValue<Vector3> LocalVelocity { get; set; } = new KeyFramedValue<Vector3>(new Vector3(0, 0, 0));
        [JsonProperty("Color")]
        public KeyFramedValue<Vector3> Color { get; set; } = new KeyFramedValue<Vector3>(new Vector3(1.0f, 1.0f, 1.0f));
        [JsonProperty("Transparancy")]
        public KeyFramedValue<float> Transparancy { get; set; } = new KeyFramedValue<float>(1.0f);
        [JsonProperty("Rotation")]
        public KeyFramedValue<float> Rotation { get; set; } = new KeyFramedValue<float>(0.0f);

        //Rendering
        public enum ParticleSortingMode
        {
            FrontToBack,
            BackToFront,
            OldestFirst,
            YoungestFirst,
        }
        public ParticleSortingMode SortingMode { get; set; } = ParticleSortingMode.FrontToBack;
        public string ImagePath { get; set; } = "../../Resources/DefaultParticleImage.png";
    }
}

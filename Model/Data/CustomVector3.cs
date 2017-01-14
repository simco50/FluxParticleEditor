using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using ParticleEditor.Annotations;
using SharpDX;

namespace ParticleEditor.Model.Data
{
    public class CustomVector3 : ObservableObject
    {
        public CustomVector3(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            X = x;
            Y = y;
            Z = z;
        }

        private float _x;

        public float X
        {
            get { return _x; }
            set
            {
                _x = value;
                RaisePropertyChanged("X");
            }
        }

        private float _y;

        public float Y
        {
            get { return _y; }
            set
            {
                _y = value;
                RaisePropertyChanged("Y");
            }
        }

        private float _z;

        public float Z
        {
            get { return _z; }
            set
            {
                _z = value;
                RaisePropertyChanged("Z");
            }
        }

        public static implicit operator CustomVector3(Vector3 other)
        {
            return new CustomVector3(other.X, other.Y, other.Z);
        }

        public static implicit operator Vector3(CustomVector3 other)
        {
            return new Vector3(other.X, other.Y, other.Z);
        }

        public static CustomVector3 operator +(CustomVector3 a, CustomVector3 b)
        {
            return new CustomVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static CustomVector3 operator -(CustomVector3 a, CustomVector3 b)
        {
            return new CustomVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static CustomVector3 operator *(float a, CustomVector3 b)
        {
            return new CustomVector3(a * b.X, a * b.Y, a * b.Z);
        }

        public static CustomVector3 operator *(CustomVector3 a, float b)
        {
            return b * a;
        }

        public static CustomVector3 operator /(float a, CustomVector3 b)
        {
            return new CustomVector3(a / b.X, a / b.Y, a / b.Z);
        }

        public static CustomVector3 operator /(CustomVector3 a, float b)
        {
            return b / a;
        }
    }
}

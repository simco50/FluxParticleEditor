using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ParticleEditor.Annotations;

namespace ParticleEditor.Data
{
    public class Vector3
    {
        public Vector3()
        {
            X = 1.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator+(Vector3 a, Vector3 b)
        {
            Vector3 r = new Vector3();
            r.X = a.X + b.X;
            r.Y = a.Y + b.Y;
            r.Z = a.Z + b.Z;
            return r;
        }

        public static Vector3 operator *(Vector3 a, float b)
        {
            Vector3 r = new Vector3();
            r.X = a.X * b;
            r.Y = a.Y * b;
            r.Z = a.Z * b;
            return r;
        }

        public float X, Y, Z;
    }
}

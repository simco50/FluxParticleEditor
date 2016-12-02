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

        public float X, Y, Z;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParticleEditor.Data.ParticleSystem;
using SharpDX;

namespace ParticleEditor.Graphics
{
    public class Particle
    {
        private ParticleVertex _vertexInfo = new ParticleVertex();
        public ParticleVertex VertexInfo { get {return _vertexInfo;} set { _vertexInfo = value; } }
        public bool Active { get; set; } = false;
        private float _lifeTimer = 0.0f;
        private float _initSize = 0.0f;
        private Vector3 _direction;
        private float _startRotation = 0.0f;

        private float _lifeTime = 0;
        private float _startVelocity = 0;
        private float _startSize = 0;

        private ParticleSystem _particleSystem;

        private Random _random = new Random();

        public Particle(ParticleSystem particleSystem)
        {
            _particleSystem = particleSystem;
        }

        public void Initialize()
        {
            Active = true;
            _lifeTimer = 0.0f;
            GetPositionAndDirection(ref _vertexInfo.Position, ref _direction);
            _lifeTime = RandF(_particleSystem.Lifetime - _particleSystem.LifetimeVariance,
                _particleSystem.Lifetime + _particleSystem.LifetimeVariance);
            if (_lifeTime < 0)
                _lifeTime = 0;
            _startRotation = _particleSystem.RandomStartRotation ? RandF(0.0f, 360.0f) : 0.0f;
            _startVelocity = RandF(_particleSystem.StartVelocity - _particleSystem.StartVelocityVariance,
                _particleSystem.StartVelocity + _particleSystem.StartVelocityVariance);
            _startSize = RandF(_particleSystem.StartSize - _particleSystem.StartSizeVariance,
                _particleSystem.StartSize + _particleSystem.StartSizeVariance);
            if (_startSize < 0)
                _startSize = 0;
            Update(0.016f);
        }

        public void Update(float deltaTime)
        {
            _lifeTimer += deltaTime / _lifeTime;
            if (_lifeTimer >= 1.0f)
            {
                Active = false;
                return;
            }

            Vector3 movement = new Vector3();
            //Constant velocity
            movement += _direction * _startVelocity;
            //World space velocity
            movement += _particleSystem.Velocity[_lifeTimer];
            //Local space velocity
            Vector3 localVel = _particleSystem.LocalVelocity[_lifeTimer];
            if (localVel.LengthSquared() > 0)
            {
                Vector3 up = new Vector3(0, 1, 0);
                Vector3 dir = _direction;
                Vector3 right = Vector3.Cross(dir, up);
                up = Vector3.Cross(right, dir);
                Matrix3x3 mat = new Matrix3x3(
                    right.X, right.Y, right.Z,
                    up.X, up.Y, up.Z,
                    dir.X, dir.Y, dir.Z);
                movement += Vector3.Transform(localVel, mat);
            }
            _vertexInfo.Position += movement * deltaTime;

            Vector3 color = _particleSystem.Color[_lifeTimer];
            _vertexInfo.Color = new Vector4(color.X, color.Y, color.Z, _particleSystem.Transparancy[_lifeTimer]);
            _vertexInfo.Size = _particleSystem.Size[_lifeTimer] * _startSize;
            _vertexInfo.Rotation = _startRotation + _particleSystem.Rotation[_lifeTimer];
        }

        public void Reset()
        {
            Active = false;
            _lifeTimer = 0.0f;
        }

        private float RandF(float a, float b)
        {
            return (float) _random.NextDouble(a, b);
        }

        private void GetPositionAndDirection(ref Vector3 position, ref Vector3 direction)
        {
            direction = new Vector3(RandF(0, 1), 0, 0);
            if (_particleSystem.Shape.ShapeType == ParticleSystem.ShapeType.CIRCLE)
            {
                Matrix3x3 randomMatrix = Matrix3x3.RotationYawPitchRoll(RandF(-MathUtil.Pi, MathUtil.Pi), RandF(-MathUtil.Pi, MathUtil.Pi), 0);
                direction = Vector3.Transform(direction, randomMatrix);

                if (_particleSystem.Shape.EmitFromShell)
                    direction.Normalize();
                position = _particleSystem.Shape.Radius * direction;
                direction.Normalize();
                return;
            }
            if (_particleSystem.Shape.ShapeType == ParticleSystem.ShapeType.SPHERE)
            {
                Matrix3x3 randomMatrix = Matrix3x3.RotationYawPitchRoll(RandF(-MathUtil.Pi, MathUtil.Pi), RandF(-MathUtil.Pi, MathUtil.Pi), RandF(-MathUtil.Pi, MathUtil.Pi));
                direction = Vector3.Transform(direction, randomMatrix);

                if (_particleSystem.Shape.EmitFromShell)
                    direction.Normalize();
                position = _particleSystem.Shape.Radius * direction;
                return;
            }
            if (_particleSystem.Shape.ShapeType == ParticleSystem.ShapeType.CONE)
            {
                Matrix3x3 randomMatrix = Matrix3x3.RotationYawPitchRoll(RandF(-MathUtil.Pi, MathUtil.Pi), RandF(-MathUtil.Pi, MathUtil.Pi), 0);
                position = Vector3.Transform(direction, randomMatrix);

                if (_particleSystem.Shape.EmitFromShell)
                    position.Normalize();
                position *= _particleSystem.Shape.Radius;

                direction = new Vector3();
                direction.Y += RandF(0, _particleSystem.Lifetime);
                float offset = direction.Y * (float)Math.Tan(_particleSystem.Shape.Angle * MathUtil.Pi / 180.0f);
                direction.X += offset * position.X;
                direction.Z += offset * position.Z;

                if (_particleSystem.Shape.EmitFromVolume)
                {
                    position += direction;
                }

                direction.Normalize();
                return;
            }
            if (_particleSystem.Shape.ShapeType == ParticleSystem.ShapeType.EDGE)
            {
                position = new Vector3(RandF(-_particleSystem.Shape.Radius, _particleSystem.Shape.Radius), 0, 0);
                direction = new Vector3(0, 0, 1);
            }
        }

    }
}

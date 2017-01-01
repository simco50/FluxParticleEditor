using System;
using ParticleEditor.Model.Data;
using ParticleEditor.ViewModels;
using SharpDX;

namespace ParticleEditor.Model.Graphics.Particles
{
    public class Particle
    {
        private ParticleVertex _vertexInfo = new ParticleVertex();
        public ParticleVertex VertexInfo { get {return _vertexInfo;} set { _vertexInfo = value; } }
        public bool Active { get; set; } = false;
        private float _lifeTimer = 0.0f;
        private Vector3 _direction;
        private float _startRotation = 0.0f;

        private float _lifeTime = 0;
        public float Lifetime { get { return _lifeTime;} }
        private float _startVelocity = 0;
        private float _startSize = 0;

        private static Random _random = new Random();

        public Particle()
        {

        }

        public void Initialize()
        {
            Active = true;
            _lifeTimer = 0.0f;
            GetPositionAndDirection(ref _vertexInfo.Position, ref _direction);
            _lifeTime = RandF(MainViewModel.MainParticleSystem.Lifetime - MainViewModel.MainParticleSystem.LifetimeVariance,
                MainViewModel.MainParticleSystem.Lifetime + MainViewModel.MainParticleSystem.LifetimeVariance);
            if (_lifeTime < 0)
                _lifeTime = 0;
            _startRotation = MainViewModel.MainParticleSystem.RandomStartRotation ? RandF(0.0f, 360.0f) : 0.0f;
            _startVelocity = RandF(MainViewModel.MainParticleSystem.StartVelocity - MainViewModel.MainParticleSystem.StartVelocityVariance,
                MainViewModel.MainParticleSystem.StartVelocity + MainViewModel.MainParticleSystem.StartVelocityVariance);
            _startSize = RandF(MainViewModel.MainParticleSystem.StartSize - MainViewModel.MainParticleSystem.StartSizeVariance,
                MainViewModel.MainParticleSystem.StartSize + MainViewModel.MainParticleSystem.StartSizeVariance);
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
            movement += MainViewModel.MainParticleSystem.Velocity[_lifeTimer];
            //Local space velocity
            Vector3 localVel = MainViewModel.MainParticleSystem.LocalVelocity[_lifeTimer];
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

            Vector3 color = MainViewModel.MainParticleSystem.Color[_lifeTimer];
            _vertexInfo.Color = new Vector4(color.X, color.Y, color.Z, MainViewModel.MainParticleSystem.Transparancy[_lifeTimer]);
            _vertexInfo.Size = MainViewModel.MainParticleSystem.Size[_lifeTimer] * _startSize;
            _vertexInfo.Rotation = _startRotation + MainViewModel.MainParticleSystem.Rotation[_lifeTimer];
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
            if (MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.CIRCLE)
            {
                Matrix3x3 randomMatrix = Matrix3x3.RotationYawPitchRoll(RandF(-MathUtil.Pi, MathUtil.Pi), RandF(-MathUtil.Pi, MathUtil.Pi), 0);
                direction = Vector3.Transform(direction, randomMatrix);

                if (MainViewModel.MainParticleSystem.Shape.EmitFromShell)
                    direction.Normalize();
                position = MainViewModel.MainParticleSystem.Shape.Radius * direction;
                direction.Normalize();
                return;
            }
            if (MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.SPHERE)
            {
                Matrix3x3 randomMatrix = Matrix3x3.RotationYawPitchRoll(RandF(-MathUtil.Pi, MathUtil.Pi), RandF(-MathUtil.Pi, MathUtil.Pi), RandF(-MathUtil.Pi, MathUtil.Pi));
                direction = Vector3.Transform(direction, randomMatrix);

                if (MainViewModel.MainParticleSystem.Shape.EmitFromShell)
                    direction.Normalize();
                position = MainViewModel.MainParticleSystem.Shape.Radius * direction;
                return;
            }
            if (MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.CONE)
            {
                Matrix3x3 randomMatrix = Matrix3x3.RotationYawPitchRoll(RandF(-MathUtil.Pi, MathUtil.Pi), RandF(-MathUtil.Pi, MathUtil.Pi), 0);
                position = Vector3.Transform(direction, randomMatrix);

                if (MainViewModel.MainParticleSystem.Shape.EmitFromShell)
                    position.Normalize();
                position *= MainViewModel.MainParticleSystem.Shape.Radius;

                direction = new Vector3();
                direction.Y += RandF(0, MainViewModel.MainParticleSystem.Lifetime);
                float offset = direction.Y * (float)Math.Tan(MainViewModel.MainParticleSystem.Shape.Angle * MathUtil.Pi / 180.0f);
                direction.X += offset * position.X / position.Length();
                direction.Z += offset * position.Z / position.Length();

                if (MainViewModel.MainParticleSystem.Shape.EmitFromVolume)
                    position += direction;

                direction.Normalize();
                return;
            }
            if (MainViewModel.MainParticleSystem.Shape.ShapeType == ParticleSystem.ShapeType.EDGE)
            {
                position = new Vector3(RandF(-MainViewModel.MainParticleSystem.Shape.Radius, MainViewModel.MainParticleSystem.Shape.Radius), 0, 0);
                direction = new Vector3(0, 0, 1);
            }
        }

    }
}

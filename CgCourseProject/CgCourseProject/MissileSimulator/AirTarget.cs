using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;
using CgCourseProject.Physics;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.MissileSimulator
{
    public class AirTarget : Module
    {
        private double _pitchSpeed = 5;
        private double _yawSpeed = 5;
        private double _rollSpeed = 5;
        private double _friction = 0.1f;
        private Rigidbody _rig;

        private double Pitch
        {
            get { return Carrier.Rotation.ToEuler().X; }
            set
            {
                Vector3 euler = Carrier.Rotation.ToEuler();
                euler.X = value;
                Carrier.Rotation = Quaternion.Euler(euler);
            }
        }

        private double Roll
        {
            get { return Carrier.Rotation.ToEuler().Z; }
            set
            {
                Vector3 euler = Carrier.Rotation.ToEuler();
                euler.Z = value;
                Carrier.Rotation = Quaternion.Euler(euler);
            }
        }

        private double Yaw
        {
            get { return Carrier.Rotation.ToEuler().Y; }
            set
            {
                Vector3 euler = Carrier.Rotation.ToEuler();
                euler.Y = value;
                Carrier.Rotation = Quaternion.Euler(euler);
            }
        }

        private double Acceleration { get; set; } = 10;


        public AirTarget(Transform carrier) : base(carrier)
        {
        }

        public override void OnUpdate()
        {
            double speed = _rig.Velocity.Magnitude;
            Vector3 force = Carrier.Forward * Acceleration - Carrier.Forward * _friction * speed * speed;
            _rig.AddForce(force);

            Vector3 euler = Carrier.Rotation.ToEuler();

            Logger.GetInstance().LogStatic("AngleX: " + MathUtils.Rad2Deg * euler.X + "; AngleY: " + MathUtils.Rad2Deg * euler.Y + "; AngleZ: " + MathUtils.Rad2Deg * euler.Z + 
                "\nX: " + Carrier.Position.X + "; Y: " + Carrier.Position.Y + "; Y: " + Carrier.Position.Z);
        }

        public override void OnStart()
        {
            base.OnStart();

            _rig = Carrier.GetModule<Rigidbody>();
            if(_rig == null)
                throw new RequiredModuleNotFound("Rigidbody");
        }
    }
}

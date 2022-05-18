using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.Physics
{
    class Rigidbody : Module
    {
        private Vector3 _angularVelocity;
        private Vector3 _velocity;

        public double Mass { get; set; }

        public Rigidbody(Transform carrier) : base(carrier)
        {
            
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // TODO Implement drag

            double timeCoefficient = Time.GetInstance().DeltaTime / 1000;


            Vector3 angularVelocity = _angularVelocity * timeCoefficient;
            
            Quaternion rotation = Quaternion.Euler(angularVelocity);

            Carrier.Rotation = Carrier.Rotation * rotation;

            Carrier.Position += _velocity * timeCoefficient;
        }

        /// <summary>
        /// Прикладывает силу (ускорение) к центру объекта
        /// </summary>
        /// <param name="force">Величина силы по трём осям (единицы на секунду в квадрате)</param>
        public void AddForce(Vector3 force)
        {
            _velocity += force;
        }

        /// <summary>
        /// Добавляет угловую скорость
        /// </summary>
        /// <param name="torque">Угловые скорости по трём осям (в радианах на секунду в квадрате)</param>
        public void AddTorque(Vector3 torque)
        {
            _angularVelocity += torque;
        }

        public Vector3 Velocity => _velocity;
        public Vector3 AngularVelocity => _angularVelocity;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.MissileSimulator
{
    class OrbitMover : Module
    {
        private Transform _orbitCenter;
        private Vector3 _rotationAngle;

        private double _currentAngle = 0;
        private Vector3 _prevPosition;

        public OrbitMover(Transform carrier, Transform orbitCenter) : base(carrier)
        {
            _orbitCenter = orbitCenter;

            _rotationAngle = Randomizer.InsideUnitCircle();

            _currentAngle = 0.1;


            _prevPosition = orbitCenter.Position;
        }
        public override void OnUpdate()
        {
            Carrier.RotateAround(_orbitCenter.Position, _rotationAngle, _currentAngle);
            //Carrier.LookAt(_orbitCenter.Position);

            Vector3 delta = _orbitCenter.Position - _prevPosition;
            Carrier.Position += delta;

            _prevPosition = _orbitCenter.Position;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Input;
using CgCourseProject.Maths;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.MissileSimulator
{
    public class CameraController : Module
    {
        private int _index = 0;
        private readonly Transform[] _fixTransform;
        private readonly string _button;
        private double xAngle, yAngle;

        private Vector3 _prevFixedPosition;

        public CameraController(Transform carrier, Transform[] fixTransform, string switchButtonName) : base(carrier)
        {
            _button = switchButtonName;
            _fixTransform = fixTransform;
        }

        public CameraController(Transform carrier, string switchButtonName, params Transform[] fixTransform) : base(carrier)
        {
            _button = switchButtonName;
            _fixTransform = fixTransform;
        }

        public override void OnStart()
        {
            base.OnStart();

            ChangeFocusedObject();
        }

        private void ChangeFocusedObject()
        {
            Carrier.Position = _fixTransform[_index].Position + new Vector3(0, 0, -10);
            _prevFixedPosition = _fixTransform[_index].Position;
        }

        public override void OnUpdate()
        {
            if (InputManager.DefaultInput.GetButtonPress(_button))
            {
                _index++;
                if (_index >= _fixTransform.Length)
                    _index = 0;

                ChangeFocusedObject();
            }

            if(_fixTransform[_index] == Carrier)
                throw new CameraSelfFixException();

            double mouseX = InputManager.DefaultInput.GetAxis("MouseX");
            double mouseY = InputManager.DefaultInput.GetAxis("MouseY");

            xAngle += 0.03 * mouseX;
            yAngle += 0.03 * mouseY;

            Vector3 rotationAxis = new Vector3(xAngle, yAngle, 0);

            // Simple FPS Rotation
            /*Quaternion rotation = Quaternion.Euler(rotationAxis);
            Carrier.Rotation = rotation;*/
            

            // Rotating around fixed object
            Carrier.RotateAround(_fixTransform[_index].Position, Vector3.Up, 0.1 * mouseX);
            Carrier.RotateAround(_fixTransform[_index].Position, Vector3.Right, 0.1 * mouseY);


            // Looking at fixed object
            Carrier.LookAt(_fixTransform[_index].Position);

            // Follow focused object
            Carrier.Position += _fixTransform[_index].Position - _prevFixedPosition;
            _prevFixedPosition = _fixTransform[_index].Position;
        }
    }
}

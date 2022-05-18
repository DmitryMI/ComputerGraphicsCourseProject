using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CgCourseProject.Maths;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.Input
{
    class WindowsMouseInput : IInputAxis
    {
        public enum MouseAxis
        {
            X, Y
        }

        private readonly MouseAxis _axis;
        private int _prevPosition;
        private int _currentPosition;
        private readonly double _sensitivity;
        

        public WindowsMouseInput(string name, MouseAxis axis)
        {
            _axis = axis;
            Name = name;
            _sensitivity = 10d;

            Time.TimedAction update = new Time.TimedAction(OnUpdate);
            Time.GetInstance().AttachTimedAction(update);
        }

        public WindowsMouseInput(string name, MouseAxis axis, double sensitivity)
        {
            _axis = axis;
            Name = name;
            _sensitivity = 1d / sensitivity;

            Time.TimedAction update = new Time.TimedAction(OnUpdate);
            Time.GetInstance().AttachTimedAction(update);
        }

        private void OnUpdate()
        {
            _prevPosition = _currentPosition;
            _currentPosition = GetData();
        }

        public string Name { get; set; }

        public int GetData()
        {
            Point pos = Cursor.Position;
            if (_axis == MouseAxis.X)
                return pos.X;
            return pos.Y;
        }

        private double GetSpeed()
        {
            double delta = (_currentPosition - _prevPosition) / _sensitivity;

            return MathUtils.Clamp(delta, -1, 1);
        }

        public double Value
        {
            get { return GetSpeed(); }
        }
    }
}

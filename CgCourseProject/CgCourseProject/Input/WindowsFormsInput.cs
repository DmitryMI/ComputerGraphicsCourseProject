using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CgCourseProject.Maths;

namespace CgCourseProject.Input
{
    public class WindowsFormsInput : IInput
    {
        private PictureBox _canvas;
        private IInputAxis[] _axises;
        private IInputButton[] _buttons;
        

        public WindowsFormsInput(PictureBox canvas, IInputAxis[] keys, IInputButton[] buttons)
        {
            _canvas = canvas;
            _axises = keys;
            _buttons = buttons;
        }

        public double GetAxis(string axisName)
        {
            if(_axises == null)
                throw new InputKeyNotFoundException();

            foreach (var axis in _axises)
            {
                if (axis.Name == axisName)
                {
                    return axis.Value;
                }
            }

            throw new InputKeyNotFoundException();
        }

        public bool GetButtonPress(string buttonName)
        {
            if (_buttons == null)
                throw new InputKeyNotFoundException();

            foreach (var button in _buttons)
            {
                if (button.Name == buttonName)
                    return button.IsPressed;
            }
            throw new InputKeyNotFoundException();
        }

        public bool IsButtonUp(string buttonName)
        {
            if (_buttons == null)
                throw new InputKeyNotFoundException();

            foreach (var button in _buttons)
                if (button.Name == buttonName)
                    return button.IsUp;


            throw new InputKeyNotFoundException();
        }

        public bool IsButtonDown(string buttonName)
        {
            if (_buttons == null)
                throw new InputKeyNotFoundException();

            foreach (var button in _buttons)
                if (button.Name == buttonName)
                    return button.IsDown;


            throw new InputKeyNotFoundException();
        }

        public ScreenPoint MousePosition => GetMouseRelativePosition();
        public ScreenPoint ScreenMousePosition => GetMouseAbsolutePosition();

        private ScreenPoint GetMouseRelativePosition()
        {
            Point p = Cursor.Position;
            Point relative = _canvas.PointToClient(p);
            return new ScreenPoint(relative.X, relative.Y);
        }

        private ScreenPoint GetMouseAbsolutePosition()
        {
            Point pos = Cursor.Position;
            return new ScreenPoint(pos.X, pos.Y);
        }
    }
}

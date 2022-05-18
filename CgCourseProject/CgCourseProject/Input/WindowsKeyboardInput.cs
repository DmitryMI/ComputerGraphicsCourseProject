using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.Input
{
    class WindowsKeyboardInput : IInputAxis, IInputButton
    {
        [DllImport("User32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        [Flags]
        public enum KeyCode
        {
            MouseLeftButton = 0x01,
            MouseRightButton = 0x02,
            MouseMiddleButton = 0x04,
            Backspace = 0x08,
            Tab = 0x09,
            Enter = 0x0D,
            Shift = 0x10,
            Control = 0x11,
            Alt = 0x12,
            Space = 0x20,
            LeftArrow = 0x25,
            UpArrow = 0x26,
            RightArrow = 0x27,
            DownArrow = 0x28,
            Number0 = 0x30,
            Number1 = 0x31,
            Number2 = 0x32, 
            Number3 = 0x33,
            Number4 = 0x34, 
            Number5 = 0x35, 
            Number6 = 0x36,
            Number7 = 0x37,
            Number8 = 0x38,
            Number9 = 0x39,
            A = 0x41,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z,
        }

        private readonly KeyCode _positive;
        private readonly KeyCode _negative;
        private bool _hasNegative;
        private bool _prevPressState;

        public string Name { get; set; }
        public bool IsDown => !IsPressedPositive();
        public bool IsUp => IsPressedPositive();

        public bool IsPressed
        {
            get
            {
                bool isDown = IsDown;
                bool prevState = _prevPressState;
                _prevPressState = isDown;
                
                if (!prevState && isDown)
                {
                    return true;
                }
                return false;
            }
        }

        public double Value
        {
            get
            {
                if (IsPressedPositive())
                    return 1d;
                if (_hasNegative && IsPressedNegative())
                    return -1d;

                return 0;
            }
        }

        public WindowsKeyboardInput(string name, KeyCode code)
        {
            _positive = code;
            Name = name;
            _hasNegative = false;
        }

        public WindowsKeyboardInput(string name, KeyCode positiveKey, KeyCode negativeKey)
        {
            Name = name;
            _positive = positiveKey;
            _negative = negativeKey;
            _hasNegative = true;
        }

        private bool IsPressedPositive()
        {
            short pressed = GetKeyState((int)_positive);
            ushort resultMask = 0x8000;

            if ((pressed & resultMask) == 0x8000)
                return true;
            return false;
        }

        private bool IsPressedNegative()
        {
            short pressed = GetKeyState((int)_negative);
            ushort resultMask = 0x8000;

            if ((pressed & resultMask) == 0x8000)
                return true;
            return false;
        }
    }
}

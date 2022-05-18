using System;
using System.Drawing;
using CgCourseProject.Maths;

namespace CgCourseProject.Graphics
{
    public struct PixelColor
    {
        private double _red, _green, _blue;

        public double Red
        {
            get => _red;
            set
            {
                _red = value;
                if (_red > 1) _red = 1;
                else if (_red < 0) _red = 0;
            }
        }

        public double Green
        {
            get => _green;
            set
            {
                _green = value;
                if (_green > 1) _green = 1;
                else if (_green < 0) _green = 0;
            }
        }

        public double Blue
        {
            get => _blue;
            set
            {
                _blue = value;
                if (_blue > 1) _blue = 1;
                else if (_blue < 0) _blue = 0;
            }
        }


        public PixelColor(Color netColor)
        {
            _red = netColor.R / 255d;
            _green = netColor.G / 255d;
            _blue = netColor.B / 255d;
            //ClampVals();
        }

        public PixelColor(double r, double g, double b)
        {
            _red = r;
            _green = g;
            _blue = b;
            ClampVals();
        }

        public static PixelColor operator *(PixelColor color, double k)
        {
            double red = color.Red * k;
            double green = color.Green * k;
            double blue = color.Blue * k;
            if (red > 1)
                red = 1;
            if (green > 1)
                green = 1;
            if (blue > 1)
                blue = 1;

            PixelColor result = new PixelColor(){_red = red, _green = green, _blue = blue};
            return result;
        }

        public static PixelColor operator +(PixelColor colorA, PixelColor colorB)
        {
            return new PixelColor(colorA.Red + colorB.Red, colorA.Green + colorB.Green, colorA.Blue + colorB.Blue);
        }

        public static implicit operator Color(PixelColor color)
        {
            return Color.FromArgb((int) (color.Red * 255), (int) (color.Green * 255), (int) (color.Blue * 255));
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            PixelColor color = (PixelColor) obj;
            double tolerance = 0.001f;

            if (Math.Abs(color.Red - Red) < tolerance && Math.Abs(color.Blue - Blue) < tolerance && Math.Abs(color.Green - Green) < tolerance)
                return true;
            return false;
        }

        private void ClampVals()
        {
            _red = MathUtils.Clamp01(_red);
            _green = MathUtils.Clamp01(_green);
            _blue = MathUtils.Clamp01(_blue);
        }
    }
}

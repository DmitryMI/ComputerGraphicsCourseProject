using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.Maths
{
    public struct ScreenPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public ScreenPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

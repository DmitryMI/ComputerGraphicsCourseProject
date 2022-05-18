using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.Maths
{
    static class Randomizer
    {
        private static Random _rnd = new Random();

        public static double Value(double start, double endExclusive)
        {
            return _rnd.NextDouble() * (start - endExclusive) + start;
        }

        public static double Value01 => _rnd.NextDouble();

        public static void Reinit(int seed)
        {
            _rnd = new Random(seed);
        }

        public static double Angle => Value(0, 2 * Math.PI);

        public static Vector2 InsideUnitCircle()
        {
            double angle = Angle;

            double x = Math.Cos(angle);
            double y = Math.Sin(angle);

            return new Vector2(x, y);
        }

        public static Vector3 IndideUnitSphere()
        {
            double phi = Angle;
            double teta = Angle;

            double x = Math.Sin(teta) * Math.Cos(phi);
            double y = Math.Sin(teta) * Math.Sin(phi);
            double z = Math.Cos(teta);

            return new Vector3(x, y, z);
        }
    }
}

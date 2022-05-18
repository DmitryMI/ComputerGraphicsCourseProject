using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.Maths
{
    public static class MathUtils
    {
        public static double Pi2 = Math.PI / 2;
        public static double Pi4 = Pi2 / 2;
        public static double Pi3 = Math.PI / 3;
        public static double Pi6 = Pi3 / 2;
        public static double Deg2Rad = Math.PI / 180;
        public static double Rad2Deg = 180 / Math.PI;

        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        public static T Clamp<T>(T value, T min, T max) where T:IComparable
        {
            if(min.CompareTo(max) == 1)
                Swap(ref min, ref max);

            int comparisonMin = value.CompareTo(min);
            if (comparisonMin == -1)
                return min;
            int comparisonMax = value.CompareTo(max);
            if (comparisonMax == 1)
                return max;

            return value;
        }

        public static T ClampFast<T>(T value, T min, T max) where T : IComparable
        {
            int comparisonMin = value.CompareTo(min);
            if (comparisonMin == -1)
                return min;
            int comparisonMax = value.CompareTo(max);
            if (comparisonMax == 1)
                return max;

            return value;
        }


        public static double Clamp01(double value)
        {
            if (value < 0)
                return 0;
            if (value > 1)
                return 1;
            return value;
        }
    }
}

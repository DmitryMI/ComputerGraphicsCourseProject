using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.Maths
{
    /// <summary>
    /// Представляет вектор в двумерном пространстве
    /// </summary>
    public class Vector2
    {
        private double _x;
        private double _y;

        public Vector2(double x, double y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// X - компонента
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Y - компонента
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator *(Vector2 a, double k)
        {
            return new Vector2(a.X * k, a.Y * k);
        }

        /// <summary>
        /// Преобразует двумерный вектор в трёхмерный с заполнением Z компоненты нулём
        /// </summary>
        /// <param name="vec2">Двумерный вектор</param>
        public static implicit operator Vector3(Vector2 vec2)
        {
            return new Vector3(vec2.X, vec2.Y, 0);
        }
    }
}

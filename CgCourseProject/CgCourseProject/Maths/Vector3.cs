using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.Maths
{
    /// <summary>
    /// Представляет вектор в 3-хмерном пространстве
    /// </summary>
    public struct Vector3
    {
        public bool Equals(Vector3 other)
        {
            return _x.Equals(other._x) && _y.Equals(other._y) && _z.Equals(other._z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector3 && Equals((Vector3) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _x.GetHashCode();
                hashCode = (hashCode * 397) ^ _y.GetHashCode();
                hashCode = (hashCode * 397) ^ _z.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Нулевой вектор
        /// </summary>
        public static Vector3 Zero = new Vector3(0, 0, 0);
        public static Vector3 Up = new Vector3(0, 1, 0);
        public static Vector3 Forward = new Vector3(0, 0, 1);
        public static Vector3 Right = new Vector3(1, 0, 0);

        private double _x;
        private double _y;
        private double _z;

        public Vector3(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public void Normalize()
        {
            double magn = Magnitude;
            double k = 1d / magn;
            X *= k;
            Y *= k;
            Z *= k;
        }

        public Vector3 Normalized
        {
            get
            {
                var tmp = new Vector3(X, Y, Z); 
                tmp.Normalize();
                return tmp;
            }
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

        /// <summary>
        /// Z - компонента
        /// </summary>
        public double Z
        {
            get => _z;
            set { _z = value; }
        }

        /// <summary>
        /// Длина вектора
        /// </summary>

        public double Magnitude => Math.Sqrt(SqrMagnitude);


        /// <summary>
        /// Квадрат длины вектора. Используйте это, если нужно сравнить длины, т.к. здесь не используется
        /// затратная операция извлечения квадратного корня
        /// </summary>
        public double SqrMagnitude => _x* _x + _y* _y + _z* _z;
    

        /// <summary>
        /// Скалярное произведение векторов
        /// </summary>
        /// <param name="a">Левый вектор</param>
        /// <param name="b">Правый вектор</param>
        /// <returns>Скалярное произведение</returns>
        public static double Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// Векторное произведение векторов
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Векторное произведение</returns>
        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }
        
        /// <summary>
        /// Сложение векторов
        /// </summary>
        /// <param name="a">Первый слагаемый вектор</param>
        /// <param name="b">Второй слагаемый вектор</param>
        /// <returns>Вектор суммы</returns>
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Вычитание векторов
        /// </summary>
        /// <param name="a">Левый операнд</param>
        /// <param name="b">Правый операнд</param>
        /// <returns>Разность векторов</returns>
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Умножение вектора на число
        /// </summary>
        /// <param name="a">Вектор</param>
        /// <param name="k">Число</param>
        /// <returns>Вектор, умноженный на число</returns>
        public static Vector3 operator *(Vector3 a, double k)
        {
            return new Vector3(a.X * k, a.Y * k, a.Z * k);
        }

        /// <summary>
        /// Преобразует трёхмерный вектор в двумерный с потерей Z компоненты
        /// </summary>
        /// <param name="vec3">Трёхмерный вектор</param>
        public static explicit operator Vector2(Vector3 vec3)
        {
            return new Vector2(vec3.X, vec3.Y);
        }
        
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return Math.Abs(a.X - b.X) < 0.001f &&
                   Math.Abs(a.Y - b.Y) < 0.001f &&
                   Math.Abs(a.Z - b.Z) < 0.001f;
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }

        public static Vector3 operator -(Vector3 vec)
        {
            return new Vector3(-vec.X, -vec.Y, -vec.Z);
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CgCourseProject.Maths
{
    public struct Quaternion : ICloneable
    {
        private System.Numerics.Quaternion _value;

        public double W
        {
            get => _value.W;
            set => _value.W = (float) value;
        }

        public double X
        {
            get => _value.X;
            set => _value.X = (float) value;
        }

        public double Y
        {
            get => _value.Y;
            set => _value.Y = (float) value;
        }

        public double Z
        {
            get => _value.Z;
            set => _value.Z = (float) value;
        }

        public static Quaternion Identity = new Quaternion(System.Numerics.Quaternion.Identity);


        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            System.Numerics.Quaternion dotNet = System.Numerics.Quaternion.Add(left.Unwraped, right.Unwraped);
            return new Quaternion(System.Numerics.Quaternion.Normalize(dotNet));
        }

        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            System.Numerics.Quaternion dotNet = System.Numerics.Quaternion.Subtract(left.Unwraped, right.Unwraped);
            return new Quaternion(System.Numerics.Quaternion.Normalize(dotNet));
        }

        public static Quaternion operator *(Quaternion q, double k)
        {
            System.Numerics.Quaternion dotNet = System.Numerics.Quaternion.Multiply(q.Unwraped, (float)k);
            return new Quaternion(System.Numerics.Quaternion.Normalize(dotNet));
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            System.Numerics.Quaternion dotNet = System.Numerics.Quaternion.Multiply(a.Unwraped, b.Unwraped);
            return new Quaternion(System.Numerics.Quaternion.Normalize(dotNet));
        }

        public Quaternion(double x, double y, double z, double w)
        {
            _value = new System.Numerics.Quaternion((float)x, (float)y, (float)z, (float)w);
            _value = System.Numerics.Quaternion.Normalize(_value);
        }

        public Quaternion(Vector3 axis, double angle)
        {
            System.Numerics.Vector3 vec3 = new System.Numerics.Vector3((float)axis.X, (float)axis.Y, (float)axis.Z);
            _value = System.Numerics.Quaternion.CreateFromAxisAngle(vec3, (float)angle);
        }

        // TODO Possibly incorrect
        public Vector3 ToEuler()
        {
            double x = Math.Atan2(2 * (W * X + Y * Z), 1 - 2 * (X * X + Y * Y));
            double y = Math.Asin(2 * (W * Y - Z * X));
            double z = Math.Atan2(2 * (W * Z + X * Y), 1 - 2 * (Y * Y + Z * Z));

            return new Vector3(x, y, z);
        }

        public System.Numerics.Quaternion Unwraped => _value;


        public static Quaternion Euler(Vector3 euler)
        {
            System.Numerics.Quaternion tmp = System.Numerics.Quaternion.CreateFromYawPitchRoll((float)euler.X, (float)euler.Y, (float)euler.Z);

            return new Quaternion(tmp);
        }

        public static Quaternion LookAt(Vector3 sourcePoint, Vector3 destPoint)
        {
            Vector3 forwardVector = (destPoint - sourcePoint).Normalized;

            double dot = Vector3.Dot(Vector3.Forward, forwardVector);

            if (Math.Abs(dot - (-1.0f)) < 0.000001f)
            {
                return new Quaternion(Vector3.Up, Math.PI);
            }
            if (Math.Abs(dot - (1.0f)) < 0.000001f)
            {
                return Quaternion.Identity;
            }

            float rotAngle = (float)Math.Acos(dot);
            Vector3 rotAxis = Vector3.Cross(Vector3.Forward, forwardVector);
            rotAxis.Normalize();
            return new Quaternion(rotAxis, rotAngle);
        }

        private Quaternion(System.Numerics.Quaternion dotNet)
        {
            _value = dotNet;
        }

        public double Magnitude => _value.Length();

        private Vector3 Axis => new Vector3(X, Y, Z);

        public Quaternion Conjugated => new Quaternion(System.Numerics.Quaternion.Conjugate(_value));

        public Quaternion Inversed => new Quaternion(System.Numerics.Quaternion.Inverse(_value));

        public object Clone()
        {
            return new Quaternion(_value);
        }

        public Vector3 Rotate(Vector3 vector)
        {
            Quaternion vectorQuaternion = new Quaternion(vector.X, vector.Y, vector.Z, 0);
            return (this * vectorQuaternion * Inversed).Axis;
        }

        public Quaternion Normalized => new Quaternion(System.Numerics.Quaternion.Normalize(_value));

        public void Normalize()
        {
            _value = System.Numerics.Quaternion.Normalize(_value);
        }
    }
}

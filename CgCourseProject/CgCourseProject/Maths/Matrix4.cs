using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.Maths
{
    public class Matrix4 : ICloneable
    {
        private double[,] _values;

        public Matrix4()
        {
            _values = new double[4, 4];
            for(int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                _values[i, j] = 0;
        }

        public static Matrix4 GetScaleMatrix(double kx, double ky, double kz)
        {
            Matrix4 matrix = new Matrix4();
            matrix[0, 0] = kx;
            matrix[1, 1] = ky;
            matrix[2, 2] = kz;
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix4 GetScaleMatrix(Vector3 scale)
        {
            return GetScaleMatrix(scale.X, scale.Y, scale.Z);
        }

        public static Matrix4 GetTranslationMatrix(double dx, double dy, double dz)
        {
            Matrix4 matrix = new Matrix4();
            matrix[0, 0] = 1;
            matrix[1, 1] = 1;
            matrix[2, 2] = 1;
            matrix[3, 3] = 1;
            matrix[0, 3] = dx;
            matrix[1, 3] = dy;
            matrix[2, 3] = dz;
            return matrix;
        }

        public static Matrix4 GetTranslationMatrix(Vector3 translation)
        {
            return GetTranslationMatrix(translation.X, translation.Y, translation.Z);
        }

        public static Matrix4 GetRotationMatrix(Vector3 rotationVec, double phi)
        {
            double x = rotationVec.X;
            double y = rotationVec.Y;
            double z = rotationVec.Z;
            double x2 = x * x;
            double y2 = y * y;
            double z2 = z * z;
            double xy = x * y;
            double yz = y * z;
            double xz = x * z;
            double cosPhi = Math.Cos(phi);
            double sinPhi = Math.Sin(phi);
            double oneMinusCos = 1 - cosPhi;

            Matrix4 matrix = new Matrix4();

            matrix[0, 0] = cosPhi + oneMinusCos * x2;
            matrix[0, 1] = oneMinusCos * xy - sinPhi * z;
            matrix[0, 2] = oneMinusCos * xz + sinPhi * y;

            matrix[1, 0] = oneMinusCos * xy + sinPhi * z;
            matrix[1, 1] = cosPhi + oneMinusCos * y2;
            matrix[1, 2] = oneMinusCos * yz - sinPhi * x;

            matrix[2, 0] = oneMinusCos * xz - sinPhi * y;
            matrix[2, 1] = oneMinusCos * yz + sinPhi * x;
            matrix[2, 2] = cosPhi + oneMinusCos * z2;

            matrix[3, 3] = 1;

            return matrix;
        }

        public static Matrix4 GetRotationMatrix(Quaternion q)
        {
            double x = q.X;
            double y = q.Y;
            double z = q.Z;
            double w = q.W;
            double x2 = x * x;
            double y2 = y * y;
            double z2 = z *z;
            double yw = y * w;
            double xw = x * w;
            double zw = z * w;
            double xy = x * y;
            double xz = x * z;
            double yz = y * z;

            Matrix4 m = new Matrix4
            {
                [0, 0] = 1 - 2 * y2 - 2 * z2,
                [0, 1] = 2 * xy - 2 * zw,
                [0, 2] = 2 * xz + 2 * yw,
                [1, 0] = 2 * xy + 2 * zw,
                [1, 1] = 1 - 2 * x2 - 2 * z2,
                [1, 2] = 2 * yz - 2 * xw,
                [2, 0] = 2 * xz - 2 * yw,
                [2, 1] = 2 * yz + 2 * xw,
                [2, 2] = 1 - 2 * x2 - 2 * y2,
                [3, 3] = 1
            };

            return m;

            /*Matrix4 m = new Matrix4();

            double wx, wy, wz, xx, yy, yz, xy, xz, zz, x2, y2, z2;
            double s = 2.0d / q.Norm;  // 4 mul 3 add 1 div
            x2 = q.X * s; y2 = q.Y * s; z2 = q.Z * s;
            xx = q.X * x2; xy = q.X * y2; xz = q.X * z2;
            yy = q.Y * y2; yz = q.Y * z2; zz = q.Z * z2;
            wx = q.W * x2; wy = q.W * y2; wz = q.W * z2;

            m[0,0] = 1.0f - (yy + zz);
            m[1,0] = xy - wz;
            m[2,0] = xz + wy;

            m[0,1] = xy + wz;
            m[1,1] = 1.0f - (xx + zz);
            m[2,1] = yz - wx;

            m[0,2] = xz - wy;
            m[1,2] = yz + wx;
            m[2,2] = 1.0f - (xx + yy);

            return m;*/
        }

        public void Transpose()
        {
            for(int i = 0; i < 4; i++)
            for (int j = i; j < 4; j++)
            {
                double tmp = _values[i, j];
                _values[i, j] = _values[j, i];
                _values[j, i] = tmp;
            }
        }

        public Matrix4 Transposed()
        {
            Matrix4 matrix = new Matrix4();
            for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                matrix[i, j] = _values[j, i];
            }
            return matrix;
        }

        public static Matrix4 GetInverseRotation(Vector3 eulerAngles)
        {
            Matrix4 euler = GetRotationMatrix(eulerAngles);
            euler.Transpose();
            return euler;
        }

        public static Matrix4 GetInverseRotation(Quaternion quaternion)
        {
            Matrix4 euler = GetRotationMatrix(quaternion);
            euler.Transpose();
            return euler;
        }

        public static Matrix4 GetRotationMatrix(double angleX, double angleY, double angleZ)
        {
            Matrix4 matrix = new Matrix4();

            double a = Math.Cos(angleX);
            double b = Math.Sin(angleX);
            double c = Math.Cos(angleY);
            double d = Math.Sin(angleY);
            double e = Math.Cos(angleZ);
            double f = Math.Sin(angleZ);

            double ad = a * d;
            double bd = b * d;

            matrix[0, 0] = c * e; // 0
            matrix[0, 1] = -c * f; // 1
            matrix[0, 2] = -d; // 2
            matrix[1, 0] = -bd * e + a * f; // 4
            matrix[1, 1] = bd * f + a * e; // 5
            matrix[1, 2] = -b * c; // 6
            matrix[2, 0] = ad * e + b * f; // 8
            matrix[2, 1] = -ad * f + b * e; // 9
            matrix[2, 2] = a * c; // 10

            matrix[3, 3] = 1; // 15

            return matrix;
        }

        public static Matrix4 GetRotationMatrix(Vector3 eulerAngles)
        {
            return GetRotationMatrix(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
        }
        

        public double this[int i, int j]
        {
            get => _values[i, j];
            set => _values[i, j] = value;
        }

        public static Vector3 operator *(Matrix4 matrix, Vector3 vector)
        {
            double x = vector.X * matrix[0, 0] + vector.Y * matrix[0, 1] + vector.Z * matrix[0, 2] + matrix[0, 3];
            double y = vector.X * matrix[1, 0] + vector.Y * matrix[1, 1] + vector.Z * matrix[1, 2] + matrix[1, 3];
            double z = vector.X * matrix[2, 0] + vector.Y * matrix[2, 1] + vector.Z * matrix[2, 2] + matrix[2, 3];

            return new Vector3(x, y, z);
        }

        public static Vector3 operator *(Vector3 vector, Matrix4 matrix)
        {
            return matrix * vector;
        }

        public static Matrix4 operator *(Matrix4 a, Matrix4 b)
        {
            Matrix4 result = new Matrix4();
            
            // TODO Remove cycle
            for(int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                double summ = 0;
                for (int r = 0; r < 4; r++)
                {
                    summ += a[i, r] * b[r, j];
                }
                result[i, j] = summ;
            }

            return result;
        }

        public static bool operator ==(Matrix4 a, Matrix4 b)
        {
            bool flag = !(a != null && b == null);
            if (a == null && b != null)
                flag = false;

            if (a != null && b != null)
            {
                for (int i = 0; i < 4 && flag; i++)
                for (int j = 0; j < 4 && flag; j++)
                {
                    if (Math.Abs(a[i, j] - b[i, j]) > 0.0001f) // TODO Remove magic number
                        flag = false;
                }
            }

            return flag;
        }

        public static bool operator !=(Matrix4 a, Matrix4 b)
        {
            return !(a == b);
        }

        public object Clone()
        {
            Matrix4 clone = new Matrix4();
            for(int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                clone[i, j] = _values[i, j];
            return clone;
        }
    }
}

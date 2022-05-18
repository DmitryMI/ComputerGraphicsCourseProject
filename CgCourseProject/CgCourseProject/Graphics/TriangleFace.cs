using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;

namespace CgCourseProject.Graphics
{
    public struct TriangleFace
    {
        public Vector3 A { get; set; }
        public Vector3 B { get; set; }
        public Vector3 C { get; set; }

        public Vector3 NormalA { get; set; }
        public Vector3 NormalB { get; set; }
        public Vector3 NormalC { get; set; }

        public Material Material { get; set; }

        public Vector3 Normal { get; set; }
        
        public TriangleFace(Vector3 a, Vector3 b, Vector3 c, Vector3 normal, Vector3 normalA, Vector3 normalB, Vector3 normalC, Material material)
        {
            A = a;
            B = b;
            C = c;
            Material = material;
            Normal = normal;
            NormalA = normalA;
            NormalB = normalB;
            NormalC = normalC;
        }

        public TriangleFace(Vector3 a, Vector3 b, Vector3 c, Vector3 normal, Material material)
        {
            A = a;
            B = b;
            C = c;
            Material = material;
            Normal = normal;
            NormalA = normal;
            NormalB = normal;
            NormalC = normal;
        }

        public TriangleFace(Vector3 a, Vector3 b, Vector3 c, Vector3 normal, PixelColor color)
        {
            A = a;
            B = b;
            C = c;
            Material = new Material();
            Material.Color = color;
            Normal = normal;
            NormalA = normal;
            NormalB = normal;
            NormalC = normal;
        }

        public Vector3 this[int i]
        {
            get
            {
                if (i == 0) return A;
                if (i == 1) return B;
                if (i == 2) return C;
                throw new ArgumentOutOfRangeException();
            }
        }

        public Vector3 GetNormal(int i)
        {

            if (i == 0) return NormalA;
            if (i == 1) return NormalB;
            if (i == 2) return NormalC;
            throw new ArgumentOutOfRangeException();

        }
    }
}

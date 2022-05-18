using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;

namespace CgCourseProject.Graphics
{
    public struct MeshTriangle
    {
        public int PointAIndex { get; set; }
        public int PointBIndex { get; set; }
        public int PointCIndex { get; set; }

        public int NormalAIndex { get; set; }
        public int NormalBIndex { get; set; }
        public int NormalCIndex { get; set; }
        
        public Vector3 Normal { get; set; }

        public Material Material { get; set; }

        public Mesh Mesh { get; set; }
        
        public Vector3 this[int index]
        {
            get
            {
                if (index == 0)
                    return Mesh.Vertexes[PointAIndex];
                if (index == 1)
                    return Mesh.Vertexes[PointBIndex];
                if (index == 2)
                    return Mesh.Vertexes[PointCIndex];
                throw new ArgumentOutOfRangeException();
            }
        }

        public Vector3 PointA
        {
            get { return Mesh.Vertexes[PointAIndex]; }
        }

        public Vector3 PointB
        {
            get { return Mesh.Vertexes[PointBIndex]; }
        }

        public Vector3 PointC
        {
            get { return Mesh.Vertexes[PointCIndex]; }
        }

        public Vector3 PointANormal
        {
            get { return Mesh.VertexNormals[NormalAIndex]; }
        }

        public Vector3 PointBNormal
        {
            get { return Mesh.VertexNormals[NormalBIndex]; }
        }
        
        public Vector3 PointCNormal
        {
            get { return Mesh.VertexNormals[NormalCIndex]; }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;

namespace CgCourseProject.Graphics
{
    public class Mesh
    {
        public Vector3[] Vertexes { get; set; }
        public Vector3[] VertexNormals { get; set; }
        public MeshTriangle[] Triangles { get; set; }

        public Mesh()
        {
            
        }

    }
}

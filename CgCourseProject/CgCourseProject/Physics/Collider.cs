using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Graphics;
using CgCourseProject.Maths;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.Physics
{
    public class BoxCollider : Module
    {
        public bool IsTrigger { get; set; }

        public Vector3 Center { get; set; }

        /// <summary>
        /// Размер коллайдера вдоль оси X
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Размер коллайдера вдоль оси Y
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Размер коллайдера вдоль оси Z
        /// </summary>
        public double Depth { get; set; }

        BoxCollider(Transform carrier, Vector3 center, double w, double h, double d) : base(carrier)
        {
            Center = center;
            Width = w;
            Height = h;
            Depth = d;
        }

        BoxCollider(Transform carrier) : base(carrier)
        {
            Center = Vector3.Zero;

            MeshContainer container = carrier.GetModule<MeshContainer>();
            if (container == null)
            {
                Width = Height = Depth = 1;
            }
            else
            {
                // Find maximums and minimums at each axis
                double 
                    maxX = container.Mesh.Vertexes[0].X,
                    minX = container.Mesh.Vertexes[0].X,
                    maxY = container.Mesh.Vertexes[0].Y,
                    minY = container.Mesh.Vertexes[0].Y,
                    maxZ = container.Mesh.Vertexes[0].Z,
                    minZ = container.Mesh.Vertexes[0].Z;

                foreach (var vertex in container.Mesh.Vertexes)
                {
                    if (vertex.X > maxX)
                        maxX = vertex.X;
                    if (vertex.X < minX)
                        minX = vertex.X;

                    if (vertex.Y > maxY)
                        maxY= vertex.Y;
                    if (vertex.Y < minY)
                        minY = vertex.Y;

                    if (vertex.Z > maxZ)
                        maxZ = vertex.Z;
                    if (vertex.Z < minZ)
                        minZ = vertex.Z;
                }

                Width = Math.Max(Math.Abs(maxX), Math.Abs(minX));
                Height = Math.Max(Math.Abs(maxY), Math.Abs(minY));
                Depth = Math.Max(Math.Abs(maxZ), Math.Abs(minZ));
            }
        }

        public bool IsColliding(BoxCollider other)
        {
            Vector3 toCenter = -Carrier.Position;

            double otherHalfWidth = other.Width / 2;
            double otherHalfHeight = other.Height / 2;
            double otherHalfDepth = other.Depth / 2;
            double halfWidth = Width / 2;
            double halfHeight = Height / 2;
            double halfDepth = Depth / 2;

            Vector3[] points = new Vector3[]
            {
                // Front
                new Vector3(otherHalfWidth, otherHalfHeight, otherHalfDepth), 
                new Vector3(otherHalfWidth, otherHalfHeight, -otherHalfDepth),
                new Vector3(otherHalfWidth, -otherHalfHeight, otherHalfDepth),
                new Vector3(otherHalfWidth, -otherHalfHeight, -otherHalfDepth),

                // Back
                new Vector3(-otherHalfWidth, otherHalfHeight, otherHalfDepth),
                new Vector3(-otherHalfWidth, otherHalfHeight, -otherHalfDepth),
                new Vector3(-otherHalfWidth, -otherHalfHeight, otherHalfDepth),
                new Vector3(-otherHalfWidth, -otherHalfHeight, -otherHalfDepth),
            };

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = other.Carrier.TranformationMatrix * points[i];
                points[i] *= Carrier.InvertedRotationMatrix;
                points[i] += toCenter;

                if (points[i].X < halfWidth & points[i].X > -halfWidth)
                    return true;

                if (points[i].Y < halfHeight & points[i].Y > -halfHeight)
                    return true;

                if (points[i].Z < halfDepth & points[i].Z > -halfDepth)
                    return true;
            }

            return false;
        }

    }
}

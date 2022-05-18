using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;

namespace CgCourseProject.Graphics
{
    public class MeshBuilder
    {
        private static Vector3 ComputeTriangleNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 v0v1 = b - a;
            Vector3 v0v2 = c - a;
            return Vector3.Cross(v0v1, v0v2);
        }
      

        /*public static Mesh Cube(double sideSize, Material material)
        {
            Mesh cube = new Mesh();
            Vector3[] verts = new Vector3[8];
            MeshTriangle[] triangles = new MeshTriangle[12];

            double halfSideSize = sideSize / 2;
            
            // Front side
            verts[0] = new Vector3(halfSideSize, halfSideSize, halfSideSize);
            verts[1] = new Vector3(halfSideSize, -halfSideSize, halfSideSize);
            verts[2] = new Vector3(halfSideSize, -halfSideSize, -halfSideSize);
            verts[3] = new Vector3(halfSideSize, halfSideSize, -halfSideSize);

            // Back side
            verts[4] = new Vector3(-halfSideSize, halfSideSize, halfSideSize);
            verts[5] = new Vector3(-halfSideSize, -halfSideSize, halfSideSize);
            verts[6] = new Vector3(-halfSideSize, -halfSideSize, -halfSideSize);
            verts[7] = new Vector3(-halfSideSize, halfSideSize, -halfSideSize);

            triangles[0] = new MeshTriangle(0, 1, 2, new Vector3(0, 0, 1), material, cube);
            triangles[1] = new MeshTriangle(0, 3, 2, new Vector3(0, 0, 1), material, cube);
            triangles[2] = new MeshTriangle(1, 5, 6, new Vector3(1, 0, 0), material, cube);
            triangles[3] = new MeshTriangle(1, 2, 6, new Vector3(1, 0, 0), material, cube);
            triangles[4] = new MeshTriangle(5, 4, 7, new Vector3(0, -1, 0), material, cube);
            triangles[5] = new MeshTriangle(5, 6, 7, new Vector3(0, -1, 0), material, cube);
            triangles[6] = new MeshTriangle(4, 0, 3, new Vector3(-1, 0, 0), material, cube);
            triangles[7] = new MeshTriangle(4, 7, 3, new Vector3(-1, 0, 0), material, cube);
            triangles[8] = new MeshTriangle(7, 3, 2, new Vector3(0, 1, 0), material, cube);
            triangles[9] = new MeshTriangle(7, 6, 2, new Vector3(0, 1, 0), material, cube);
            triangles[10] = new MeshTriangle(1, 0, 4, new Vector3(0, -1, 0), material, cube);
            triangles[11] = new MeshTriangle(1, 5, 4, new Vector3(0, -1, 0), material, cube);

            cube.Vertexes = verts;
            cube.Triangles = triangles;


            return cube;
        }*/

        public static Mesh TestColoredCube(double sideSize)
        {
            PixelColor[] colors = new PixelColor[]
            {
                new PixelColor(Color.Red),
                new PixelColor(Color.Blue),
                new PixelColor(Color.Green),
                new PixelColor(Color.Yellow),
                new PixelColor(Color.OrangeRed),
                new PixelColor(Color.Magenta),
                new PixelColor(Color.Aqua),
                new PixelColor(Color.DimGray),
                new PixelColor(Color.Fuchsia),
                new PixelColor(Color.Indigo),
                new PixelColor(Color.MediumSlateBlue),
                new PixelColor(Color.SaddleBrown),
            };

            Mesh cube = new Mesh();
            Vector3[] verts = new Vector3[8];
            Vector3[] normals = new Vector3[6];
            MeshTriangle[] triangles = new MeshTriangle[12];

            double halfSideSize = sideSize / 2;

            // Front side
            verts[0] = new Vector3(halfSideSize, halfSideSize, halfSideSize);
            verts[1] = new Vector3(halfSideSize, -halfSideSize, halfSideSize);
            verts[2] = new Vector3(halfSideSize, -halfSideSize, -halfSideSize);
            verts[3] = new Vector3(halfSideSize, halfSideSize, -halfSideSize);

            // Back side
            verts[4] = new Vector3(-halfSideSize, halfSideSize, halfSideSize);
            verts[5] = new Vector3(-halfSideSize, -halfSideSize, halfSideSize);
            verts[6] = new Vector3(-halfSideSize, -halfSideSize, -halfSideSize);
            verts[7] = new Vector3(-halfSideSize, halfSideSize, -halfSideSize);

            normals[0] = new Vector3(1, 0, 0);
            normals[1] = new Vector3(0, 1, 0);
            normals[2] = new Vector3(0, 0, 1);
            normals[3] = new Vector3(-1, 0, 0);
            normals[4] = new Vector3(0, -1, 0);
            normals[5] = new Vector3(0, 0, -1);

            triangles[0] = new MeshTriangle()
            {
                PointAIndex = 0,
                PointBIndex = 1,
                PointCIndex = 2,
                Normal = new Vector3(1, 0, 0),
                Material = new Material()
                {
                    Diffuse = 0.5,
                    Specular = 0.5,
                    Shading = Material.ShadingMode.Flat,
                    Color = colors[0]
                },
                Mesh = cube,
                NormalAIndex = 0,
                NormalBIndex = 0,
                NormalCIndex = 0
            };
        
            triangles[1] = new MeshTriangle(){ PointAIndex = 0, PointBIndex = 3, PointCIndex = 2, NormalAIndex = 0, NormalBIndex = 0, NormalCIndex = 0, Normal = new Vector3(1, 0, 0), Material = new Material(){Color = colors[1]}, Mesh = cube};
            triangles[2] = new MeshTriangle(){PointAIndex = 1, PointBIndex = 5, PointCIndex = 6, NormalAIndex = 4, NormalBIndex = 4, NormalCIndex = 4, Normal =  new Vector3(0, -1, 0), Material = new Material() { Color = colors[2] }, Mesh = cube };
            triangles[3] = new MeshTriangle() { PointAIndex = 1, PointBIndex = 2, PointCIndex = 6, NormalAIndex = 4, NormalBIndex = 4, NormalCIndex = 4, Normal = new Vector3(0, -1, 0), Material = new Material() { Color = colors[3] }, Mesh = cube };
            triangles[4] = new MeshTriangle() { PointAIndex = 5, PointBIndex = 4, PointCIndex = 7, NormalAIndex = 3, NormalBIndex = 3, NormalCIndex = 3, Normal = new Vector3(-1, 0, 0), Material = new Material() { Color = colors[4] }, Mesh = cube };
            triangles[5] = new MeshTriangle() { PointAIndex = 5, PointBIndex = 6, PointCIndex = 7, NormalAIndex = 3, NormalBIndex = 3, NormalCIndex = 3, Normal = new Vector3(-1, 0, 0), Material = new Material() { Color = colors[5] }, Mesh = cube };
            triangles[6] = new MeshTriangle() { PointAIndex = 4, PointBIndex = 0, PointCIndex = 3, NormalAIndex = 1, NormalBIndex = 1, NormalCIndex = 1, Normal = new Vector3(0, 1, 0), Material = new Material() { Color = colors[6] }, Mesh = cube };
            triangles[7] = new MeshTriangle() { PointAIndex = 4, PointBIndex = 7, PointCIndex = 3, NormalAIndex = 1, NormalBIndex = 1, NormalCIndex = 1, Normal = new Vector3(0, 1, 0), Material = new Material() { Color = colors[7] }, Mesh = cube };
            triangles[8] = new MeshTriangle() { PointAIndex = 7, PointBIndex = 3, PointCIndex = 2, NormalAIndex = 5, NormalBIndex = 5, NormalCIndex = 5, Normal = new Vector3(0, 0, -1), Material = new Material() { Color = colors[8] }, Mesh = cube };
            triangles[9] = new MeshTriangle() { PointAIndex = 7, PointBIndex = 6, PointCIndex = 2, NormalAIndex = 5, NormalBIndex = 5, NormalCIndex = 5, Normal = new Vector3(0, 0, -1), Material = new Material() { Color = colors[9] }, Mesh = cube };
            triangles[10] = new MeshTriangle() { PointAIndex = 1, PointBIndex = 0, PointCIndex = 4, NormalAIndex = 2, NormalBIndex = 2, NormalCIndex = 2, Normal = new Vector3(0, 0, 1), Material = new Material() { Color = colors[10] }, Mesh = cube };
            triangles[11] = new MeshTriangle() { PointAIndex = 1, PointBIndex = 5, PointCIndex = 4, NormalAIndex = 2, NormalBIndex = 2, NormalCIndex = 2, Normal = new Vector3(0, 0, 1), Material = new Material() { Color = colors[11] }, Mesh = cube };
            /*triangles[3] = new MeshTriangle(1, 2, 6, new Vector3(0, -1, 0), colors[3], cube);
            triangles[4] = new MeshTriangle(5, 4, 7, new Vector3(-1, 0, 0), colors[4], cube);
            triangles[5] = new MeshTriangle(5, 6, 7, new Vector3(-1, 0, 0),  colors[5], cube);
            triangles[6] = new MeshTriangle(4, 0, 3, new Vector3(0, 1, 0), colors[6], cube);
            triangles[7] = new MeshTriangle(4, 7, 3, new Vector3(0, 1, 0), colors[7], cube);
            triangles[8] = new MeshTriangle(7, 3, 2, new Vector3(0, 0, -1), colors[8], cube);
            triangles[9] = new MeshTriangle(7, 6, 2, new Vector3(0, 0, -1), colors[9], cube);
            triangles[10] = new MeshTriangle(1, 0, 4, new Vector3(0, 0, 1), colors[10], cube);
            triangles[11] = new MeshTriangle(1, 5, 4, new Vector3(0, 0, 1), colors[11], cube);*/

            cube.Vertexes = verts;
            cube.Triangles = triangles;
            cube.VertexNormals = normals;


            return cube;
        }

        private struct ObjVertex
        {
            public int VertexIndex { get; set; }
            public int TextureCoordinateIndex { get; set; }
            public int NormalIndex { get; set; }
        }

        private struct ObjFace
        {
            public ObjVertex[] Vertexes { get; set; }
            public Vector3 Normal { get; set; }
            public bool HasTextureCoordinates { get; set; }
        }

        private struct ObjModel
        {
            public Vector3[] Vertexes { get; set; }
            public Vector3[] Normals { get; set; }
            public Vector3[] TextureCoordinates { get; set; }

            public ObjFace[] Faces { get; set; }

            public string Name { get; set; }
        }

        private static ObjModel[] ReadObjModelFile(string objText)
        {
            List<ObjModel> models = new List<ObjModel>();
            List<Vector3> vertexes = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector3> textureCoordinates = new List<Vector3>();
            List<ObjFace> faces = new List<ObjFace>();
            string name = "";
            bool firstModel = true;

            string[] lines = objText.Split('\n');
            foreach (var line in lines)
            {
                string lineTmp = line.Trim();
                if(lineTmp.StartsWith("#")) // Commentary, skipping
                    continue;

                string prefix = "";
                string[] units = lineTmp.Split(' ');
                prefix = units[0];

                if (prefix == "o") // New model description started
                {

                    if (!firstModel)
                    {
                        ObjModel model = new ObjModel()
                        {
                            Faces = faces.ToArray(),
                            Name = name,
                            Vertexes = vertexes.ToArray(),
                            Normals = normals.ToArray(),
                            TextureCoordinates = textureCoordinates.ToArray()
                        };
                        models.Add(model);
                    }

                    firstModel = false;

                    vertexes.Clear();
                    normals.Clear();
                    textureCoordinates.Clear();
                    faces.Clear();

                    name = units[1];
                }

                if (prefix == "v")
                {
                    Vector3 vector3;
                    bool success = ReadVector(units, 1, out vector3);
                    if(!success)
                        throw new FileReadingException();
                    vertexes.Add(vector3);
                }

                if (prefix == "vn")
                {
                    Vector3 vector3;
                    bool success = ReadVector(units, 1, out vector3);
                    if (!success)
                        throw new FileReadingException();
                    normals.Add(vector3);
                }

                if (prefix == "vt")
                {
                    Vector3 vector3;
                    bool success = ReadVector(units, 1, out vector3);
                    if (!success)
                        throw new FileReadingException();
                    textureCoordinates.Add(vector3);
                }

                if (prefix == "f")
                {
                    ObjFace face;
                    bool success = ReadFace(units, 1, out face);
                    if (!success)
                        throw new FileReadingException();
                    faces.Add(face);
                }
            }

            ObjModel lastModel = new ObjModel()
            {
                Faces = faces.ToArray(),
                Name = name,
                Vertexes = vertexes.ToArray(),
                Normals = normals.ToArray(),
                TextureCoordinates = textureCoordinates.ToArray()
            };
            models.Add(lastModel);

            return models.ToArray();
        }

        private static bool ReadVector(string[] units, int startIndex, out Vector3 vector)
        {
            double x, y, z;
            bool success = true;
            success &= Double.TryParse(units[startIndex], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
            success &= Double.TryParse(units[startIndex + 1], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
            success &= Double.TryParse(units[startIndex + 2], NumberStyles.Any, CultureInfo.InvariantCulture, out z);

            vector = new Vector3(x, y, z);
            return success;
        }

        private static bool ReadFace(string[] units, int startIndex, out ObjFace face)
        {
            bool success = true;
            List<ObjVertex> vertexes = new List<ObjVertex>();
            bool hasTextureCoordinates = true;
            for (int i = startIndex; i < units.Length; i++)
            {
                string[] subUnits = units[i].Split('/');
                int vertexIndex, textureCoordinateIndex, normalIndex;
                success &= Int32.TryParse(subUnits[0], out vertexIndex);
                hasTextureCoordinates &= Int32.TryParse(subUnits[1], out textureCoordinateIndex); // Can be empty
                success &= Int32.TryParse(subUnits[2], out normalIndex);
                vertexes.Add(new ObjVertex()
                {
                    VertexIndex = vertexIndex,
                    NormalIndex = normalIndex,
                    TextureCoordinateIndex = textureCoordinateIndex
                });

            }

            face = new ObjFace();
            face.HasTextureCoordinates = hasTextureCoordinates;
            face.Vertexes = vertexes.ToArray();

            return success;
        }

        private static Vector3 GetTriangleFaceNormal(Vector3 A, Vector3 B, Vector3 C)
        {
            Vector3 v0v1 = B - A;
            Vector3 v0v2 = C - A;
            return Vector3.Cross(v0v1, v0v2);
        }

        public static Mesh FromFileTrianglesOnly(string objFile)
        {
            if(!File.Exists(objFile))
                throw new FileReadingException();
            string data = File.ReadAllText(objFile);

            ObjModel[] models = ReadObjModelFile(data);

            ObjModel model = models[0];

            // Check if not triangulated
            foreach (var face in model.Faces)
            {
                if(face.Vertexes.Length > 3)
                    throw new FileReadingException();
            }

            Mesh mesh = new Mesh();

            Vector3[] vertexes = model.Vertexes;
            Vector3[] vertexNormals = model.Normals;
            MeshTriangle[] triangles = new MeshTriangle[model.Faces.Length];

            for (int i = 0; i < model.Faces.Length; i++)
            {
                ObjFace face = model.Faces[i];
                Vector3 normal = GetTriangleFaceNormal(
                    vertexes[face.Vertexes[0].VertexIndex - 1],
                    vertexes[face.Vertexes[1].VertexIndex - 1],
                    vertexes[face.Vertexes[2].VertexIndex - 1]);
                triangles[i] = new MeshTriangle()
                {
                    PointAIndex = face.Vertexes[0].VertexIndex - 1,
                    PointBIndex = face.Vertexes[1].VertexIndex - 1,
                    PointCIndex = face.Vertexes[2].VertexIndex - 1,
                    Normal = normal,
                    Material = new Material(),
                    Mesh = mesh,
                    NormalAIndex = face.Vertexes[0].NormalIndex - 1,
                    NormalBIndex = face.Vertexes[1].NormalIndex - 1,
                    NormalCIndex = face.Vertexes[2].NormalIndex - 1
                };
                
                // TODO Read material
            }

            mesh.Vertexes = vertexes;
            mesh.Triangles = triangles;
            mesh.VertexNormals = vertexNormals;

            return mesh;
        }
    }
}

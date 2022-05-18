using System;
using System.Collections.Generic;
using System.Drawing;
using CgCourseProject.Maths;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.Graphics
{
    public abstract class Camera : Module
    {
        public double ViewportDistance { get; set; }
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }
        public int CanvasHeight { get;  }
        public int CanvasWidth { get;  }

        public bool UseVertexNormals { get; set; } = true;
        public bool UseParallelRendering { get; set; } = true;

        private int _trianglesRendered;

        protected PixelColor[, ] FrameBuffer;
        protected double[,] ZBuffer;
        protected List<LightSource> LightSources;
        protected List<Vector3> LightSourcePositions;
        private readonly int _canvasWidthHalf;
        private readonly int _canvasHeightHalf;
        
        private Plane[] _clippingPlanes = new Plane[]
        {
            new Plane(){Normal = new Vector3(0, 0, 1), Distance = -1} , // Near
            new Plane(){Normal = new Vector3(0, 0, -1), Distance = 1000}, // Far
            new Plane(){Normal = new Vector3(1.414, 0, 1.414), Distance = 0} , // Left
            new Plane(){Normal = new Vector3(-1.414, 0, 1.414), Distance = 0} , // Right
            new Plane(){Normal = new Vector3(0, -1.414, 1.414), Distance = 0} , // Top
            new Plane(){Normal = new Vector3(0, 1.414, 1.414), Distance = 0} , // Bottom
        };

        public Camera(Transform carrier, int canvasWidth, int canvasHeight) : base(carrier)
        {
            ViewportDistance = 1;
            ViewportWidth = 1;
            ViewportHeight = 1;
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            _canvasHeightHalf = CanvasHeight / 2;
            _canvasWidthHalf = CanvasWidth / 2;
            FrameBuffer = new PixelColor[canvasWidth, canvasHeight];
            PlaceClippingPlanes();

            ZBuffer = new double[CanvasWidth, CanvasHeight];
        }

        private void PlaceClippingPlanes()
        {
            // Near
            _clippingPlanes[0] = new Plane() {Normal = new Vector3(0, 0, 1), Distance = -ViewportDistance};

            // Far
            _clippingPlanes[1] = new Plane() {Normal = new Vector3(0, 0, -1), Distance = 1000};

            // Left
            _clippingPlanes[2] = new Plane() {Normal = new Vector3(1.414, 0, 1.414), Distance = 0};

            // Right
            _clippingPlanes[3] = new Plane() {Normal = new Vector3(-1.414, 0, 1.414), Distance = 0 };

            // Top
            _clippingPlanes[4] = new Plane() { Normal = new Vector3(0, -1.414, 1.414), Distance = 0 };

            // Bottom
            _clippingPlanes[5] = new Plane() { Normal = new Vector3(0, 1.414, 1.414), Distance = 0 };


        }

        public Vector3 TransformPointToCamera(Vector3 pointPosition)
        {
            pointPosition -= Carrier.Position;
            return pointPosition * Carrier.InvertedRotationMatrix;
        }

        public ScreenPoint ViewportToCanvas(Vector2 vieportPoint)
        {
            int x = (int) Math.Round(vieportPoint.X * CanvasWidth / ViewportWidth);
            int y = (int) Math.Round(vieportPoint.Y * CanvasHeight / ViewportHeight);
            return new ScreenPoint(x, y);
        }

        public Vector2 CanvasToViewport(ScreenPoint cavasPixel)
        {
            return new Vector2(cavasPixel.X * (double)ViewportWidth / CanvasWidth, cavasPixel.Y * (double)ViewportHeight / CanvasHeight);
        }

        public Vector2 CanvasToViewport(Vector2 cavasPosition)
        {
            return new Vector2(cavasPosition.X * ViewportWidth / CanvasWidth, cavasPosition.Y * ViewportHeight / CanvasHeight);
        }

        public ScreenPoint WorldToScreenPoint(Vector3 vector3)
        {
            double k = ViewportDistance / vector3.Z;
            Vector2 pos = new Vector2(vector3.X * k, vector3.Y * k);
            return ViewportToCanvas(pos);
        }

        public Vector3 ScreenToWorldPoint(ScreenPoint point, double invertedZ)
        {
            double oz = 1 / invertedZ;
            double ux = point.X * oz / ViewportDistance;
            double uy = point.Y * oz / ViewportDistance;
            Vector2 p2d = CanvasToViewport(new Vector2(ux, uy));
            return new Vector3(p2d.X, p2d.Y, oz);
        }

        public Vector3 ScreenToWorldPoint(int x, int y, double invertedZ)
        {
            double oz = 1 / invertedZ;
            double ux = x * oz / ViewportDistance;
            double uy = y * oz / ViewportDistance;
            Vector2 p2d = CanvasToViewport(new Vector2(ux, uy));
            return new Vector3(p2d.X, p2d.Y, oz);
        }

        protected void DrawLine2D(ScreenPoint a, ScreenPoint b, PixelColor color)
        {
            int
                x = a.X,
                y = a.Y,
                dx = b.X - a.X,
                dy = b.Y - a.Y,
                sx, sy;
            if (dx > 0)
                sx = 1;
            else if (dx == 0)
                sx = 0;
            else
                sx = -1;

            if (dy > 0)
                sy = 1;
            else if (dy == 0)
                sy = 0;
            else
                sy = -1;


            bool exchange = false;
            if (dy < 0)
                dy = -dy;

            if (dx < 0)
                dx = -dx;
            if (dy > dx)
            {
                int t = dy;
                dy = dx;
                dx = t;
                exchange = true;
            }

            if (!exchange)
            {
                int e = 0;
                int m = dy;
                for (int i = 0; Math.Abs(x - b.X) > 0.001f; x += sx, i++)
                {

                    if(x < FrameBuffer.GetLength(0) && y < FrameBuffer.GetLength(1) && x >= 0 && y >= 0)
                        FrameBuffer[x, y] = color;
                    //FrameBuffer[CanvasWidth * x + y] = color;
                    //fixed (PixelColor* ptr = FrameBuffer) ;

                    e += m;
                    if (2 * e >= dy)
                    {
                        y += sy;
                        e -= dx;
                    }
                }
            }
            else
            {
                int e = 0;
                int m = dy;
                for (int i = 0; Math.Abs(y - b.Y) > 0.001f; y += sy, i++)
                {
                    if (x < FrameBuffer.GetLength(0) && y < FrameBuffer.GetLength(1) && x >= 0 && y >= 0)
                        FrameBuffer[x, y] = color;

                    e += m;
                    if (2 * e >= dx)
                    {
                        x += sx;
                        e -= dx;
                    }
                }
            }
        }

        public static List<int> InterpolateInt(int i0, int v0, int i1, int v1)
        {
            if (i0 == i1)
            {
                return new List<int>(){v0};
            }

            int
                x = i0,
                y = v0,
                dx = i1 - i0,
                dy = v1 - v0,
                sx, sy;
            if (dx > 0)
                sx = 1;
            else if (dx == 0)
                sx = 0;
            else
                sx = -1;

            if (dy > 0)
                sy = 1;
            else if (dy == 0)
                sy = 0;
            else
                sy = -1;


            bool exchange = false;
            if (dy < 0)
                dy = -dy;

            if (dx < 0)
                dx = -dx;

            if (dy > dx)
            {
                int t = dy;
                dy = dx;
                dx = t;
                exchange = true;
            }

            List<int> result = new List<int>();

            if (!exchange)
            {
                int e = 0;
                int m = dy;
                for (int i = 0; x != i1; x += sx, i++)
                {

                    result.Add(y);

                    e += m;
                    if (2 * e >= dy)
                    {
                        y += sy;
                        e -= dx;
                    }
                }

                result.Add(y);
            }
            else
            {
                int e = 0;
                int m = dy;
                for (int i = 0; y != v1; y += sy, i++)
                {
                    e += m;
                    if (2 * e >= dx)
                    {
                        result.Add(y);
                        x += sx;
                        e -= dx;
                    }
                }

                result.Add(y);
            }



            return result;
        }

        public static List<double> Interpolate(int i0, double d0, int i1, double d1)
        {
            int count = i1 - i0;

            List<double> result;

            if (count > 0)
                result = new List<double>(count + 1);
            else
                result = new List<double>();

            if (count < 0)
            {
                result.Add(d0); // FIXME
            }
            else if (count == 0)
            {
                result.Add(d0);
            }
            else
            {
                double a = (d1 - d0) / (count);

                double d = d0;
                for (int i = i0; i <= i1; i++, d += a)
                    result.Add(d);
            }
            return result;
        }

        protected void DrawWireframeTriangle(ScreenPoint p0, ScreenPoint p1, ScreenPoint p2, PixelColor edgeColor)
        {
            p0.X += _canvasWidthHalf;
            p1.X += _canvasWidthHalf;
            p2.X += _canvasWidthHalf;
            p0.Y += _canvasHeightHalf;
            p1.Y += _canvasHeightHalf;
            p2.Y += _canvasHeightHalf;
            DrawLine2D(p0, p1, edgeColor);
            DrawLine2D(p1, p2, edgeColor);
            DrawLine2D(p0, p2, edgeColor);
        }
        

        protected double ComputeVertexIntensity(Vector3 vertex, Vector3 normal, Material material)
        {
            Vector3 viewerPos = Carrier.Position;
            Vector3 view = new Vector3(viewerPos.X - vertex.X, viewerPos.Y - vertex.Y, viewerPos.Z - vertex.Z);
            double diffuse = material.Diffuse;
            double specular = material.Specular;

            double illumination = 0;
            for (int i = 0; i < LightSources.Count; i++)
            {
                Vector3 lightPosition = LightSourcePositions[i];

                if (LightSources[i].LightSourceType == LightSource.LightType.Ambient)
                {
                    illumination += LightSources[i].Intensity;
                    continue;
                }

                Vector3 vl = Vector3.Zero;

                if (LightSources[i].LightSourceType == LightSource.LightType.Directional)
                {
                    Matrix4 cameraMatrix4 = Carrier.RotationMatrix.Transposed();
                    Vector3 rotatedLight = cameraMatrix4 * lightPosition;
                    vl = rotatedLight;
                }
                else if (LightSources[i].LightSourceType == LightSource.LightType.Point)
                {
                    vl = lightPosition - vertex;
                }

                // Diffuse component
                double cosAlpha = Vector3.Dot(vl, normal) / (vl.Magnitude * normal.Magnitude);
                if (cosAlpha > 0)
                {
                    
                    illumination += Math.Pow(cosAlpha, diffuse) * LightSources[i].Intensity;
                }

                // Specular component
                Vector3 reflected = (normal * (2 * Vector3.Dot(normal, vl))) - vl;
                

                double cosBeta = Vector3.Dot(reflected, view) / (reflected.Magnitude * view.Magnitude);
                if (cosBeta > 0)
                {
                    illumination += Math.Pow(cosBeta, specular) * LightSources[i].Intensity;
                }
            }

            return illumination;
        }

        protected int[] SortTriangleVertexes(ScreenPoint[] verts)
        {
            int[] indexes = new[]{0, 1, 2};
            

            for (int i = 0; i < 3; i++)
            {
                bool flag = false;
                for (int j = 0; j < 3 - i - 1; j++)
                {
                    if (verts[j].Y > verts[j + 1].Y)
                    {
                        /*Vector3 tmp = verts[j];
                        verts[j] = verts[j + 1];
                        verts[j + 1] = tmp;*/
                        MathUtils.Swap(ref verts[j], ref verts[j + 1]);
                        MathUtils.Swap(ref indexes[j], ref indexes[j + 1]);
                        flag = true;
                    }
                }
                if (!flag)
                    break;
            }

            return indexes;
        }
        
        protected void RenderTriangleFlatShading(TriangleFace triangle)
        {


            //int[] vertindexes = SortTriangleVertexes(triangle);
            Vector3 v0 = triangle.A;
            Vector3 v1 = triangle.B;
            Vector3 v2 = triangle.C;


            #region Backface culling

            //Vector3 faceNormal = triangle.Normal;
            Vector3 faceNormal = triangle.Normal;

            Vector3 center = (v0 + v1 + v2) * -0.3333333d; // Some attention needed

            if (Vector3.Dot(center, faceNormal) < 0)
            {
                return;
            }

            #endregion

            _trianglesRendered++;

            // Compute attribure values (X, 1 / Z) at the verts
            // TODO Fix stupid sorting

            ScreenPoint p0 = WorldToScreenPoint(v0);
            ScreenPoint p1 = WorldToScreenPoint(v1);
            ScreenPoint p2 = WorldToScreenPoint(v2);

            ScreenPoint[] points = new ScreenPoint[]{p0, p1, p2};
            int[] vertindexes = SortTriangleVertexes(points);
            v0 = triangle[vertindexes[0]];
            v1 = triangle[vertindexes[1]];
            v2 = triangle[vertindexes[2]];
            p0 = points[0];
            p1 = points[1];
            p2 = points[2];
            

            if(p0.Y > p1.Y || p0.Y > p2.Y || p1.Y > p2.Y)
                throw new InvalidOperationException();

            // Compute attribute values at the edges
            var xInterpolate = EdgeInterpolate(p0.Y, p0.X, p1.Y, p1.X, p2.Y, p2.X);
            var zInterpolate = EdgeInterpolate(p0.Y, 1d / v0.Z, p1.Y, 1d / v1.Z, p2.Y, 1 / v2.Z);
            
            center = -center;
            var flatIntensity = ComputeVertexIntensity(center, faceNormal, triangle.Material);

            var x02 = xInterpolate[0];
            var x012 = xInterpolate[1];
            var iz02 = zInterpolate[0];
            var iz012 = zInterpolate[1];

            if (x02.Count == 0)
                return;

            List<double> xLeft, xRight;
            List<double> izLeft, izRight;
            int m = x02.Count / 2; 
            if (x02[m] < x012[m])
            {
                xLeft = x02;
                xRight = x012;

                izLeft = iz02;
                izRight = iz012;
            }
            else
            {
                xLeft = x012;
                xRight = x02;

                izLeft = iz012;
                izRight = iz02;
            }

            // Special treatment of horizontal edges
            if (p0.Y == p1.Y)
            {
                double leftXStart = p0.X;
                double rightXStart = p1.X;
                double leftZStart = 1 / v0.Z;
                double rightZStart = 1 / v1.Z;
                if (leftXStart > rightXStart)
                {
                    MathUtils.Swap(ref leftXStart, ref rightXStart);
                    MathUtils.Swap(ref leftZStart, ref rightZStart);
                }
                xLeft[0] = leftXStart;
                xRight[0] = rightXStart;
                izLeft[0] = leftZStart;
                izRight[0] = rightZStart;
            }

            PixelColor color = triangle.Material.Color * flatIntensity;

            for (int y = p0.Y, dy = 0; y < p2.Y; y++, dy++)
            {
                //int dy = y - p0.Y;
                int xl = (int)xLeft[dy];
                int xr = (int)xRight[dy];

                double zl = izLeft[dy];
                double zr = izRight[dy];
                //List<double> zscan = Interpolate(xLeftCurrent, zl, xRightCurrent, zr);
                double z = zl;
                double sz = (zr - zl) / (xr - xl);

                for (int x = xl; x < xr; x++)
                {
                    //double invZ = zscan[x - xl];
                    double invZ = z;

                    //double intLeft, intRight;
                    
                    int nX = x + _canvasWidthHalf;
                    int nY = y + _canvasHeightHalf;
                    if (
                        nX < ZBuffer.GetLength(0) &&
                        nX >= 0 &&
                        nY >= 0 &&
                        nY < ZBuffer.GetLength(1) &&
                        ZBuffer[nX, nY] < invZ
                        )
                    {
                        ZBuffer[nX, nY] = invZ;
                        FrameBuffer[nX, nY] = color;
                    }

                    z += sz;
                }
                
            }

            //DrawWireframeTriangle(p0, p1, p2, new PixelColor(Color.Red));
        }
        
        protected void RenderTriangleGouraundShading(TriangleFace triangle)
        {
            //int[] vertindexes = SortTriangleVertexes(triangle);
            Vector3 v0 = triangle.A;
            Vector3 v1 = triangle.B;
            Vector3 v2 = triangle.C;


            #region Backface culling

            Vector3 faceNormal = triangle.Normal;

            Vector3 center = (v0 + v1 + v2) * -0.3333333d; // Some attention needed

            if (Vector3.Dot(center, faceNormal) < 0)
            {
                return;
            }

            #endregion

            _trianglesRendered++;

            // Compute attribure values (X, 1 / Z) at the verts
            ScreenPoint p0 = WorldToScreenPoint(v0);
            ScreenPoint p1 = WorldToScreenPoint(v1);
            ScreenPoint p2 = WorldToScreenPoint(v2);


            ScreenPoint[] points = new ScreenPoint[] { p0, p1, p2 };
            int[] vertindexes = SortTriangleVertexes(points);
            v0 = triangle[vertindexes[0]];
            v1 = triangle[vertindexes[1]];
            v2 = triangle[vertindexes[2]];
            p0 = points[0];
            p1 = points[1];
            p2 = points[2];

            // Compute attribute values at the edges
            //var xInterpolateInt = EdgeInterpolateInt(p0.Y, p0.X, p1.Y, p1.X, p2.Y, p2.X);
            //var first = EdgeInterpolate(p0.Y, p0.X, p1.Y, p1.X, p2.Y, p2.X);
            //var zInterpolate = EdgeInterpolate(p0.Y, 1d / v0.Z, p1.Y, 1d / v1.Z, p2.Y, 1 / v2.Z);

            Vector3 normal0, normal1, normal2;

            if (UseVertexNormals)
            {
                normal0 = triangle.GetNormal(vertindexes[0]);
                normal1 = triangle.GetNormal(vertindexes[1]);
                normal2 = triangle.GetNormal(vertindexes[2]);
            }
            else
            {
                normal0 = faceNormal;
                normal1 = faceNormal;
                normal2 = faceNormal;
            }

            double i0 = ComputeVertexIntensity(v0, normal0, triangle.Material);
            double i1 = ComputeVertexIntensity(v1, normal1, triangle.Material);
            double i2 = ComputeVertexIntensity(v2, normal2, triangle.Material);
            var gouraudIntensities = EdgeInterpolate(p0.Y, i0, p1.Y, i1, p2.Y, i2);

            //var x02 = xInterpolateInt[0];
            //var x012 = xInterpolateInt[1];
            //var iz02 = zInterpolate[0];
            //var iz012 = zInterpolate[1];
            var i02 = gouraudIntensities[0];
            var i012 = gouraudIntensities[1];

            //if (x02.Count == 0)
                //return;

            List<double> iLeft, iRight;



            bool leftExpands = false;

            int xlInitial = p0.X;
            int xrInitial = p0.X;
            double zlInitial = 1d / v0.Z;
            double zrInitial = 1d / v0.Z;

            int xlFinal;
            int xrFinal;
            double zlFinal, zrFinal;

            //if (x02[m] < x012[m])
            if (p1.X > p0.X)
            {
                xlFinal = p2.X;
                xrFinal = p1.X;
                
                zlFinal = 1d / v2.Z;
                zrFinal = 1d / v1.Z;

                iLeft = i02;
                iRight = i012;
            }
            else
            {
                xlFinal = p1.X;
                xrFinal = p2.X;
               
                zlFinal = 1d / v1.Z;
                zrFinal = 1d / v2.Z;

                iLeft = i012;
                iRight = i02;

                leftExpands = true;
            }

           

            // Special treatment of horizontal edges
            if (p0.Y == p1.Y)
            {
                int leftXStart = p0.X;
                int rightXStart = p1.X;
                double leftZStart = 1 / v0.Z;
                double rightZStart = 1 / v1.Z;
                if (leftXStart > rightXStart)
                {
                    MathUtils.Swap(ref leftXStart, ref rightXStart);
                    MathUtils.Swap(ref leftZStart, ref rightZStart);
                }
                
                xlInitial = leftXStart;
                xrInitial = rightXStart;
                zlInitial = leftZStart;
                zrInitial = rightZStart;
            }

            PixelColor triangleColor = triangle.Material.Color;

            double xlCurrent = xlInitial;
            double xrCurrent = xrInitial;
            double zlCurrect = zlInitial;
            double zrCurrent = zrInitial;
            double sxl, sxr, szl, szr;

            int dxl = xlFinal - xlInitial;
            int dxr = xrFinal - xrInitial;
            double dzl = zlFinal - zlInitial;
            double dzr = zrFinal - zrInitial;
            int dy = p1.Y - p0.Y;
            if (!leftExpands)
            {
                sxl = (double) dxl / (p2.Y - p0.Y);
                sxr = (double) dxr / dy;
                szl = dzl / (p2.Y - p0.Y);
                szr = dzr / dy;
            }
            else
            {
                sxl = (double)dxl / dy;
                sxr = (double)dxr / (p2.Y - p0.Y);
                szl = dzl / dy;
                szr = dzr / (p2.Y - p0.Y);
            }

            for (int y = p0.Y, arrOffset = 0; y <= p2.Y; y++, arrOffset++)
            {
                int xl = (int) Math.Round(xlCurrent);
                int xr = (int)Math.Round(xrCurrent);

                double zl = zlCurrect;
                double zr = zrCurrent;

                double il = iLeft[arrOffset];
                double ir = iRight[arrOffset];

                if (xl > xr)
                {
                    int tmp = xl;
                    xl = xr;
                    xr = tmp;

                    MathUtils.Swap(ref zl, ref zr);

                    MathUtils.Swap(ref il, ref ir);
                }

              


                double invZ = zl;
                double sz;
                double si;
                if (xr - xl != 0)
                {
                    sz = (zr - zl) / (xr - xl);
                    si = (ir - il) / (xr - xl);
                }
                else
                {
                    sz = 0;
                    si = 0;
                }

                int nXl = xl + _canvasWidthHalf;
                if (nXl < 0)
                    nXl = 0;
                else if (nXl >= CanvasWidth)
                    nXl = CanvasWidth;

                int nXr = xr + _canvasWidthHalf;
                if (nXr < 0)
                    nXr = 0;
                else if (nXr >= CanvasWidth)
                    nXr = CanvasWidth;

                int nY = _canvasHeightHalf - y;

                //int invY = CanvasHeight - nY;

                //if (nY >= 0 && nY < CanvasHeight && nXl >= 0 && nXr < CanvasWidth)
                //{

                double intensity = il;

                    for (int x = nXl; x < nXr; x++)
                    {

                        if (x >= 0 && x < CanvasWidth && nY >=0 && nY < CanvasHeight && ZBuffer[x, nY] < invZ)
                            //if(true)
                        {
                            ZBuffer[x, nY] = invZ;

                            FrameBuffer[x, nY] = new PixelColor()
                            {
                                Blue = triangleColor.Blue * intensity,
                                Green = triangleColor.Green * intensity,
                                Red = triangleColor.Red * intensity
                            };

                        }

                        invZ += sz;
                        intensity += si;
                    }

                    if (y == p1.Y)
                    {
                        dy = p2.Y - p1.Y;
                        if (leftExpands)
                        {
                            dxl = (p2.X - p1.X);
                            sxl = (double) dxl / dy;
                            dzl = (1 / v2.Z - 1 / v1.Z);
                            szl = dzl / dy;
                        }
                        else
                        {
                            dxr = p2.X - p1.X;
                            sxr = (double) dxr / dy;
                            dzr = (1 / v2.Z - 1 / v1.Z);
                            szr = dzr / dy;
                        }
                    }
                //}

                xlCurrent += sxl;
                xrCurrent += sxr;
                zlCurrect += szl;
                zrCurrent += szr;
            }

        }

        protected void RenderTrianglePhongShading(TriangleFace triangle)
        {
            Vector3 v0 = triangle.A;
            Vector3 v1 = triangle.B;
            Vector3 v2 = triangle.C;


            #region Backface culling

            Vector3 faceNormal = triangle.Normal;

            Vector3 center = (v0 + v1 + v2) * -0.3333333d; // Some attention needed

            if (Vector3.Dot(center, faceNormal) < 0)
            {
                return;
            }

            #endregion

            _trianglesRendered++;

            // Compute attribure values (X, 1 / Z) at the verts
            ScreenPoint p0 = WorldToScreenPoint(v0);
            ScreenPoint p1 = WorldToScreenPoint(v1);
            ScreenPoint p2 = WorldToScreenPoint(v2);


            ScreenPoint[] points = new ScreenPoint[] { p0, p1, p2 };
            int[] vertindexes = SortTriangleVertexes(points);
            v0 = triangle[vertindexes[0]];
            v1 = triangle[vertindexes[1]];
            v2 = triangle[vertindexes[2]];
            p0 = points[0];
            p1 = points[1];
            p2 = points[2];

            // Compute attribute values at the edges
            var xInterpolate = EdgeInterpolateInt(p0.Y, p0.X, p1.Y, p1.X, p2.Y, p2.X);
            var zInterpolate = EdgeInterpolate(p0.Y, 1d / v0.Z, p1.Y, 1d / v1.Z, p2.Y, 1d / v2.Z);

            Vector3 normal0 = Vector3.Zero, normal1 = Vector3.Zero, normal2 = Vector3.Zero;

            if (UseVertexNormals)
            {
                normal0 = triangle.GetNormal(vertindexes[0]);
                normal1 = triangle.GetNormal(vertindexes[1]);
                normal2 = triangle.GetNormal(vertindexes[2]);
            }
            else
            {
                normal0 = faceNormal;
                normal1 = faceNormal;
                normal2 = faceNormal;
            }
            
            List<double>[] phongIntensitiesX = null;
            List<double>[] phongIntensitiesY = null;
            List<double>[] phongIntensitiesZ = null;

           
            phongIntensitiesX = EdgeInterpolate(p0.Y, normal0.X, p1.Y, normal1.X, p2.Y, normal2.X);
            phongIntensitiesY= EdgeInterpolate(p0.Y, normal0.Y, p1.Y, normal1.Y, p2.Y, normal2.Y);
            phongIntensitiesZ = EdgeInterpolate(p0.Y, normal0.Z, p1.Y, normal1.Z, p2.Y, normal2.Z);
            

            var x02 = xInterpolate[0];
            var x012 = xInterpolate[1];
            var iz02 = zInterpolate[0];
            var iz012 = zInterpolate[1];
            var nx02 = phongIntensitiesX[0];
            var nx012 = phongIntensitiesX[1];
            var ny02 = phongIntensitiesY[0];
            var ny012= phongIntensitiesY[1];
            var nz02 = phongIntensitiesZ[0];
            var nz012 = phongIntensitiesZ[1];

            if(x02.Count == 0)
                return;

            List<int> xLeft, xRight;
            List<double> izLeft, izRight;
            List<double> nxLeft, nxRight;
            List<double> nyLeft, nyRight;
            List<double> nzLeft, nzRight;
            int m = x02.Count / 2; // x012
            if (x02[m] < x012[m])
            {
                xLeft = x02;
                xRight = x012;

                izLeft = iz02;
                izRight = iz012;

                nxLeft = nx02;
                nxRight = nx012;
                nyLeft = ny02;
                nyRight = ny012;
                nzLeft = nz02;
                nzRight = nz012;
            }
            else
            {
                xLeft = x012;
                xRight = x02;

                izLeft = iz012;
                izRight = iz02;

                nxLeft = nx012;
                nxRight = nx02;
                nyLeft = ny012;
                nyRight = ny02;
                nzLeft = nz012;
                nzRight = nz02;
            }

            // Special treatment of horizontal edges
            if (p0.Y == p1.Y)
            {
                int leftXStart = p0.X;
                int rightXStart = p1.X;
                double leftZStart = 1 / v0.Z;
                double rightZStart = 1 / v1.Z;
                if (leftXStart > rightXStart)
                {
                    MathUtils.Swap(ref leftXStart, ref rightXStart);
                    MathUtils.Swap(ref leftZStart, ref rightZStart);
                }
                xLeft[0] = leftXStart;
                xRight[0] = rightXStart;
                izLeft[0] = leftZStart;
                izRight[0] = rightZStart;
            }


            for (int y = p0.Y, dy = 0; y < p2.Y; y++, dy++)
            {
                int xLeftCurrent = xLeft[dy];
                int xRightCurrent = xRight[dy];
                double zl = izLeft[dy];
                double zr = izRight[dy];
                List<double> zscan = Interpolate(xLeftCurrent, zl, xRightCurrent, zr);

                double nxl = nxLeft[dy];
                double nxr = nxRight[dy];
                double nyl = nyLeft[dy];
                double nyr = nyRight[dy];
                double nzl = nzLeft[dy];
                double nzr = nzRight[dy];

                var nxscan = Interpolate(xLeftCurrent, nxl, xRightCurrent, nxr);
                var nyscan = Interpolate(xLeftCurrent, nyl, xRightCurrent, nyr);
                var nzscan = Interpolate(xLeftCurrent, nzl, xRightCurrent, nzr);

                for (int x = xLeftCurrent; x < xRightCurrent; x++)
                {
                    int dx = x - xLeftCurrent;

                    double invZ = zscan[dx];

                    int nX = x + _canvasWidthHalf;
                    int nY = y + _canvasHeightHalf;
                    if (
                        nX < ZBuffer.GetLength(0) &&
                        nX > 0 &&
                        nY > 0 &&
                        nY < ZBuffer.GetLength(1) &&
                        ZBuffer[nX, nY] < invZ
                    )
                    {
                        Vector3 vertex = ScreenToWorldPoint(x, y, invZ);
                        Vector3 normal = new Vector3(nxscan[dx], nyscan[dx], nzscan[dx]);
                        
                        
                        double intensity = ComputeVertexIntensity(vertex, normal, triangle.Material);

                        
                        ZBuffer[nX, nY] = invZ;
                        
                        FrameBuffer[nX, nY] = triangle.Material.Color * intensity;
                    }
                }
            }
        }

        public void DrawFaceNormal(TriangleFace face)
        {
            Vector3 normal = face.Normal;

            Vector3 center = (face.A + face.B + face.C) * 0.3333333d;

            ScreenPoint lineStart = WorldToScreenPoint(center);
            ScreenPoint lineEnd = WorldToScreenPoint(center + normal);

            int xShift = _canvasWidthHalf;
            int yShift = _canvasHeightHalf;

            lineStart.X += xShift;
            lineStart.Y += yShift;
            lineEnd.X += xShift;
            lineEnd.Y += yShift;

            DrawLine2D(lineStart, lineEnd, new PixelColor(Color.Blue));
        }

        public void Render(List<MeshContainer> meshContainers, List<LightSource> lightSources)
        {
            //Time.GetInstance().PauseTime();

            // Create list of clipped triangles
            List<TriangleFace> clippedTriangles = new List<TriangleFace>();


            foreach (var meshContainer in meshContainers)
            {
                foreach (var triangle in meshContainer.Mesh.Triangles)
                {
                    Vector3 aVec = triangle.PointA;
                    Vector3 bVec = triangle.PointB;
                    Vector3 cVec = triangle.PointC;
                    Vector3 normal = triangle.Normal;
                    //Vector3 normal = triangle.C
                    Vector3 normalA;
                    Vector3 normalB;
                    Vector3 normalC;

                    Matrix4 objRotation = meshContainer.Carrier.RotationMatrix;

                    Matrix4 objMatrix = meshContainer.Carrier.TranformationMatrix;
                    aVec = objMatrix * aVec;
                    bVec = objMatrix * bVec;
                    cVec = objMatrix * cVec;
                    normal = objRotation * normal;
                    normalA = objRotation * triangle.PointANormal;
                    normalB = objRotation * triangle.PointBNormal;
                    normalC = objRotation * triangle.PointCNormal;


                    aVec = TransformPointToCamera(aVec);
                    bVec = TransformPointToCamera(bVec);
                    cVec = TransformPointToCamera(cVec);
                    Matrix4 cameraInvRot = Carrier.RotationMatrix.Transposed();
                    normal = cameraInvRot * normal;
                    normalA = cameraInvRot * normalA;
                    normalB = cameraInvRot * normalB;
                    normalC = cameraInvRot * normalC;

                    clippedTriangles.Add(new TriangleFace(aVec, bVec, cVec, normal, normalA, normalB, normalC,
                        triangle.Material));
                }
            }

            // Clipping triangle against all clipping planes
            foreach (var clippingPlane in _clippingPlanes)
            {
                clippedTriangles = ClipTrianglesAgainstPlane(clippedTriangles, clippingPlane);
            }

            for (int i = 0; i < CanvasWidth; i++)
            for (int j = 0; j < CanvasHeight; j++)
            {
                ZBuffer[i, j] = 0;
            }


            _trianglesRendered = 0;

            List<Vector3> lightSourcePositions = new List<Vector3>(lightSources.Count);
            foreach (var source in lightSources)
            {
                Vector3 pos = source.Carrier.Position;
                lightSourcePositions.Add(TransformPointToCamera(pos));
            }

            LightSources = lightSources;
            LightSourcePositions = lightSourcePositions;

#if !DEBUG
            if (UseParallelRendering)
                Parallel.ForEach(clippedTriangles, RenderTriangle);
            else
                foreach (var triangle in clippedTriangles)
                {
                    RenderTriangle(triangle);
                }
#else
            foreach (var triangle in clippedTriangles)
            {
                RenderTriangle(triangle);
            }

            //RenderWireframe(meshContainers);
#endif


            //Logger.GetInstance().LogStatic("Rendered triangles: " + _trianglesRendered);

            //Time.GetInstance().ResumeTime();

            OnRenderFinish();
        }

        private void RenderTriangle(TriangleFace triangle)
        {
            switch (triangle.Material.Shading)
            {
                case Material.ShadingMode.Flat:
                    RenderTriangleFlatShading(triangle);
                    break;
                case Material.ShadingMode.Gouraud:
                    RenderTriangleGouraundShading(triangle);
                    break;
                case Material.ShadingMode.Phong:
                    RenderTrianglePhongShading(triangle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //DrawWireframeTriangle(triangle.A, triangle.B, triangle.C, new PixelColor(Color.Red));
        }

        public void RenderWireframe(List<MeshContainer> meshContainers)
        {
            foreach (var meshContainer in meshContainers)
            {
                foreach (var triangle in meshContainer.Mesh.Triangles)
                {
                    Vector3 aVec = triangle.PointA;
                    Vector3 bVec = triangle.PointB;
                    Vector3 cVec = triangle.PointC;

                    Matrix4 objMatrix = meshContainer.Carrier.TranformationMatrix;
                    aVec = objMatrix * aVec;
                    bVec = objMatrix * bVec;
                    cVec = objMatrix * cVec;

                    aVec = TransformPointToCamera(aVec);
                    bVec = TransformPointToCamera(bVec);
                    cVec = TransformPointToCamera(cVec);

                    ScreenPoint a = WorldToScreenPoint(aVec);
                    ScreenPoint b = WorldToScreenPoint(bVec);
                    ScreenPoint c = WorldToScreenPoint(cVec);

                    


                    DrawWireframeTriangle(a,b, c, new PixelColor(Color.Red));
                }
            }

            OnRenderFinish();
        }

        protected static List<double>[] EdgeInterpolate(int y0, double v0, int y1, double v1, int y2, double v2)
        {
            List<double> v01 = Interpolate(y0, v0, y1, v1);
            List<double> v12 = Interpolate(y1 + 1, v1, y2, v2); // y1 + 1
            List<double> v02 = Interpolate(y0, v0, y2, v2);
            
            
            List<double> v012 = new List<double>(v01.Count + v12.Count);
            v012.AddRange(v01);
            v012.AddRange(v12);
            return new List<double>[] {v02, v012};
        }

        protected static List<int>[] EdgeInterpolateInt(int y0, int v0, int y1, int v1, int y2, int v2)
        {
            List<int> v01 = InterpolateInt(y0, v0, y1, v1);
            List<int> v12 = InterpolateInt(y1 + 1, v1, y2, v2); // y1 + 1
            List<int> v02 = InterpolateInt(y0, v0, y2, v2);


            List<int> v012 = new List<int>(v01.Count + v12.Count);
            v012.AddRange(v01);
            v012.AddRange(v12);
            return new List<int>[] { v02, v012 };
        }

        protected List<TriangleFace> ClipTrianglesAgainstPlane(List<TriangleFace> triangles, Plane plane)
        {
            List<TriangleFace> resultList = new List<TriangleFace>();

            foreach (var triangle in triangles)
            {
                Vector3 v0 = triangle.A;
                Vector3 v1 = triangle.B;
                Vector3 v2 = triangle.C;

                // Если угол между радиус-вектором вершины треугольника и нормалями отсекающих плоскостей меньше 90 градусов, то вершина находится внутри

                bool in0 = Vector3.Dot(plane.Normal, v0) + plane.Distance > 0;
                bool in1 = Vector3.Dot(plane.Normal, v1) + plane.Distance > 0;
                bool in2 = Vector3.Dot(plane.Normal, v2) + plane.Distance > 0;

                if (!in0 && !in1 && !in2)
                {
                    // Nothing to do, because triangle is fully clipped out
                }

                if (in0 && in1 && in2)
                {
                    // Triangle is fully visible
                    resultList.Add(triangle);
                }

                if (in0 && in1 && !in2)
                {
                    // Triangle has two visible vertexes. Output is two clipped triangles
                }

                if (in1 && in2 && !in0)
                {
                    // Triangle has two visible vertexes. Output is two clipped triangles
                }

                if (in0 && in2 && !in1)
                {
                    // Triangle has two visible vertexes. Output is two clipped triangles
                }

                if (in0)
                {
                    // Triangle has only one visible vertex. Output is one clipped triangle
                }
            }

            return resultList;
        }
        /*protected void PutPixel(int x, int y, PixelColor color)
        {
            if (x > 0 && y < CanvasWidth && x > 0 && y < CanvasHeight)
            {
                frameBuffer[x, y] = color;
            }
        }*/

        public abstract void OnRenderFinish();
    }
}

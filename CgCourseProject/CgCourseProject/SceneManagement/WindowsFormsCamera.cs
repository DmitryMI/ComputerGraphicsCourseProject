using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CgCourseProject.Graphics;
using CgCourseProject.Input;
using CgCourseProject.Maths;

namespace CgCourseProject.SceneManagement
{

    /// <summary>
    /// Camera, that draws inside PictureBox using Bitmap
    /// </summary>
    class WindowsFormsCamera : Camera
    {
        private PictureBox _canvas;

        private BitmapData _data;
        private Bitmap _bitmap;
        private int _rectCount;
        private int _heightPart;
        private int _byteArrPartLen;

        public WindowsFormsCamera(Transform carrier, PictureBox pictureBox) : base(carrier, pictureBox.Width,
            pictureBox.Height)
        {
            _canvas = pictureBox;

            ViewportWidth = 1;
            ViewportHeight = 1;

            _rectCount = Environment.ProcessorCount;
            _heightPart = CanvasHeight / _rectCount;

            _byteArrPartLen = CanvasWidth * 4 * _heightPart;

            UseVertexNormals = true;
        }

        public override void OnRenderFinish()
        {
            _bitmap = new Bitmap(CanvasWidth, CanvasHeight);

            _data = _bitmap.LockBits(new Rectangle(0, 0, CanvasWidth, CanvasHeight), ImageLockMode.WriteOnly,
                _bitmap.PixelFormat);
            
            Parallel.For(0, _rectCount, Draw);
                
            _bitmap.UnlockBits(_data);
            _canvas.Image = _bitmap;
        }

        private void Draw(int rectNum)
        {
            int bytes = _byteArrPartLen;
            int arrOffset = 0;

            byte[] rgbPool = new byte[bytes];

            int yStart = _heightPart * rectNum;
            int yEnd = yStart + _heightPart;

            for (int y = yStart; y < yEnd; y++)
            for (int x = 0; x < CanvasWidth; x++)
            {
                if (ZBuffer[x, y] != 0)
                {
                    //Color color = FrameBuffer[x, y];
                    PixelColor color = FrameBuffer[x, y];
                        //bitmap.SetPixel(x, y, FrameBuffer[x, y]);
                    rgbPool[arrOffset] = (byte) (color.Blue * 255);
                    rgbPool[arrOffset + 1] = (byte) (color.Green * 255);
                    rgbPool[arrOffset + 2] = (byte) (color.Red * 255);
                    rgbPool[arrOffset + 3] = 255;
                    //FrameBuffer[x, y] = null;
                }

                arrOffset += 4;
            }

            IntPtr ptr = _data.Scan0;
            ptr += bytes * rectNum;

            Marshal.Copy(rgbPool, 0, ptr, bytes);
        }
    }
}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;

namespace CgCourseProject.Graphics
{
    public class Material
    {
        private double _diffuse;
        private double _specular;

        public enum ShadingMode
        {
            Flat, Gouraud, Phong
        }

        public PixelColor Color { get; set; }

        public double Diffuse
        {
            get => _diffuse;
            set => _diffuse = MathUtils.Clamp(value, 0, 1);
        }

        public double Specular
        {
            get => _specular;
            set => _specular = MathUtils.Clamp(value, 0, 1);
        }

        public ShadingMode Shading { get; set; }

        public Material()
        {
            Color = new PixelColor(1, 1, 1);
            _diffuse = 0.5;
            _specular = 0.5;
            Shading = ShadingMode.Gouraud;
        }

        public Material(PixelColor color, double diffuse, double specular, ShadingMode shading)
        {
            Color = color;
            _diffuse = diffuse;
            _specular = specular;
            Shading = shading;
        }
    }
}

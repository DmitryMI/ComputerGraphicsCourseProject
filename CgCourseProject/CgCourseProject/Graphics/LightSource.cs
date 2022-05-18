using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.Graphics
{
    public class LightSource : Module
    {
        public enum LightType
        {
            Directional, Point, Ambient
        }

        /// <summary>
        /// Интенсивность света
        /// </summary>
        public double Intensity { get; set; }

        /// <summary>
        /// Вектор направления света (только для направленного света)
        /// </summary>
        public Vector3 Vector { get; set; }

        /// <summary>
        /// Тип источника
        /// </summary>
        public LightType LightSourceType { get; set; }

        /// <summary>
        /// Максимальная дистанция распространения света
        /// </summary>
        public double MaxRange { get; set; }

        public LightSource(Transform carrier, LightType type, Vector3 vector, double intensity) : base(carrier)
        {
            Intensity = intensity;
            Vector = vector;
            LightSourceType = type;
        }
    }
}

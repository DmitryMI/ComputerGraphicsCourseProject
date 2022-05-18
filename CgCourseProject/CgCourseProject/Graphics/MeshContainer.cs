using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.Graphics
{
    /// <inheritdoc />
    /// <summary>
    /// Отвечает за отрисовку объемной фигуры
    /// </summary>
    public class MeshContainer : Module
    {
        public Mesh Mesh { get; }

        public MeshContainer(Transform carrier, Mesh mesh) : base(carrier)
        {
            Mesh = mesh;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Maths;

namespace CgCourseProject.Input
{
    public interface IInput
    {
        /// <summary>
        /// Получает информацию об указанном ключе управления
        /// </summary>
        /// <param name="axisName">Имя ключа управления</param>
        /// <returns>Степень активности от -1 до 1</returns>
        double GetAxis(string axisName);

        /// <summary>
        /// Определяет состояние оси в момент активации
        /// </summary>
        /// <param name="buttonName">Имя ключа</param>
        /// <returns>true, если ось перешла из неактивного состояния в активное на предыдущем кадре</returns>
        bool GetButtonPress(string buttonName);

        bool IsButtonUp(string buttonName);
        bool IsButtonDown(string buttonName);

        /// <summary>
        /// Положение курсора мыши (зависит от реализации)
        /// </summary>
        ScreenPoint MousePosition { get; }

        /// <summary>
        /// Положение курсора мыши относительно левого верхнего угла экрана
        /// </summary>
        ScreenPoint ScreenMousePosition { get; }

    }
}

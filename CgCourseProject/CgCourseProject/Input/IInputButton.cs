using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.Input
{
    public interface IInputButton
    {
        string Name { get; set; }
        bool IsDown { get; }
        bool IsUp { get; }
        bool IsPressed { get; }
    }
}

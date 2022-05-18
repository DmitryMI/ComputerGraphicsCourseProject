using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.Input
{
    public interface IInputAxis
    {
        string Name { get; set; }
        double Value { get; }
    }
}

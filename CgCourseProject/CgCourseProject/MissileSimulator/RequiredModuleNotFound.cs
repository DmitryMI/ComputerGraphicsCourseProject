using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.MissileSimulator
{
    class RequiredModuleNotFound : Exception
    {
        public RequiredModuleNotFound(string moduleName) : base(moduleName + " is required, but not found!")
        {
            
        }
    }
}

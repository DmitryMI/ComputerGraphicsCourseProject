using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.SceneManagement
{
    class TransformNotInstantiatedException : Exception
    {
        public TransformNotInstantiatedException() : base("Transform must be instantiated")
        {
            
        }
    }
}

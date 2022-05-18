using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.SceneManagement
{
    class ReaddingRestrictedException : Exception
    {
        public ReaddingRestrictedException() : base("Multiple adding of modules is restricted")
        {
            
        }
    }
}

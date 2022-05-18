using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgCourseProject.SceneManagement
{
    abstract class Logger
    {
        public abstract void Log(string message);
        public abstract void LogStatic(string text);

        public abstract void ClearLog();
        protected Logger()
        {

        }

        private static Logger _instance;

        public static void SetupDefaultLogger(Logger logger)
        {
            _instance = logger;
        }

        public static Logger GetInstance()
        {
            return _instance;
        }
    }
}

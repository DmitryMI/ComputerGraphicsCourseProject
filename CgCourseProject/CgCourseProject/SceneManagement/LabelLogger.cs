using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CgCourseProject.SceneManagement
{
    class LabelLogger : Logger
    {
        private Label _label;
        public LabelLogger(Label label)
        {
            _label = label;
        }

        public override void Log(string message)
        {

            _label.Text += message;
        }

        public override void LogStatic(string text)
        {
            _label.Text = text;
        }

        public override void ClearLog()
        {
            _label.Text = "";
        }
    }
}

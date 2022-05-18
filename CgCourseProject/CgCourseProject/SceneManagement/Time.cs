using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CgCourseProject.SceneManagement
{
    /// <summary>
    /// Управляет временем
    /// </summary>
    class Time
    {
        /// <summary>
        /// Интервал между кадрами в миллисекундах
        /// </summary>
        public const int MinimumFrameInterval = 20;

        public delegate void TimedAction();

        private static Time _timeInstance = null;

        public static Time GetInstance()
        {
            if(_timeInstance == null)
                _timeInstance = new Time();
            return _timeInstance;
        }
        public void PauseTime()
        {
            Paused = true;
        }

        public void ResumeTime()
        {
            Paused = false;
        }
    

        public void AttachTimedAction(TimedAction action)
        {
            _timeInstance._actions.Add(action);
        }

        public void DetachTimedAction(TimedAction action)
        {
            _timeInstance._actions.Remove(action);
        }

        private System.Windows.Forms.Timer _timer;
        private List<TimedAction> _actions;
        private bool _paused = false;
        private double _timePast = 0;
        private double _prevTime = 0;
        private double _deltaTime = 0;

        public bool Paused { get => _paused; private set { _paused = value; } }

        public double DeltaTime
        {
            get { return _deltaTime; }
        }

        private Time()
        {
            _actions = new List<TimedAction>();
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = MinimumFrameInterval;
            _timer.Tick += OnTimerElapsed;
            _timer.Start();
        }

        private void OnTimerElapsed(Object sender, EventArgs e)
        {
            _timePast += MinimumFrameInterval;
            
            if (!Paused)
            {
                _deltaTime = _timePast - _prevTime;
                _prevTime = _timePast;
                foreach (var action in _actions)
                {
                    action();
                }
            }
        }


    }
}

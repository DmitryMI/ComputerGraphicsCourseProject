using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using CgCourseProject.Physics;

namespace CgCourseProject.SceneManagement
{
    /// <summary>
    /// Присоединяемый к Transform модуль
    /// </summary>
    public abstract class Module
    {
        private Transform _carrier;
        private bool _initialized = false;

        public Module(Transform carrier)
        {
            _carrier = carrier;
            carrier.AddModule(this);
        }

        public Transform Carrier
        {
            get { return _carrier; }
        }

        /// <summary>
        /// Возвращает true, если метод Start ещё не был вызван
        /// </summary>
        public bool Initialized
        {
            get => _initialized;
        }

        public void OnInit()
        {
            _initialized = true;
        }

        /// <summary>
        /// Вызывается на следующем кадре после подключения к Transform
        /// </summary>
        public virtual void OnStart()
        {
            _initialized = true;
        }

        /// <summary>
        /// Вызывается каждый кадр после подключения к Transform
        /// </summary>
        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnCollision(BoxCollider other)
        {
            
        }

    }
}

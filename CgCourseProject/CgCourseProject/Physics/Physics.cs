using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.SceneManagement;

namespace CgCourseProject.Physics
{
    public class Physics
    {
        private static Physics _instance;
        public static Physics GetInstanse()
        {
            if(_instance == null)
                _instance = new Physics();
            return _instance;
        }

        private Physics()
        {
            Time.GetInstance().AttachTimedAction(OnUpdate);
        }

        private void OnUpdate()
        {
            if(Scene.GetOwningScene(this) == null) // TODO Optimize
                return;

            CheckCollisions();
        }

        private void CheckCollisions()
        {
            BoxCollider[] colliders = Scene.GetOwningScene(this).FindModulesOfType<BoxCollider>();

            for (int i = 0; i < colliders.Length; i++)
            {
                for (int j = i + 1; j < colliders.Length; j++)
                {
                    if (colliders[i].IsColliding(colliders[j]))
                    {
                        foreach (var m in colliders[i].Carrier.GetModules<Module>())
                        {
                            m.OnCollision(colliders[j]);
                        }

                        foreach (var m in colliders[j].Carrier.GetModules<Module>())
                        {
                            m.OnCollision(colliders[i]);
                        }
                    }
                }
            }
        }
    }
}

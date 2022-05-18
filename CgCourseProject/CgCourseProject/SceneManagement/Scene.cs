using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Graphics;
using CgCourseProject.Maths;

namespace CgCourseProject.SceneManagement
{
    /// <summary>
    /// Класс Scene отвечает за хранение всех объектов, вступающих во взаимодействие.
    /// </summary>
    class Scene
    {
        private List<Transform> _objects;
        private List<MeshContainer> _meshModules;
        private List<LightSource> _lightSources;
    
        private Camera _cameraModule;
        private Time.TimedAction _timedAction;
        

        private static List<Scene> _activeScenes;

        private static void AddActiveScene(Scene scene)
        {
            if (_activeScenes == null) 
                _activeScenes = new List<Scene>();

            if(!_activeScenes.Contains(scene))
                _activeScenes.Add(scene);
        }

        private static void RemoveActiveScene(Scene scene)
        {
            _activeScenes.Remove(scene);
        }

        public static Scene GetOwningScene(Transform transform)
        {
            foreach (var scene in _activeScenes)
            {
                if (scene._objects.Contains(transform))
                    return scene;
            }

            return null;
        }

        public static Scene GetOwningScene(Physics.Physics processor)
        {
            foreach (var scene in _activeScenes)
            {
                if (scene.PhysicsProcessor == processor)
                    return scene;
            }

            return null;
        }

        public Scene()
        {
            _objects = new List<Transform>();
            _meshModules = new List<MeshContainer>();
            _lightSources = new List<LightSource>();
            AddActiveScene(this);
            _timedAction = OnNewFrame;
            Time.GetInstance().AttachTimedAction(_timedAction);
        }

        public Physics.Physics PhysicsProcessor { get; set; }

        public void DestroyScene()
        {
            //_activeScenes.Remove(this);
            RemoveActiveScene(this);
            Time.GetInstance().DetachTimedAction(_timedAction);
            _objects.Clear();
            _meshModules.Clear();
            _cameraModule = null;
        }

        /// <summary>
        /// Adds an object to the scene and enebles it's processing
        /// </summary>
        /// <param name="obj">Object to be activated</param>
        public void Instantiate(Transform obj)
        {
            _objects.Add(obj);
            MeshContainer meshContainer = obj.GetModule<MeshContainer>();
            LightSource light = obj.GetModule<LightSource>();

            if(meshContainer != null)
                RegistrateMeshContainer(meshContainer);

            if(light != null)
                RegistrateLightSource(light);

            // Naming
            if (obj.Name == "")
            {
                obj.Name = obj.GetType().Name;
            }
        }

        /// <summary>
        /// Removes object from scene
        /// </summary>
        /// <param name="obj"></param>
        public void Destroy(Transform obj)
        {
            _objects.Remove(obj);

            MeshContainer meshContainer = obj.GetModule<MeshContainer>();
            LightSource lightSource = obj.GetModule<LightSource>();

            if (meshContainer != null)
                DeleteMeshContainer(meshContainer);

            if(lightSource != null)
                DeleteLightSource(lightSource);
        }

        /// <summary>
        /// Registrates camera module as rendering camera
        /// </summary>
        /// <param name="cameraModule"></param>
        public void RegistrateCamera(Camera cameraModule)
        {
            _cameraModule = cameraModule;
        }

        /// <summary>
        /// Any transform, that has mesh container will be registered
        /// </summary>
        /// <param name="meshContainer"></param>
        internal void RegistrateMeshContainer(MeshContainer meshContainer)
        {
            if(!_meshModules.Contains(meshContainer))
                _meshModules.Add(meshContainer);
        }

        internal void RegistrateLightSource(LightSource lightSource)
        {
            if(!_lightSources.Contains(lightSource))
                _lightSources.Add(lightSource);
        }

        internal void DeleteLightSource(LightSource lightSource)
        {
            _lightSources.Remove(lightSource);
        }

        internal void DeleteMeshContainer(MeshContainer meshContainer)
        {
            _meshModules.Remove(meshContainer);
        }


        /// <summary>
        /// Возвращает список всех объектов определённого типа
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <returns>Список объектов типа T</returns>
        public T FindObjectOfType<T>() where T : Transform
        {
            foreach (var obj in _objects)
            {
                if (obj is T)
                    return (T) obj;
            }

            return null;
        }

        /// <summary>
        /// Возвращает список всех модулей определённого типа, подключенных к объектам сцены
        /// </summary>
        /// <typeparam name="T">Тип модуля</typeparam>
        /// <returns>Список модулей</returns>
        public T[] FindModulesOfType<T>() where T : Module
        {
            List<T> modules = new List<T>();
            foreach (var obj in _objects)
            {
                modules.Add(obj.GetModule<T>());
            }

            return modules.ToArray();
        }

        private void OnNewFrame()
        {
            //Time.GetInstance().PauseTime();
            foreach (var transform in _objects)
            {
                if(transform.Enabled)
                    transform.OnUpdate();
            }

            DoRender();

            //Time.GetInstance().ResumeTime();
        }

        private void DoRender()
        {
            _cameraModule?.Render(_meshModules, _lightSources);
        }

        
    }
}

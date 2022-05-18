using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgCourseProject.Graphics;
using CgCourseProject.Maths;

namespace CgCourseProject.SceneManagement
{
    /// <summary>
    /// Базовый класс для всех объектов, размещаемых на сцене. Отвечает за расположение самого
    /// объекта и его дочерних объектов
    /// </summary>
    public class Transform
    {
        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _scale;
        private bool _enabled;

        private readonly List<Transform> _children;
        private readonly List<Module> _modules;
        private Transform _parent = null;
        private Matrix4 _transformationMatrix;
        private Matrix4 _scaleMatrix;
        private Matrix4 _rotationMatrix;
        private Matrix4 _translationMatrix;
        private Matrix4 _invRotationMatrix;
        private Vector3 _forward, _right, _up;

        public Transform()
        {
            _position = Vector3.Zero;
            _rotation = Quaternion.Identity;
            _scale = new Vector3(1, 1, 1);
            _children = new List<Transform>();
            _modules = new List<Module>();
            Enabled = true;
        }

        public void DetachFromParent()
        {
            _parent = null;
        }

        /// <summary>
        /// If object is disabled, it will not receive OnUpdate events
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public string Name { get; set; }

        public void AddChild(Transform child)
        {
            _children.Add(child);
            child.DetachFromParent();
            child.Parent = this;
        }

        public void RemoveChild(int index)
        {
            Transform child = _children[index];
            child.DetachFromParent();
            _children.RemoveAt(index);
        }

        public void RemoveChild(Transform child)
        {
            _children.Remove(child);
        }

        public void AddModule(Module module)
        {
            if (!_modules.Contains(module))
            {
                _modules.Add(module);

                Scene scene = Scene.GetOwningScene(this);

                if (scene != null)
                {

                    if (module is MeshContainer)
                        scene.RegistrateMeshContainer((MeshContainer) module);
                    else if (module is LightSource)
                        scene.RegistrateLightSource((LightSource) module);
                }
            }
            else
            {
                throw new ReaddingRestrictedException();
            }
        }

        public void RemoveModule(Module module)
        {
            _modules.Remove(module);

            if (module is MeshContainer)
                Scene.GetOwningScene(this).DeleteMeshContainer((MeshContainer)module);
            else if (module is LightSource)
                Scene.GetOwningScene(this).DeleteLightSource((LightSource)module);
        }

        public void RotateAround(Vector3 center, Vector3 axis, double angle)
        {

            Matrix4 matrix = Matrix4.GetRotationMatrix(axis, angle);

            Vector3 posTemp = Position - center;
            posTemp *= matrix;
            posTemp += center;

            Position = posTemp;
        }

        public Vector3 Forward => _forward;
        public Vector3 Right => _right;
        public Vector3 Up => _up;

        public T GetModule<T>() where T:Module
        {
            foreach (var module in _modules)
            {
                if (module is T)
                    return (T) module;
            }

            return null;
        }

        public T[] GetModules<T>() where T : Module
        {
            List<T> list = new List<T>();
            foreach (var module in _modules)
            {
                if (module is T)
                    list.Add((T)module);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Положение в сцене относительно начала координат
        /// </summary>
        public Vector3 Position
        {
            get => _position;
            set
            {
                Vector3 oldPos = _position;
                _position = value;
                ProcessChildrenPosition(_position - oldPos);
            }
        }

        /// <summary>
        /// Поворот, выраженный в углах эйлера
        /// </summary>
        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                Quaternion oldRot = _rotation;
                _rotation = value;
                ProcessChildrenRotation(_rotation - oldRot);
                _rotation.Normalize();
            }
        }

        public Vector3 Scale
        {
            get => _scale;
            set
            {
                Vector3 oldScale = _scale;
                _scale = value;
                ProcessChildrenScale(_scale - oldScale);
            }
        }

        /// <summary>
        /// Матрица, приводящая вектор в положение этого объекта (использовать при рендеринге, перевычисляется один раз за кадр)
        /// </summary>
        public Matrix4 TranformationMatrix => _transformationMatrix;

        /// <summary>
        /// Матрица, приводящая объект к системе координат в центре на текущем объекте (использовать при рендеринге, перевычисляется один раз за кадр)
        /// </summary>
        public Matrix4 InvertedRotationMatrix => _invRotationMatrix;

        public Matrix4 TranslationMatrix => _translationMatrix;
        public Matrix4 RotationMatrix => _rotationMatrix;
        public Matrix4 ScaleMatrix => _scaleMatrix;


        private void ProcessChildrenRotation(Quaternion rotationShift)
        {
            // TODO Process rotation
        }

        private void ProcessChildrenPosition(Vector3 positionShift)
        {
            // Shift children position
            foreach (var child in _children)
            {
                child.Position += positionShift;
            }
        }

        private void ProcessChildrenScale(Vector3 scaleShift)
        {
            // Shift children position
            foreach (var child in _children)
            {
                child.Scale += scaleShift;
            }
        }

        public virtual Transform Clone()
        {
            Transform clone = new Transform
            {
                _position = _position,
                _rotation = _rotation
            };
            foreach (var child in _children)
            {
                clone.AddChild(child);
            }

            return clone;
        }

        /// <summary>
        /// Делает так, чтобы объект смотрел на точку. Ориентация по оси Z сохраняется
        /// </summary>
        /// <param name="position">Точка, в которую нужно смотреть</param>
        public void LookAt(Vector3 position)
        {
            Rotation = Quaternion.LookAt(Position, position);
        }
        
        public void OnUpdate()
        {
            _rotationMatrix = Matrix4.GetRotationMatrix(Rotation);
            _translationMatrix = Matrix4.GetTranslationMatrix(Position);
            _scaleMatrix = Matrix4.GetScaleMatrix(Scale);
            _transformationMatrix = _translationMatrix * _scaleMatrix * _rotationMatrix;
            _invRotationMatrix = _rotationMatrix.Transposed();

            _forward = new Vector3(1, 0, 0) * _rotationMatrix;
            _right = new Vector3(0, 0, 1) * _rotationMatrix;
            _up = new Vector3(0, 1, 0) * _rotationMatrix;

            foreach (var module in _modules)
            {
                if (module.Initialized)
                    module.OnUpdate();
                else
                {
                    module.OnStart();
                    module.OnInit();
                }
            }
        }
    }
}

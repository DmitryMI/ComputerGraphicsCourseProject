using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CgCourseProject.Graphics;
using CgCourseProject.Input;
using CgCourseProject.Maths;
using CgCourseProject.MissileSimulator;
using CgCourseProject.Physics;
using CgCourseProject.SceneManagement;

namespace CgCourseProject
{
    public partial class SceneViewer : Form
    {
        public const double Deg2Rad = Math.PI / 180;

        private Scene _mainScene;
        private Transform _camTransform;
        private Transform _cubeTransform;

        public SceneViewer()
        {
            InitializeComponent();
        }

        private void SceneViewer_Load(object sender, EventArgs e)
        {
            Logger.SetupDefaultLogger(new LabelLogger(LogLabel));

            Scene mainScene = new Scene();
            _mainScene = mainScene;

            Transform cameraTransform = new Transform();
            _camTransform = cameraTransform;
            cameraTransform.Position = new Vector3(0, 0, -20);
            WindowsFormsCamera cameraModule = new WindowsFormsCamera(cameraTransform, Canvas);
            cameraModule.UseVertexNormals = true;
            
            mainScene.Instantiate(cameraTransform);
            mainScene.RegistrateCamera(cameraModule);

            Transform auxCube = new Transform();
            auxCube.Position = new Vector3(-5, 0, 0);
            new MeshContainer(auxCube, MeshBuilder.FromFileTrianglesOnly("Cube.obj"));
            mainScene.Instantiate(auxCube);

            // Creating cube
            Transform someObject = new Transform();
            someObject.Position = new Vector3(0, 0, 0);
            someObject.Rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            mainScene.Instantiate(someObject);
            _cubeTransform = someObject;
            
            Mesh mesh = MeshBuilder.FromFileTrianglesOnly("Cube.obj");
            //Mesh mesh = MeshBuilder.TestColoredCube(2);
            MeshContainer meshContainer = new MeshContainer(someObject, mesh);
            Rigidbody rig = new Rigidbody(someObject);
            rig.AddTorque(new Vector3(1, 1, 1));
            rig.AddForce(new Vector3(1, 1, 1));
            //AirTarget airTarget = new AirTarget(someObject);
            
            // Light source
            Transform lightSourceTransform = new Transform();
            lightSourceTransform.Position = new Vector3(15, 15, 15);
            LightSource lightSource = new LightSource(lightSourceTransform,
                LightSource.LightType.Directional, Vector3.Zero, 0.5);
            mainScene.Instantiate(lightSourceTransform);

            // Ambient light
            Transform ambientLightObject = new Transform();
            ambientLightObject.Position = new Vector3(0, 0, 0);
            LightSource ambientLight = new LightSource(ambientLightObject,
                LightSource.LightType.Ambient, Vector3.Zero, 0.1);
            mainScene.Instantiate(ambientLightObject);

            IInputAxis[] keys =
            {
                new WindowsKeyboardInput("Vertical", WindowsKeyboardInput.KeyCode.W, WindowsKeyboardInput.KeyCode.S),
                new WindowsKeyboardInput("Horizontal",  WindowsKeyboardInput.KeyCode.A, WindowsKeyboardInput.KeyCode.D),
                new WindowsMouseInput("MouseX", WindowsMouseInput.MouseAxis.X),
                new WindowsMouseInput("MouseY", WindowsMouseInput.MouseAxis.Y)
            };

            IInputButton[] keyButtons =
            {
                new WindowsKeyboardInput("Space", WindowsKeyboardInput.KeyCode.Space)
            };

            WindowsFormsInput input = new WindowsFormsInput(Canvas, keys, keyButtons);

            InputManager.DefaultInput = input;

            CameraController camController = new CameraController(cameraTransform, "Space", _cubeTransform, auxCube);
            
            // TESTING ROTATION AROUND
            new OrbitMover(auxCube, someObject);

            Time.GetInstance().AttachTimedAction(GlobalUpdate);
        }

        private void GlobalUpdate()
        {
            
        }
      
    }
}

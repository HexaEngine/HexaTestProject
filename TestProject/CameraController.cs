#nullable disable

namespace TestProject
{
    using BepuPhysics.Collidables;
    using BepuPhysics.CollisionDetection;
    using HexaEngine.Core;
    using HexaEngine.Core.Editor.Attributes;
    using HexaEngine.Core.Input;
    using HexaEngine.Core.Physics;
    using HexaEngine.Core.Physics.Characters;
    using HexaEngine.Core.Scenes;
    using HexaEngine.Core.Scripts;
    using System.Numerics;

    public class CameraController : IScriptBehaviour, IContactEventHandler
    {
        private Scene scene;
        private PhysicsSystem physics;
        private CharacterInput input;

        private Camera camera;

        public GameObject GameObject { get; set; }

        [EditorProperty]
        public float Speed { get; set; } = 100F;

        [EditorProperty]
        public float AngluarSpeed { get; set; } = 15F;

        public void Awake()
        {
            camera = GameObject as Camera;
            scene = GameObject.GetScene();
            physics = scene.GetRequiredSystem<PhysicsSystem>();
            input = new(physics.CharacterControllers, GameObject.Transform.GlobalPosition, new Capsule(0.5f, 1f), 0.1f, 1, 10, 100, 10, Speed);
            Application.MainWindow.LockCursor = true;
            physics.ContactEvents.Register(input.BodyHandle, this);
        }

        public void Update()
        {
            Vector2 delta = Mouse.Delta * Time.Delta;

            if (Gamepads.Controllers.Count > 0)
            {
                var gamepad = Gamepads.Controllers[0];
                var lx = (gamepad.AxisStates[GamepadAxis.RightX] + short.MaxValue) / (float)short.MaxValue - 1;
                var ly = (gamepad.AxisStates[GamepadAxis.RightY] + short.MaxValue) / (float)short.MaxValue - 1;
                delta += new Vector2(lx, ly) * Time.Delta * 5f;
            }

            if (delta.X != 0 || delta.Y != 0)
            {
                var re = new Vector3(delta.X, delta.Y, 0) * AngluarSpeed;
                camera.Transform.Rotation += re;
                if (camera.Transform.Rotation.Y < 270 & camera.Transform.Rotation.Y > 180)
                {
                    camera.Transform.Rotation = new Vector3(camera.Transform.Rotation.X, 270f, camera.Transform.Rotation.Z);
                }
                if (camera.Transform.Rotation.Y > 90 & camera.Transform.Rotation.Y < 270)
                {
                    camera.Transform.Rotation = new Vector3(camera.Transform.Rotation.X, 90f, camera.Transform.Rotation.Z);
                }
            }

            input.UpdateCharacterGoals(camera, Time.Delta);
            input.UpdateCameraPosition(camera, 0);
        }

        public void Destroy()
        {
            physics.ContactEvents.Unregister(input.BodyHandle);
            input.Dispose();
            Application.MainWindow.LockCursor = false;
        }

        public void OnStartedTouching<TManifold>(CollidableReference eventSource, CollidablePair pair, ref TManifold contactManifold, int workerIndex) where TManifold : unmanaged, IContactManifold<TManifold>
        {
            var normal = contactManifold.GetNormal(ref contactManifold, 0);

            if (Gamepads.Controllers.Count > 0)
            {
                var gamepad = Gamepads.Controllers[0];
                gamepad.Rumble((ushort)(ushort.MaxValue * normal.Y), (ushort)(ushort.MaxValue * normal.Y), 100);
            }
        }
    }
}
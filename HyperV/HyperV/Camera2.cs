using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HyperV
{
    public class Camera2 : Camera
    {
        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        const float ACCELERATION = 0.001f;
        const float INITIAL_ROTATION_SPEED = 5f;
        const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
        const float TRANSLATION_INITIAL_SPEED = 0.5f;
        const float DELTA_YAW = MathHelper.Pi / 180; // 1 degree at a time
        const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degree at a time
        const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degree at a time
        const float RAYON_COLLISION = 1f;
        const int CHARACTER_HEIGHT = -6;

        Vector3 Direction { get; set; }
        Vector3 Lateral { get; set; }
        Maze Maze { get; set; }
        Walls Walls { get; set; }
        float TranslationSpeed { get; set; }
        float SpeedRotation { get; set; }
        Point PreviousMousePosition { get; set; }
        Point CurrentMousePosition { get; set; }
        Vector2 MouseDisplacement { get; set; }

        float UpdateInterval { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
        InputManager InputMgr { get; set; }
        float Height { get; set; }
        List<Character> Characters { get; set; }

        public Camera2(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval) : base(game)
        {
            UpdateInterval = updateInterval;
            CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLAN_DISTANCE, FAR_PLANE_DISTANCE);
            CreateLookAt(positionCamera, target, orientation);
            Height = positionCamera.Y;
        }

        public override void Initialize()
        {
            SpeedRotation = INITIAL_ROTATION_SPEED;
            TranslationSpeed = TRANSLATION_INITIAL_SPEED;
            TimeElapsedSinceUpdate = 0;
            base.Initialize();
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Maze = Game.Services.GetService(typeof(Maze)) as Maze;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
        }

        protected override void CreateLookAt()
        {
            Vector3.Normalize(Direction);
            Vector3.Normalize(VerticalOrientation);
            Vector3.Normalize(Lateral);

            View = Matrix.CreateLookAt(Position, Position + Direction, VerticalOrientation);
        }

        protected override void CreateLookAt(Vector3 position, Vector3 target, Vector3 orientation)
        {
            Position = position;
            Target = target;
            VerticalOrientation = orientation;

            Direction = target - Position;
            //Direction = target;

            Vector3.Normalize(Target);

            CreateLookAt();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += elapsedTime;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                MouseFunctions();
                KeyboardFunctions();

                ManageHeight();
                CreateLookAt();




                Game.Window.Title = Position.ToString();
                Position = new Vector3(Position.X, Height, Position.Z);
                TimeElapsedSinceUpdate = 0;
            }
            base.Update(gameTime);
        }

        //Mouse
        #region
        private void MouseFunctions()
        {
            PreviousMousePosition = CurrentMousePosition;
            CurrentMousePosition = InputMgr.GetPositionMouse();
            MouseDisplacement = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
                                            CurrentMousePosition.Y - PreviousMousePosition.Y);

            ManageRotationMouse();

            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

        }

        private void ManageRotationMouse()
        {
            ManageMouseYaw();
            ManageMousePitch();
        }

        private void ManageMouseYaw()
        {
            Matrix yawMatrix = Matrix.Identity;

            yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -MouseDisplacement.X);

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void ManageMousePitch()
        {
            Matrix rollMatrix = Matrix.Identity;

            rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -MouseDisplacement.Y);

            Direction = Vector3.Transform(Direction, rollMatrix);
        }
        #endregion

        //Keyboard
        #region
        private void KeyboardFunctions()
        {
            ManageDisplacement();
            ManageKeyboardRotation();
        }

        private void ManageDisplacement()
        {
            float displacementDirection = (ManageKey(Keys.W) - ManageKey(Keys.S)) * TranslationSpeed;
            float displacementLateral = (ManageKey(Keys.A) - ManageKey(Keys.D)) * TranslationSpeed;

            Direction = Vector3.Normalize(Direction);
            Lateral = Vector3.Cross(Direction, VerticalOrientation);
            Position += displacementDirection * Direction;
            Position -= displacementLateral * Lateral;
            if (Maze.CheckForCollisions(Position))
            {
                Position -= displacementDirection * Direction;
                Position += displacementLateral * Lateral;
            }
        }

        const float MAX_DISTANCE = 4.5f;

        bool CheckForCharacterCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Characters.Count && !result; ++i)
            {
                result = Vector3.Distance(Characters[i].GetPosition(), Position) < MAX_DISTANCE;
            }

            return result;
        }

        private void ManageKeyboardRotation()
        {
            ManageKeyboardYaw();
            ManageKeyboardPitch();
        }

        private void ManageKeyboardYaw()
        {
            Matrix yawMatrix = Matrix.Identity;

            if (InputMgr.IsPressed(Keys.Left))
            {
                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED);
            }
            if (InputMgr.IsPressed(Keys.Right))
            {
                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, -DELTA_YAW * INITIAL_ROTATION_SPEED);
            }

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void ManageKeyboardPitch()
        {
            Matrix rollMatrix = Matrix.Identity;

            if (InputMgr.IsPressed(Keys.Down))
            {
                rollMatrix = Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL * INITIAL_ROTATION_SPEED);
            }
            if (InputMgr.IsPressed(Keys.Up))
            {
                rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED);
            }

            Direction = Vector3.Transform(Direction, rollMatrix);
        }
        #endregion

        private void ManageHeight()
        {
            Position = Maze.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
        }

        private int ManageKey(Keys key)
        {
            return InputMgr.IsPressed(key) ? 1 : 0;
        }
    }
}


using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HyperV
{
    public class PlayerCamera : Camera
    {
        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        const float ACCELERATION = 0.001f;
        const float INITIAL_ROTATION_SPEED = 5f;
        const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
        const float TRANSLATION_INITIAL_SPEED = 0.5f;
        const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_PITCH = MathHelper.Pi / 180; // 1 degré à la fois
        const float COLLISION_RADIUS = 1f;
        const int CHARACTER_HEIGHT = 10;

        Vector3 Direction { get; set; }
        Vector3 Lateral { get; set; }
        Grass Grass { get; set; }
        float TranslationSpeed { get; set; }
        float SpeedRotation { get; set; }
        Point PreviousMousePosition { get; set; }
        Point CurrentMousePosition { get; set; }
        Vector2 DisplacementMouse { get; set; }

        float UpdateInterval { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
        InputManager InputMgr { get; set; }

        public PlayerCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation, float updateInterval)
           : base(game)
        {
            UpdateInterval = updateInterval;
            CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
            CreateLookAt(cameraPosition, target, orientation);
        }

        public override void Initialize()
        {
            SpeedRotation = INITIAL_ROTATION_SPEED;
            TranslationSpeed = TRANSLATION_INITIAL_SPEED;
            TimeElapsedSinceUpdate = 0;
            base.Initialize();
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Grass = Game.Services.GetService(typeof(Grass)) as Grass;
            CurrentMousePosition = InputMgr.GetPositionMouse();
            PreviousMousePosition = InputMgr.GetPositionMouse();
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

            Vector3.Normalize(Target);

            CreateLookAt();
        }

        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += timeElapsed;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                MouseFunctions();
                KeyboardFunctions();
                
                ManageHeight();
                CreateLookAt();

                Game.Window.Title = Position.ToString();

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
            DisplacementMouse = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
                                            CurrentMousePosition.Y - PreviousMousePosition.Y);

            ManageMouseRotation();

            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

        }

        private void ManageMouseRotation()
        {
            ManageMouseYaw();
            ManageMouseRoll();
        }

        private void ManageMouseYaw()
        {
            Matrix yawMatrix = Matrix.Identity;

            yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.X);

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void ManageMouseRoll()
        {
            Matrix rollMatrix = Matrix.Identity;

            rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.Y);

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
            float lateralDisplacement = (ManageKey(Keys.A) - ManageKey(Keys.D)) * TranslationSpeed;

            Direction = Vector3.Normalize(Direction);
            Position += displacementDirection * Direction;

            Lateral = Vector3.Cross(Direction, VerticalOrientation);
            Position -= lateralDisplacement * Lateral;
        }

        private void ManageKeyboardRotation()
        {
            ManageKeyboardYaw();
            ManageKeyboardRoll();
        }

        private void ManageKeyboardYaw()
        {
            Matrix yawMatrix = Matrix.Identity;

            if (InputMgr.IsPressed(Keys.Left))
            {
                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW*INITIAL_ROTATION_SPEED);
            }
            if(InputMgr.IsPressed(Keys.Right))
            {
                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, -DELTA_YAW* INITIAL_ROTATION_SPEED);
            }

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void ManageKeyboardRoll()
        {
            Matrix rollMatrix = Matrix.Identity;

            if (InputMgr.IsPressed(Keys.Down))
            {
                rollMatrix = Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL* INITIAL_ROTATION_SPEED);
            }
            if(InputMgr.IsPressed(Keys.Up))
            {
                rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL* INITIAL_ROTATION_SPEED);
            }

            Direction = Vector3.Transform(Direction, rollMatrix);
        }
        #endregion

        private void ManageHeight()
        {
            Position = Grass.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
        }
        private int ManageKey(Keys touche)
        {
            return InputMgr.IsPressed(touche) ? 1 : 0;
        }

    }
}

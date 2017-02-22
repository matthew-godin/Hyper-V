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
        //Grass Grass { get; set; }
        float TranslationSpeed { get; set; }
        float SpeedRotation { get; set; }
        Point PreviousMousePosition { get; set; }
        Point CurrentMousePosition { get; set; }
        Vector2 DéplacementMouse { get; set; }

        float UpdateInterval { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
        InputManager InputMgr { get; set; }

        public PlayerCamera(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval)
           : base(game)
        {
            UpdateInterval = updateInterval;
            CréerVolumeDeVisualisation(OBJECTIVE_OPENNESS, DISTANCE_PLAN_RAPPROCHÉ, FAR_PLANE_DISTANCE);
            CréerPointDeView(positionCamera, target, orientation);
        }

        public override void Initialize()
        {
            SpeedRotation = INITIAL_ROTATION_SPEED;
            TranslationSpeed = TRANSLATION_INITIAL_SPEED;
            TimeElapsedSinceUpdate = 0;
            base.Initialize();
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            //Grass = Game.Services.GetService(typeof(Grass)) as Grass;
            CurrentMousePosition = InputMgr.GetPositionMouse();
            PreviousMousePosition = InputMgr.GetPositionMouse();
        }

        protected override void CréerPointDeView()
        {
            Vector3.Normalize(Direction);
            Vector3.Normalize(VerticalOrientation);
            Vector3.Normalize(Lateral);

            View = Matrix.CreateLookAt(Position, Position + Direction, VerticalOrientation);

        }

        protected override void CréerPointDeView(Vector3 position, Vector3 target, Vector3 orientation)
        {
            Position = position;
            Target = target;
            VerticalOrientation = orientation;

            Direction = target - Position;

            Vector3.Normalize(Target);

            CréerPointDeView();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += elapsedTime;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                MouseFunctions();
                KeyboardFunctions();

                //ManageHeight();
                CréerPointDeView();

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
            DéplacementMouse = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
                                            CurrentMousePosition.Y - PreviousMousePosition.Y);

            ManageRotationMouse();

            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

        }

        private void ManageRotationMouse()
        {
            ManageYawMouse();
            ManagePitchMouse();
        }

        private void ManageYawMouse()
        {
            Matrix yawMatrix = Matrix.Identity;

            yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -DéplacementMouse.X);

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void ManagePitchMouse()
        {
            Matrix rollMatrix = Matrix.Identity;

            rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -DéplacementMouse.Y);

            Direction = Vector3.Transform(Direction, rollMatrix);
        }
        #endregion

        //Keyboard
        #region
        private void KeyboardFunctions()
        {
            ManageDisplacement();
            ManageRotationKeyboard();
        }

        private void ManageDisplacement()
        {
            float displacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * TranslationSpeed;
            float displacementLateral = (GérerTouche(Keys.A) - GérerTouche(Keys.D)) * TranslationSpeed;

            Direction = Vector3.Normalize(Direction);
            Position += displacementDirection * Direction;

            Lateral = Vector3.Cross(Direction, VerticalOrientation);
            Position -= displacementLateral * Lateral;
        }

        private void ManageRotationKeyboard()
        {
            ManageYawKeyboard();
            ManagePitchKeyboard();
        }

        private void ManageYawKeyboard()
        {
            Matrix yawMatrix = Matrix.Identity;

            if (InputMgr.IsPressede(Keys.Left))
            {
                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED);
            }
            if (InputMgr.IsPressede(Keys.Right))
            {
                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, -DELTA_YAW * INITIAL_ROTATION_SPEED);
            }

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void ManagePitchKeyboard()
        {
            Matrix rollMatrix = Matrix.Identity;

            if (InputMgr.IsPressede(Keys.Down))
            {
                rollMatrix = Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL * INITIAL_ROTATION_SPEED);
            }
            if (InputMgr.IsPressede(Keys.Up))
            {
                rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED);
            }

            Direction = Vector3.Transform(Direction, rollMatrix);
        }
        #endregion

        private void ManageHeight()
        {
            //Position = Grass.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
        }
        private int GérerTouche(Keys touche)
        {
            return InputMgr.IsPressede(touche) ? 1 : 0;
        }

    }
}

using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace HyperV
{
    public class PlayerCamera : Camera
    {
        const int NUM_SECONDS_IN_ONE_MINUTE = 60;
        const int MAXIMAL_RUN_FACTOR = 4;
        const int MINIMAL_DISTANCE_POUR_RAMASSAGE = 45;
        const int JUMP_HEIGHT = 10;
        const int JUMP = 25;
        const int GAMEPAD_VECTOR_DISPLACEMENT_VALUE = 35;
        const float STANDARD_UPDATE_INTERVAL_JUMP = 1f / 60f;
        const float SPEED_WHEN_TIRED = 0.1f;
        const float INITIAL_ROTATION_SPEED = 5f;
        const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
        protected const float TRANSLATION_INITIAL_SPEED = 0.5f;
        const float DELTA_YAW = MathHelper.Pi / 180; 
        const float DELTA_ROLL = MathHelper.Pi / 180; 


        //CONSTRUCTOR
        readonly float UpdateInterval;
        protected float BaseHeight { get; set; }
        Vector2 Origin { get; set; }
        //CreateLookAt
        public Vector3 Direction { get; private set; }
        public Vector3 Lateral { get; private set; }


        //INITIALIZE
        //Mouse
        Point PreviousMousePosition { get; set; }
        Point CurrentMousePosition { get; set; }
        public Vector2 MouseDisplacementStickGamePad { get; private set; }//This protection if for the catapult
        //Displacement
        protected float TranslationSpeed { get; set; }
        //Player actions
        protected bool Jump { get; private set; }
        bool Run { get; set; }
        bool Grab { get; set; }
        //Activated
        protected bool DeactivateCertainCommands { get; set; } //Needed for catapult level
        public bool IsMouseCameraActivated { get; set; }
        public bool IsKeyboardCameraActivated { get; set; }
        public bool AreDisplacementKeyboardCommandsActivated { get; set; }
        public bool IsDead { get; private set; }
        //Autres
        public Ray Visor { get; private set; }
        float TimeElapsedSinceUpdate { get; set; }
        //*Jump*
        bool ContinueJump { get; set; }
        float t { get; set; }
        protected float Height { get; set; }
        Vector3 ControlPositionPts { get; set; }
        Vector3 ControlPositionPtsPlusUn { get; set; }
        Vector3[] ControlPts { get; set; }


        //LoadContent
        InputManager InputMgr { get; set; }
        GamePadManager GamePadMgr { get; set; }
        LifeBar[] LifeBars { get; set; }


        public PlayerCamera(Game game, Vector3 cameraPosition, Vector3 target,
                            Vector3 orientation, float updateInterval, float renderDistance)
            : base(game)
        {
            CreateLookAt(cameraPosition, target, orientation);
            UpdateInterval = updateInterval;
            FarPlaneDistance = renderDistance;
            CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, FarPlaneDistance);

            BaseHeight = Position.Y;
            Origin = new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height) / 2;
        }

        public override void Initialize()
        {
            //Mouse
            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
            MouseDisplacementStickGamePad = Vector2.Zero;

            //Displacement
            TranslationSpeed = TRANSLATION_INITIAL_SPEED;

            //Player actions
            Run = false;
            Jump = false;
            Grab = false;

            //Activated
            DeactivateCertainCommands = false;
            AreDisplacementKeyboardCommandsActivated = true;
            IsKeyboardCameraActivated = true;
            IsMouseCameraActivated = true;
            IsDead = false;

            //Autres
            Visor = new Ray();
            
            TimeElapsedSinceUpdate = 0;

            //*Jump*
            ContinueJump = false;
            t = 0;
            Height = BaseHeight;
            InitializeComplexObjectsJump();

            base.Initialize();
            LoadContent();
        }

        protected virtual void LoadContent()
        {
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GamePadMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            LifeBars = Game.Services.GetService(typeof(LifeBar[])) as LifeBar[];
        }

        protected override void CreateLookAt()
        {
            Direction = Vector3.Normalize(Direction);
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

        public void EstablishRenderDistance(float renderDistance)
        {
            FarPlaneDistance = renderDistance;
            CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, FarPlaneDistance);
        }

        public void EstablishDirection(Vector3 direction)
        {
            Direction = direction;
        }

        public void Attack(int val)
        {
            LifeBars[0].Attack(val);
        }

        protected virtual void ManageLifeBars()
        {
            if (!LifeBars[1].Water)
            {
                if (Run && !LifeBars[1].Tired && (InputMgr.IsPressed(Keys.W) ||
                    InputMgr.IsPressed(Keys.A) || InputMgr.IsPressed(Keys.S) ||
                    InputMgr.IsPressed(Keys.D) || GamePadMgr.PositionThumbStickLeft.X != 0 ||
                    GamePadMgr.PositionThumbStickLeft.Y != 0))
                {
                    LifeBars[1].Attack();
                }
                else
                {
                    LifeBars[1].AttackNegative();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            PopulateCommands();
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += timeElapsed;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                PerformUpdate();
                TimeElapsedSinceUpdate = 0;
            }
            base.Update(gameTime);
        }

        protected virtual void PerformUpdate()
        {
            MouseFunctions();
            if (!DeactivateCertainCommands)
            {
                KeyboardFunctions();
            }
            GamePadFunctions();

            ManageHeight();
            CreateLookAt();


            ManageGrabbing();
            ManageRun();
            ManageJump();

            ManageLifeBars();
        }


        //Mouse
        #region
        private void MouseFunctions()
        {
            if (IsMouseCameraActivated)
            {
                PreviousMousePosition = CurrentMousePosition;
                CurrentMousePosition = InputMgr.GetPositionMouse();
                MouseDisplacementStickGamePad = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X, CurrentMousePosition.Y - PreviousMousePosition.Y);

                ManageMouseRotation();

                CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
            }
            else
            {
                Game.IsMouseVisible = true;
            }
        }

        private void ManageMouseRotation()
        {
            ManageMouseYaw();
            if (!DeactivateCertainCommands)
            {
                ManageMouseRoll();
            }
        }

        private void ManageMouseYaw()
        {
            Matrix yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -MouseDisplacementStickGamePad.X);

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void ManageMouseRoll()
        {
            Matrix rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -MouseDisplacementStickGamePad.Y);

            Direction = Vector3.Transform(Direction, rollMatrix);
        }
        #endregion

        //Keyboard
        #region
        private void KeyboardFunctions()
        {
            if (AreDisplacementKeyboardCommandsActivated)
            {
                ManageDisplacement((ManageKey(Keys.W) - ManageKey(Keys.S)),
                                (ManageKey(Keys.A) - ManageKey(Keys.D)));
            }
            if (IsKeyboardCameraActivated)
            {
                ManageKeyboardRotation();
            }
        }
 
        protected virtual void ManageDisplacement(float direction, float latéral)
        {
            float displacementDirection = direction * TranslationSpeed;
            float lateralDisplacement = latéral * TranslationSpeed;

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
                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED);
            }
            if (InputMgr.IsPressed(Keys.Right))
            {
                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, -DELTA_YAW * INITIAL_ROTATION_SPEED);
            }

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void ManageKeyboardRoll()
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

        //GamePad
        #region
        private void GamePadFunctions()
        {
            if (GamePadMgr.EstGamepadActivated)
            {
                ManageDisplacement(GamePadMgr.PositionThumbStickLeft.Y,
                                 -GamePadMgr.PositionThumbStickLeft.X);

                MouseDisplacementStickGamePad = new Vector2(GAMEPAD_VECTOR_DISPLACEMENT_VALUE,
                                                            -GAMEPAD_VECTOR_DISPLACEMENT_VALUE) * GamePadMgr.PositionThumbStickRight;
                ManageMouseRotation();//Works with previous variable, so Gamepad rotation as well 
            }
        }
        #endregion

        private void PopulateCommands()
        {
            Run = (InputMgr.IsPressed(Keys.RightShift) && AreDisplacementKeyboardCommandsActivated) ||
                      (InputMgr.IsPressed(Keys.LeftShift) && AreDisplacementKeyboardCommandsActivated) ||
                      GamePadMgr.PositionsGâchettes.X > 0;

            Jump = (InputMgr.IsPressed(Keys.Space) && AreDisplacementKeyboardCommandsActivated) ||
                     GamePadMgr.IsPressed(Buttons.A);

            Grab = InputMgr.IsNewLeftClick() ||
                       InputMgr.IsOldLeftClick() ||
                       InputMgr.IsNewKey(Keys.E) && AreDisplacementKeyboardCommandsActivated ||
                       GamePadMgr.IsNewButton(Buttons.RightStick) || Grab;
        }


        protected virtual void ManageHeight()
        {
            if (!ContinueJump)
            {
                Height = BaseHeight;
            }
            Position = new Vector3(Position.X, Height, Position.Z);
        }

        private int ManageKey(Keys touche)
        {
            return InputMgr.IsPressed(touche) ? 1 : 0;
        }

        private void ManageGrabbing()
        {
            Visor = new Ray(Position, Direction);

            foreach (GrabbableModel grabbableSphere in Game.Components.Where(component => component is GrabbableModel))
            {
                grabbableSphere.Grab = grabbableSphere.IsColliding(Visor) <= MINIMAL_DISTANCE_POUR_RAMASSAGE &&
                           grabbableSphere.IsColliding(Visor) != null && Grab;

                if (grabbableSphere.Grab && !grabbableSphere.Placed)
                {
                    if (!GrabbableModel.Taken)
                    {
                        grabbableSphere.IsGrabbed = true;
                        GrabbableModel.Taken = true;
                        break;
                    }
                    else 
                    {
                        grabbableSphere.IsGrabbed = false;
                        GrabbableModel.Taken = false;
                        break;
                    }
                }
            }
            Grab = false;
        }


        //Jump
        #region
        protected virtual void ManageJump()
        {
            if (Jump)
            {
                InitializeComplexObjectsJump();
                ContinueJump = true;
            }

            if (ContinueJump)
            {
                if (t > NUM_SECONDS_IN_ONE_MINUTE)
                {
                    InitializeComplexObjectsJump();
                    ContinueJump = false;
                    t = 0;
                }
                Height = ComputeBezier(t * STANDARD_UPDATE_INTERVAL_JUMP, ControlPts).Y;
                ++t;
            }
        }

        void InitializeComplexObjectsJump()
        {
            Position = new Vector3(Position.X, BaseHeight, Position.Z);
            ControlPositionPts = new Vector3(Position.X, Position.Y, Position.Z);
            ControlPositionPtsPlusUn = Position + Vector3.Normalize(new Vector3(Direction.X, 0, Direction.Z)) * JUMP;
            ControlPts = ComputeControlPoints();
        }

        private Vector3[] ComputeControlPoints()
        {
            Vector3[] pts = new Vector3[4];
            pts[0] = ControlPositionPts;
            pts[3] = ControlPositionPtsPlusUn;
            pts[1] = new Vector3(pts[0].X, pts[0].Y + JUMP_HEIGHT, pts[0].Z);
            pts[2] = new Vector3(pts[3].X, pts[3].Y + JUMP_HEIGHT, pts[3].Z);
            return pts;
        }

        private Vector3 ComputeBezier(float t, Vector3[] ControlPts)
        {
            float x = (1 - t);
            return ControlPts[0] * (x * x * x) +
                   3 * ControlPts[1] * t * (x * x) +
                   3 * ControlPts[2] * t * t * x +
                   ControlPts[3] * t * t * t;

        }
        #endregion

        

        private void ManageRun()
        {
            TranslationSpeed = LifeBars[1].Tired ? SPEED_WHEN_TIRED : Run ? (GamePadMgr.PositionsGâchettes.X > 0 ? GamePadMgr.PositionsGâchettes.X : 1) * MAXIMAL_RUN_FACTOR * TRANSLATION_INITIAL_SPEED : TRANSLATION_INITIAL_SPEED;
        }
    }
}

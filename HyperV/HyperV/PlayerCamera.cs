using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class PlayerCamera : Camera
    {
        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        const float ACCELERATION = 0.001f;
        const float INITIAL_ROTATION_SPEED = 5f;
        const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
        protected const float TRANSLATION_INITIAL_SPEED = 0.5f;
        const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_PITCH = MathHelper.Pi / 180; // 1 degré à la fois
        const float COLLISION_RADIUS = 1f;
        const int CHARACTER_HEIGHT = 10;
        const int MAXIMAL_RUN_FACTOR = 4;
        const int MINIMAL_DISTANCE_POUR_RAMASSAGE = 45;

        public Vector3 Direction { get; protected set; }//
        public Vector3 Lateral { get; private set; }//
        Grass Grass { get; set; }
        protected float TranslationSpeed { get; set; }
        float SpeedRotation { get; set; }
        Point PreviousMousePosition { get; set; }
        Point CurrentMousePosition { get; set; }
        public Vector2 DisplacementMouse { get; set; }

        protected bool DésactiverDisplacement { get; set; }
        protected float UpdateInterval { get; set; }
        protected float TimeElapsedSinceUpdate { get; set; }
        InputManager InputMgr { get; set; }
        GamePadManager GamePadMgr { get; set; }

        protected bool Jump { get; private set; }
        bool Run { get; set; }
        public bool Grab { get; set; }

        public bool IsMouseCameraActivated { get; set; }
        bool EstDisplacementEtAutresKeyboardActivated { get; set; }
        bool IsKeyboardCameraActivated { get; set; }

        public Ray Visor { get; private set; }

        protected float Height { get; set; }

        protected LifeBar[] LifeBars { get; set; }
        Vector2 Origin { get; set; }

        public PlayerCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation, float updateInterval, float renderDistance) : base(game)
        {
            FarPlaneDistance = renderDistance;
            UpdateInterval = updateInterval;
            CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, /*FAR_PLANE_DISTANCE*/FarPlaneDistance);
            CreateLookAt(cameraPosition, target, orientation);
            Height = cameraPosition.Y;
            Origin = new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height) / 2;
        }

        public float GetRenderDistance()
        {
            return FarPlaneDistance;
        }

        public void SetRenderDistance(float renderDistance)
        {
            FarPlaneDistance = renderDistance;
            CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, /*FAR_PLANE_DISTANCE*/FarPlaneDistance);
            //CreateLookAt(Position, Target, Orientation);
        }

        public void InitializeDirection(Vector3 direction)
        {
            Direction = direction;
        }

        public override void Initialize()
        {
            SpeedRotation = INITIAL_ROTATION_SPEED;
            TranslationSpeed = TRANSLATION_INITIAL_SPEED;
            TimeElapsedSinceUpdate = 0;

            EstDisplacementEtAutresKeyboardActivated = true;
            IsKeyboardCameraActivated = true;

            Run = false;
            Jump = false;
            Grab = false;
            ContinueJump= false;
            IsMouseCameraActivated = true;


            Visor = new Ray();

            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

            base.Initialize();
            LoadContent();

            InitializeComplexObjectsJump();
            Height = Height;//CHARACTER_HEIGHT;
        }

        protected virtual void LoadContent()
        {
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GamePadMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;

            LifeBars = Game.Services.GetService(typeof(LifeBar[])) as LifeBar[];
        }

        public bool Dead { get; private set; }

        public void Attack(int val)
        {
            LifeBars[0].Attack(val);
        }

        protected override void CreateLookAt()
        {
            Direction = Vector3.Normalize(Direction); // NEW FROM 4/7/2017 2:30 AM was only Vector3.Normalize(Direction); before ******************************************************************************************************************************************************************
            Vector3.Normalize(VerticalOrientation);
            Vector3.Normalize(Lateral);
            //Position -= new Vector3(Origin.X, 0, Origin.Y);

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
            AffectCommandsForGrab();
            ManageGrabbing();
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += timeElapsed;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                MouseFunctions();
                if (!DésactiverDisplacement)
                {
                    KeyboardFunctions();
                }
                GamePadFunctions();

                ManageHeight();
                CreateLookAt();

                PopulateCommands(); // Grab moved to AffectCommandsForGrab()

                //ManageGrabbing();
                ManageRun();
                ManageJump();

                ManageLifeBars();
                TimeElapsedSinceUpdate = 0;
            }
            base.Update(gameTime);
        }

        protected virtual void ManageLifeBars()
        {
            if (!LifeBars[1].Water)
            {
                if (Run && !LifeBars[1].Tired && (InputMgr.IsPressed(Keys.W) || InputMgr.IsPressed(Keys.A) || InputMgr.IsPressed(Keys.S) || InputMgr.IsPressed(Keys.D)))
                {
                    LifeBars[1].Attack();
                }
                else
                {
                    LifeBars[1].AttackNegative();
                }
            }
        }


        //Mouse
        #region
        private void MouseFunctions()
        {
            if (IsMouseCameraActivated)
            {
                PreviousMousePosition = CurrentMousePosition;
                CurrentMousePosition = InputMgr.GetPositionMouse();
                DisplacementMouse = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X, CurrentMousePosition.Y - PreviousMousePosition.Y);

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
            if (!DésactiverDisplacement)
            {
                ManageMouseRoll();
            }
        }

        private void ManageMouseYaw()
        {
            Matrix yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.X);

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void ManageMouseRoll()
        {
            Matrix rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.Y);

            Direction = Vector3.Transform(Direction, rollMatrix);
        }
        #endregion

        //Keyboard
        #region
        private void KeyboardFunctions()
        {
            if (EstDisplacementEtAutresKeyboardActivated)
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

                DisplacementMouse = new Vector2(35, -35) * GamePadMgr.PositionThumbStickRight;
                ManageMouseRotation();
            }
        }
        #endregion

        private void PopulateCommands()
        {
            Run = (InputMgr.IsPressed(Keys.RightShift) && EstDisplacementEtAutresKeyboardActivated) ||
                      (InputMgr.IsPressed(Keys.LeftShift) && EstDisplacementEtAutresKeyboardActivated) ||
                      GamePadMgr.PositionsGâchettes.X > 0;

            Jump = (InputMgr.IsPressed(Keys.R/*Keys.Space*/) && EstDisplacementEtAutresKeyboardActivated) ||
                     GamePadMgr.IsPressed(Buttons.A);

            //Grab = InputMgr.IsNewLeftClick() ||
            //           InputMgr.IsOldLeftClick() ||
            //           InputMgr.IsNewKey(Keys.E) && EstDisplacementEtAutresKeyboardActivated ||
            //           GamePadMgr.IsNewButton(Buttons.RightStick);
        }

        private void AffectCommandsForGrab()
        {
            Grab = InputMgr.IsNewLeftClick() ||
                       InputMgr.IsOldLeftClick() ||
                       InputMgr.IsNewKey(Keys.E) && EstDisplacementEtAutresKeyboardActivated ||
                       GamePadMgr.IsNewButton(Buttons.RightStick);
        }

        protected virtual void ManageHeight()
        {
            //Position = Grass.GetPositionWithHeight(Position, (int)Height);
            if (!ContinueJump)
            {
                Height = Height;
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
                    else if (grabbableSphere.IsGrabbed)
                    {
                        grabbableSphere.IsGrabbed = false;
                        GrabbableModel.Taken = false;
                        break;
                    }
                }
            }

            //NEW
            foreach (Arc grabbableSphere in Game.Components.Where(component => component is Arc))
            {
                grabbableSphere.Grab = grabbableSphere.IsColliding(Visor) <= MINIMAL_DISTANCE_POUR_RAMASSAGE &&
                           grabbableSphere.IsColliding(Visor) != null && Grab;
                
                if (grabbableSphere.Grab && !grabbableSphere.Placed)
                {
                    if (/*!GrabbableModel.Taken*/true)
                    {
                        grabbableSphere.IsGrabbed = true;
                        GrabbableModel.Taken = true;
                        break;
                    }
                    else if (grabbableSphere.IsGrabbed)
                    {
                        grabbableSphere.IsGrabbed = false;
                        GrabbableModel.Taken = false;
                        break;
                    }
                }
            }
            //NEW
        }

        //private bool Taken()
        //{
        //    bool result = false;
        //    foreach (GrabbableModel grabbableSphere in Game.Components.Where(component => component is GrabbableModel))
        //    {
        //        if (grabbableSphere.IsGrabbed && !grabbableSphere.Placed)
        //        {
        //            result = true;
        //            break;
        //        }
        //    }
        //    return result;
        //}

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
                if (t > 60)
                {
                    InitializeComplexObjectsJump();
                    ContinueJump = false;
                    t = 0;
                }
                Height = ComputeBezier(t * (1f / 60f), ControlPts).Y;
                ++t;
            }
        }

        bool ContinueJump { get; set; }
        float t { get; set; }
        protected float Height { get; set; }

        Vector3 ControlPositionPts { get; set; }
        Vector3 ControlPositionPtsPlusUn { get; set; }
        Vector3[] ControlPts { get; set; }

        void InitializeComplexObjectsJump()
        {
            Position = new Vector3(Position.X, Height/*CHARACTER_HEIGHT*/, Position.Z);
            ControlPositionPts = new Vector3(Position.X, Position.Y, Position.Z);
            ControlPositionPtsPlusUn = Position + Vector3.Normalize(new Vector3(Direction.X, 0, Direction.Z)) * 25;
            //Position = new Vector3(ControlPositionPts.X, ControlPositionPts.Y, ControlPositionPts.Z);//******
            //Direction = ControlPositionPtsPlusUn - ControlPositionPts;//******
            ControlPts = ComputeControlPoints();
        }

        private Vector3[] ComputeControlPoints()
        {
            Vector3[] pts = new Vector3[4];
            pts[0] = ControlPositionPts;
            pts[3] = ControlPositionPtsPlusUn;
            pts[1] = new Vector3(pts[0].X, pts[0].Y + 20, pts[0].Z);
            pts[2] = new Vector3(pts[3].X, pts[3].Y + 20, pts[3].Z);
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

        const float TIRED_SPEED = 0.1f;

        private void ManageRun()
        {
            TranslationSpeed = LifeBars[1].Tired ? TIRED_SPEED : Run ? (GamePadMgr.PositionsGâchettes.X > 0 ? GamePadMgr.PositionsGâchettes.X : 1) * MAXIMAL_RUN_FACTOR * TRANSLATION_INITIAL_SPEED : TRANSLATION_INITIAL_SPEED;
        }
    }
}

using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HyperV
{
    public class PlayerCamera : Camera
    {
        //const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        //const float ACCELERATION = 0.001f;
        //const float INITIAL_ROTATION_SPEED = 5f;
        //const float TRANSLATION_INITIAL_SPEED = 0.5f;
        //const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
        //const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
        //const float DELTA_PITCH = MathHelper.Pi / 180; // 1 degré à la fois
        //const float COLLISION_RADIUS = 1f;
        //const int CHARACTER_HEIGHT = 10;

        //Vector3 Direction { get; set; }
        //Vector3 Lateral { get; set; }
        //Grass Grass { get; set; }
        //float TranslationSpeed { get; set; }
        //float SpeedRotation { get; set; }

        //float UpdateInterval { get; set; }
        //float TimeElapsedSinceUpdate { get; set; }
        //InputManager InputMgr { get; set; }

        //bool inZoom;
        //bool InZoom
        //{
        //    get { return inZoom; }
        //    set
        //    {
        //        float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
        //        inZoom = value;
        //        if (inZoom)
        //        {
        //            CreateViewingFrustum(OBJECTIVE_OPENNESS / 2, aspectRatio, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
        //        }
        //        else
        //        {
        //            CreateViewingFrustum(OBJECTIVE_OPENNESS, aspectRatio, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
        //        }
        //    }
        //}

        //public PlayerCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation, float updateInterval)
        //   : base(game)
        //{
        //    UpdateInterval = updateInterval;
        //    CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
        //    CreateLookAt(cameraPosition, target, orientation);
        //    InZoom = false;
        //}

        //public override void Initialize()
        //{
        //    SpeedRotation = INITIAL_ROTATION_SPEED;
        //    TranslationSpeed = TRANSLATION_INITIAL_SPEED;
        //    TimeElapsedSinceUpdate = 0;
        //    base.Initialize();
        //    InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
        //    Grass = Game.Services.GetService(typeof(Grass)) as Grass;
        //}

        //protected override void CreateLookAt()
        //{
        //    Vector3.Normalize(Direction);
        //    Vector3.Normalize(VerticalOrientation);
        //    Vector3.Normalize(Lateral);

        //    View = Matrix.CreateLookAt(Position, Position + Direction, VerticalOrientation);

        //}

        //protected override void CreateLookAt(Vector3 position, Vector3 target, Vector3 orientation)
        //{
        //    Position = position;
        //    Target = target;
        //    VerticalOrientation = orientation;

        //    Direction = target - Position;

        //    Vector3.Normalize(Target);

        //    CreateLookAt();
        //}

        //public override void Update(GameTime gameTime)
        //{
        //    float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    TimeElapsedSinceUpdate += timeElapsed;
        //    KeyboardManagement();
        //    if (TimeElapsedSinceUpdate >= UpdateInterval)
        //    {
        //        if (InputMgr.IsPressed(Keys.LeftShift) || InputMgr.IsPressed(Keys.RightShift))
        //        {
        //            ManageAcceleration();
        //            ManageDisplacement();
        //            ManageRotation();
        //            CreateLookAt();
        //            ManageHeight();
        //            Game.Window.Title = Position.ToString();
        //        }
        //        TimeElapsedSinceUpdate = 0;
        //    }
        //    base.Update(gameTime);
        //}

        //private void ManageHeight()
        //{
        //    Position = Grass.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
        //}

        //#region
        //private int ManageKey(Keys touche)
        //{
        //    return InputMgr.IsPressed(touche) ? 1 : 0;
        //}

        //private void ManageAcceleration()
        //{
        //    int accelerationValue = (ManageKey(Keys.Subtract) + ManageKey(Keys.OemMinus)) - (ManageKey(Keys.Add) + ManageKey(Keys.OemPlus));
        //    if (accelerationValue != 0)
        //    {
        //        UpdateInterval += ACCELERATION * accelerationValue;
        //        UpdateInterval = MathHelper.Max(STANDARD_UPDATE_INTERVAL, UpdateInterval);
        //    }
        //}

        //private void ManageDisplacement()
        //{
        //    float displacementDirection = (ManageKey(Keys.W) - ManageKey(Keys.S)) * TranslationSpeed;
        //    float lateralDisplacement = (ManageKey(Keys.A) - ManageKey(Keys.D)) * TranslationSpeed;

        //    Direction = Vector3.Normalize(Direction);
        //    Position += displacementDirection * Direction;

        //    Lateral = Vector3.Cross(Direction, VerticalOrientation);
        //    Position -= lateralDisplacement * Lateral;
        //}

        //private void ManageRotation()
        //{
        //    ManageYaw();
        //    ManagePitch();
        //    ManageRoll();
        //}

        //private void ManageYaw()
        //{
        //    Matrix yawMatrix = Matrix.Identity;

        //    if (InputMgr.IsPressed(Keys.Left))
        //    {
        //        yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW*INITIAL_ROTATION_SPEED);
        //    }
        //    if(InputMgr.IsPressed(Keys.Right))
        //    {
        //        yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, -DELTA_YAW* INITIAL_ROTATION_SPEED);
        //    }

        //    Direction = Vector3.Transform(Direction, yawMatrix);
        //}

        //private void ManagePitch()
        //{
        //    Matrix rollMatrix = Matrix.Identity;

        //    if (InputMgr.IsPressed(Keys.Down))
        //    {
        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL* INITIAL_ROTATION_SPEED);
        //    }
        //    if(InputMgr.IsPressed(Keys.Up))
        //    {
        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL* INITIAL_ROTATION_SPEED);
        //    }

        //    Direction = Vector3.Transform(Direction, rollMatrix);
        //    //VerticalOrientation = Vector3.Transform(VerticalOrientation, rollMatrix);
        //}

        //private void ManageRoll()
        //{
        //    Matrix matriceRoulis = Matrix.Identity;

        //    if (InputMgr.IsPressed(Keys.PageUp))
        //    {
        //        matriceRoulis = Matrix.CreateFromAxisAngle(Direction, DELTA_PITCH* INITIAL_ROTATION_SPEED);
        //    }
        //    if(InputMgr.IsPressed(Keys.PageDown))
        //    {
        //        matriceRoulis = Matrix.CreateFromAxisAngle(Direction, -DELTA_PITCH* INITIAL_ROTATION_SPEED);
        //    }

        //    VerticalOrientation = Vector3.Transform(VerticalOrientation, matriceRoulis);
        //}

        //private void KeyboardManagement()
        //{
        //    if (InputMgr.IsNewKey(Keys.Z))
        //    {
        //        InZoom = !InZoom;
        //    }
        //}
        //#endregion


        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        const float ACCELERATION = 0.001f;
        const float INITIAL_ROTATION_SPEED = 5f;
        const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
        const float TRANSLATION_INITIAL_SPEED = 0.5f;
        const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_PITCH = MathHelper.Pi / 180; // 1 degré à la fois
        const float COLLISION_RADIUS = 1f;
        const int CHARACTER_HEIGHT = -6;

        Vector3 Direction { get; set; }
        Vector3 Lateral { get; set; }
        Maze Maze { get; set; }
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
            Maze = Game.Services.GetService(typeof(Maze)) as Maze;
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
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += timeElapsed;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                MouseFunctions();
                KeyboardFunctions();

                ManageHeight();
                CreateLookAt();




                Game.Window.Title = Position.ToString();
                Position = new Vector3(Position.X, 4, Position.Z);
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
            Lateral = Vector3.Cross(Direction, VerticalOrientation);
            Position += displacementDirection * Direction;
            Position -= lateralDisplacement * Lateral;
            if (Maze.CheckForCollisions(Position))
            {
                Position -= displacementDirection * Direction;
                Position += lateralDisplacement * Lateral;
            }
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

        private void ManageHeight()
        {
            Position = Maze.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
        }

        private int ManageKey(Keys touche)
        {
            return InputMgr.IsPressed(touche) ? 1 : 0;
        }
        //const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        //const float ACCELERATION = 0.001f;
        //const float INITIAL_ROTATION_SPEED = 5f;
        //const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
        //const float TRANSLATION_INITIAL_SPEED = 0.5f;
        //const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
        //const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
        //const float DELTA_PITCH = MathHelper.Pi / 180; // 1 degré à la fois
        //const float COLLISION_RADIUS = 1f;
        //const int CHARACTER_HEIGHT = 10;

        //Vector3 Direction { get; set; }
        //Vector3 Lateral { get; set; }
        //Grass Grass { get; set; }
        //float TranslationSpeed { get; set; }
        //float SpeedRotation { get; set; }
        //Point PreviousMousePosition { get; set; }
        //Point CurrentMousePosition { get; set; }
        //Vector2 DisplacementMouse { get; set; }

        //float UpdateInterval { get; set; }
        //float TimeElapsedSinceUpdate { get; set; }
        //InputManager InputMgr { get; set; }

        //public PlayerCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation, float updateInterval)
        //   : base(game)
        //{
        //    UpdateInterval = updateInterval;
        //    CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
        //    CreateLookAt(cameraPosition, target, orientation);
        //}

        //public override void Initialize()
        //{
        //    SpeedRotation = INITIAL_ROTATION_SPEED;
        //    TranslationSpeed = TRANSLATION_INITIAL_SPEED;
        //    TimeElapsedSinceUpdate = 0;
        //    base.Initialize();
        //    InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
        //    Grass = Game.Services.GetService(typeof(Grass)) as Grass;
        //    CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
        //    PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
        //    Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
        //}

        //protected override void CreateLookAt()
        //{
        //    Vector3.Normalize(Direction);
        //    Vector3.Normalize(VerticalOrientation);
        //    Vector3.Normalize(Lateral);

        //    View = Matrix.CreateLookAt(Position, Position + Direction, VerticalOrientation);
        //}

        //protected override void CreateLookAt(Vector3 position, Vector3 target, Vector3 orientation)
        //{
        //    Position = position;
        //    Target = target;
        //    VerticalOrientation = orientation;

        //    Direction = target - Position;

        //    Vector3.Normalize(Target);

        //    CreateLookAt();
        //}

        //public override void Update(GameTime gameTime)
        //{
        //    float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    TimeElapsedSinceUpdate += timeElapsed;
        //    if (TimeElapsedSinceUpdate >= UpdateInterval)
        //    {
        //        MouseFunctions();
        //        KeyboardFunctions();

        //        ManageHeight();
        //        CreateLookAt();


        //        ManageGrabbing();

        //        //Game.Window.Title = Position.ToString();

        //        TimeElapsedSinceUpdate = 0;
        //    }
        //    base.Update(gameTime);
        //}

        ////Mouse
        //#region
        //private void MouseFunctions()
        //{
        //    PreviousMousePosition = CurrentMousePosition;
        //    CurrentMousePosition = InputMgr.GetPositionMouse();
        //    DisplacementMouse = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
        //                                    CurrentMousePosition.Y - PreviousMousePosition.Y);

        //    ManageMouseRotation();

        //    CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
        //    Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

        //}

        //private void ManageMouseRotation()
        //{
        //    ManageMouseYaw();
        //    ManageMouseRoll();
        //}

        //private void ManageMouseYaw()
        //{
        //    Matrix yawMatrix = Matrix.Identity;

        //    yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.X);

        //    Direction = Vector3.Transform(Direction, yawMatrix);
        //}

        //private void ManageMouseRoll()
        //{
        //    Matrix rollMatrix = Matrix.Identity;

        //    rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.Y);

        //    Direction = Vector3.Transform(Direction, rollMatrix);
        //}
        //#endregion

        ////Keyboard
        //#region
        //private void KeyboardFunctions()
        //{
        //    ManageDisplacement();
        //    ManageKeyboardRotation();
        //}

        //private void ManageDisplacement()
        //{
        //    float displacementDirection = (ManageKey(Keys.W) - ManageKey(Keys.S)) * TranslationSpeed;
        //    float lateralDisplacement = (ManageKey(Keys.A) - ManageKey(Keys.D)) * TranslationSpeed;

        //    Direction = Vector3.Normalize(Direction);
        //    Position += displacementDirection * Direction;

        //    Lateral = Vector3.Cross(Direction, VerticalOrientation);
        //    Position -= lateralDisplacement * Lateral;
        //}

        //private void ManageKeyboardRotation()
        //{
        //    ManageKeyboardYaw();
        //    ManageKeyboardRoll();
        //}

        //private void ManageKeyboardYaw()
        //{
        //    Matrix yawMatrix = Matrix.Identity;

        //    if (InputMgr.IsPressed(Keys.Left))
        //    {
        //        yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED);
        //    }
        //    if (InputMgr.IsPressed(Keys.Right))
        //    {
        //        yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, -DELTA_YAW * INITIAL_ROTATION_SPEED);
        //    }

        //    Direction = Vector3.Transform(Direction, yawMatrix);
        //}

        //private void ManageKeyboardRoll()
        //{
        //    Matrix rollMatrix = Matrix.Identity;

        //    if (InputMgr.IsPressed(Keys.Down))
        //    {
        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL * INITIAL_ROTATION_SPEED);
        //    }
        //    if (InputMgr.IsPressed(Keys.Up))
        //    {
        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED);
        //    }

        //    Direction = Vector3.Transform(Direction, rollMatrix);
        //}
        //#endregion

        //private void ManageHeight()
        //{
        //    Position = Grass.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
        //}

        //private int ManageKey(Keys touche)
        //{
        //    return InputMgr.IsPressed(touche) ? 1 : 0;
        //}

        //private void ManageGrabbing()
        //{
        //    //Ray visor = new Ray(Position, Direction);

        //    //foreach (SphereRamassable grabbableSphere in Game.Components.Where(component => component is GrabbableSphere))
        //    //{
        //    //    Game.Window.Title = grabbableSphere.IsColliding(visor).ToString();
        //    //    //if (grabbableSphere.IsColliding(visor) != null)
        //    //    //{
        //    //    //    Game.Window.Title = "true";
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    Game.Window.Title = "false";
        //    //    //}
        //    //}
        //}
    }
}

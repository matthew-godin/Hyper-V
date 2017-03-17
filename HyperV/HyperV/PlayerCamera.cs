//using XNAProject;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using System.Collections.Generic;

//namespace HyperV
//{
//    public class PlayerCamera : Camera
//    {
//        //const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
//        //const float ACCELERATION = 0.001f;
//        //const float INITIAL_ROTATION_SPEED = 5f;
//        //const float TRANSLATION_INITIAL_SPEED = 0.5f;
//        //const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
//        //const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
//        //const float DELTA_PITCH = MathHelper.Pi / 180; // 1 degré à la fois
//        //const float COLLISION_RADIUS = 1f;
//        //const int CHARACTER_HEIGHT = 10;

//        //Vector3 Direction { get; set; }
//        //Vector3 Lateral { get; set; }
//        //Grass Grass { get; set; }
//        //float TranslationSpeed { get; set; }
//        //float SpeedRotation { get; set; }

//        //float UpdateInterval { get; set; }
//        //float TimeElapsedSinceUpdate { get; set; }
//        //InputManager InputMgr { get; set; }

//        //bool inZoom;
//        //bool InZoom
//        //{
//        //    get { return inZoom; }
//        //    set
//        //    {
//        //        float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
//        //        inZoom = value;
//        //        if (inZoom)
//        //        {
//        //            CreateViewingFrustum(OBJECTIVE_OPENNESS / 2, aspectRatio, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
//        //        }
//        //        else
//        //        {
//        //            CreateViewingFrustum(OBJECTIVE_OPENNESS, aspectRatio, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
//        //        }
//        //    }
//        //}

//        //public PlayerCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation, float updateInterval)
//        //   : base(game)
//        //{
//        //    UpdateInterval = updateInterval;
//        //    CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
//        //    CreateLookAt(cameraPosition, target, orientation);
//        //    InZoom = false;
//        //}

//        //public override void Initialize()
//        //{
//        //    SpeedRotation = INITIAL_ROTATION_SPEED;
//        //    TranslationSpeed = TRANSLATION_INITIAL_SPEED;
//        //    TimeElapsedSinceUpdate = 0;
//        //    base.Initialize();
//        //    InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
//        //    Grass = Game.Services.GetService(typeof(Grass)) as Grass;
//        //}

//        //protected override void CreateLookAt()
//        //{
//        //    Vector3.Normalize(Direction);
//        //    Vector3.Normalize(VerticalOrientation);
//        //    Vector3.Normalize(Lateral);

//        //    View = Matrix.CreateLookAt(Position, Position + Direction, VerticalOrientation);

//        //}

//        //protected override void CreateLookAt(Vector3 position, Vector3 target, Vector3 orientation)
//        //{
//        //    Position = position;
//        //    Target = target;
//        //    VerticalOrientation = orientation;

//        //    Direction = target - Position;

//        //    Vector3.Normalize(Target);

//        //    CreateLookAt();
//        //}

//        //public override void Update(GameTime gameTime)
//        //{
//        //    float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
//        //    TimeElapsedSinceUpdate += timeElapsed;
//        //    KeyboardManagement();
//        //    if (TimeElapsedSinceUpdate >= UpdateInterval)
//        //    {
//        //        if (InputMgr.IsPressed(Keys.LeftShift) || InputMgr.IsPressed(Keys.RightShift))
//        //        {
//        //            ManageAcceleration();
//        //            ManageDisplacement();
//        //            ManageRotation();
//        //            CreateLookAt();
//        //            ManageHeight();
//        //            Game.Window.Title = Position.ToString();
//        //        }
//        //        TimeElapsedSinceUpdate = 0;
//        //    }
//        //    base.Update(gameTime);
//        //}

//        //private void ManageHeight()
//        //{
//        //    Position = Grass.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
//        //}

//        //#region
//        //private int ManageKey(Keys touche)
//        //{
//        //    return InputMgr.IsPressed(touche) ? 1 : 0;
//        //}

//        //private void ManageAcceleration()
//        //{
//        //    int accelerationValue = (ManageKey(Keys.Subtract) + ManageKey(Keys.OemMinus)) - (ManageKey(Keys.Add) + ManageKey(Keys.OemPlus));
//        //    if (accelerationValue != 0)
//        //    {
//        //        UpdateInterval += ACCELERATION * accelerationValue;
//        //        UpdateInterval = MathHelper.Max(STANDARD_UPDATE_INTERVAL, UpdateInterval);
//        //    }
//        //}

//        //private void ManageDisplacement()
//        //{
//        //    float displacementDirection = (ManageKey(Keys.W) - ManageKey(Keys.S)) * TranslationSpeed;
//        //    float lateralDisplacement = (ManageKey(Keys.A) - ManageKey(Keys.D)) * TranslationSpeed;

//        //    Direction = Vector3.Normalize(Direction);
//        //    Position += displacementDirection * Direction;

//        //    Lateral = Vector3.Cross(Direction, VerticalOrientation);
//        //    Position -= lateralDisplacement * Lateral;
//        //}

//        //private void ManageRotation()
//        //{
//        //    ManageYaw();
//        //    ManagePitch();
//        //    ManageRoll();
//        //}

//        //private void ManageYaw()
//        //{
//        //    Matrix yawMatrix = Matrix.Identity;

//        //    if (InputMgr.IsPressed(Keys.Left))
//        //    {
//        //        yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW*INITIAL_ROTATION_SPEED);
//        //    }
//        //    if(InputMgr.IsPressed(Keys.Right))
//        //    {
//        //        yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, -DELTA_YAW* INITIAL_ROTATION_SPEED);
//        //    }

//        //    Direction = Vector3.Transform(Direction, yawMatrix);
//        //}

//        //private void ManagePitch()
//        //{
//        //    Matrix rollMatrix = Matrix.Identity;

//        //    if (InputMgr.IsPressed(Keys.Down))
//        //    {
//        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL* INITIAL_ROTATION_SPEED);
//        //    }
//        //    if(InputMgr.IsPressed(Keys.Up))
//        //    {
//        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL* INITIAL_ROTATION_SPEED);
//        //    }

//        //    Direction = Vector3.Transform(Direction, rollMatrix);
//        //    //VerticalOrientation = Vector3.Transform(VerticalOrientation, rollMatrix);
//        //}

//        //private void ManageRoll()
//        //{
//        //    Matrix matriceRoulis = Matrix.Identity;

//        //    if (InputMgr.IsPressed(Keys.PageUp))
//        //    {
//        //        matriceRoulis = Matrix.CreateFromAxisAngle(Direction, DELTA_PITCH* INITIAL_ROTATION_SPEED);
//        //    }
//        //    if(InputMgr.IsPressed(Keys.PageDown))
//        //    {
//        //        matriceRoulis = Matrix.CreateFromAxisAngle(Direction, -DELTA_PITCH* INITIAL_ROTATION_SPEED);
//        //    }

//        //    VerticalOrientation = Vector3.Transform(VerticalOrientation, matriceRoulis);
//        //}

//        //private void KeyboardManagement()
//        //{
//        //    if (InputMgr.IsNewKey(Keys.Z))
//        //    {
//        //        InZoom = !InZoom;
//        //    }
//        //}
//        //#endregion


//        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
//        const float ACCELERATION = 0.001f;
//        const float INITIAL_ROTATION_SPEED = 5f;
//        const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
//        const float TRANSLATION_INITIAL_SPEED = 0.5f;
//        const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
//        const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
//        const float DELTA_PITCH = MathHelper.Pi / 180; // 1 degré à la fois
//        const float COLLISION_RADIUS = 1f;
//        const int CHARACTER_HEIGHT = -6;

//        Vector3 Direction { get; set; }
//        Vector3 Lateral { get; set; }
//        Maze Maze { get; set; }
//        //Grass Grass { get; set; }
//        Walls Walls { get; set; }
//        float TranslationSpeed { get; set; }
//        float SpeedRotation { get; set; }
//        Point PreviousMousePosition { get; set; }
//        Point CurrentMousePosition { get; set; }
//        Vector2 DisplacementMouse { get; set; }

//        float UpdateInterval { get; set; }
//        float TimeElapsedSinceUpdate { get; set; }
//        InputManager InputMgr { get; set; }
//        float Height { get; set; }
//        List<Character> Characters { get; set; }

//        public PlayerCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation, float updateInterval)
//           : base(game)
//        {
//            UpdateInterval = updateInterval;
//            CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, 100); // 500 FAR_PLANE_DISTANCE
//            CreateLookAt(cameraPosition, target, orientation);
//            Height = cameraPosition.Y;
//        }

//        public override void Initialize()
//        {
//            SpeedRotation = INITIAL_ROTATION_SPEED;
//            TranslationSpeed = TRANSLATION_INITIAL_SPEED;
//            TimeElapsedSinceUpdate = 0;
//            base.Initialize();
//            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
//            Maze = Game.Services.GetService(typeof(Maze)) as Maze;
//            //Grass = Game.Services.GetService(typeof(Grass)) as Grass;
//            //Walls = Game.Services.GetService(typeof(Walls)) as Walls;
//            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
//            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//            PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
//            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
//        }

//        protected override void CreateLookAt()
//        {
//            Vector3.Normalize(Direction);
//            Vector3.Normalize(VerticalOrientation);
//            Vector3.Normalize(Lateral);

//            View = Matrix.CreateLookAt(Position, Position + Direction, VerticalOrientation);
//        }

//        protected override void CreateLookAt(Vector3 position, Vector3 target, Vector3 orientation)
//        {
//            Position = position;
//            Target = target;
//            VerticalOrientation = orientation;

//            Direction = target - Position;
//            //Direction = target;

//            Vector3.Normalize(Target);

//            CreateLookAt();
//        }

//        public override void Update(GameTime gameTime)
//        {
//            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
//            TimeElapsedSinceUpdate += timeElapsed;
//            if (TimeElapsedSinceUpdate >= UpdateInterval)
//            {
//                MouseFunctions();
//                KeyboardFunctions();

//                ManageHeight();
//                CreateLookAt();




//                Game.Window.Title = Position.ToString();
//                Position = new Vector3(Position.X, Height, Position.Z);
//                TimeElapsedSinceUpdate = 0;
//            }
//            base.Update(gameTime);
//        }

//        //Mouse
//        #region
//        private void MouseFunctions()
//        {
//            PreviousMousePosition = CurrentMousePosition;
//            CurrentMousePosition = InputMgr.GetPositionMouse();
//            DisplacementMouse = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
//                                            CurrentMousePosition.Y - PreviousMousePosition.Y);

//            ManageMouseRotation();

//            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

//        }

//        private void ManageMouseRotation()
//        {
//            ManageMouseYaw();
//            ManageMouseRoll();
//        }

//        private void ManageMouseYaw()
//        {
//            Matrix yawMatrix = Matrix.Identity;

//            yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.X);

//            Direction = Vector3.Transform(Direction, yawMatrix);
//        }

//        private void ManageMouseRoll()
//        {
//            Matrix rollMatrix = Matrix.Identity;

//            rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.Y);

//            Direction = Vector3.Transform(Direction, rollMatrix);
//        }
//        #endregion

//        //Keyboard
//        #region
//        private void KeyboardFunctions()
//        {
//            ManageDisplacement();
//            ManageKeyboardRotation();
//        }

//        private void ManageDisplacement()
//        {
//            float displacementDirection = (ManageKey(Keys.W) - ManageKey(Keys.S)) * TranslationSpeed;
//            float lateralDisplacement = (ManageKey(Keys.A) - ManageKey(Keys.D)) * TranslationSpeed;

//            Direction = Vector3.Normalize(Direction);
//            Lateral = Vector3.Cross(Direction, VerticalOrientation);
//            Position += displacementDirection * Direction;
//            Position -= lateralDisplacement * Lateral;
//            if (Maze.CheckForCollisions(Position))
//            {
//                Position -= displacementDirection * Direction;
//                Position += lateralDisplacement * Lateral;
//            }
//            //Vector3 newDirection = new Vector3(0, 0, 0);
//            //if (Walls.CheckForCollisions(Position, ref newDirection, Direction) || CheckForCharacterCollision())
//            //{
//            //    Position -= displacementDirection * Direction;
//            //    //Position += displacementDirection * newDirection;
//            //    Position += lateralDisplacement * Lateral;
//            //}
//        }

//        const float MAX_DISTANCE = 4.5f;

//        bool CheckForCharacterCollision()
//        {
//            bool result = false;
//            int i;

//            for (i = 0; i < Characters.Count && !result; ++i)
//            {
//                result = Vector3.Distance(Characters[i].GetPosition(), Position) < MAX_DISTANCE;
//            }

//            return result;
//        }

//        private void ManageKeyboardRotation()
//        {
//            ManageKeyboardYaw();
//            ManageKeyboardRoll();
//        }

//        private void ManageKeyboardYaw()
//        {
//            Matrix yawMatrix = Matrix.Identity;

//            if (InputMgr.IsPressed(Keys.Left))
//            {
//                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED);
//            }
//            if (InputMgr.IsPressed(Keys.Right))
//            {
//                yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, -DELTA_YAW * INITIAL_ROTATION_SPEED);
//            }

//            Direction = Vector3.Transform(Direction, yawMatrix);
//        }

//        private void ManageKeyboardRoll()
//        {
//            Matrix rollMatrix = Matrix.Identity;

//            if (InputMgr.IsPressed(Keys.Down))
//            {
//                rollMatrix = Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL * INITIAL_ROTATION_SPEED);
//            }
//            if (InputMgr.IsPressed(Keys.Up))
//            {
//                rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED);
//            }

//            Direction = Vector3.Transform(Direction, rollMatrix);
//        }
//        #endregion

//        private void ManageHeight()
//        {
//            Position = Maze.GetPositionWithHeight(Position, CHARACTER_HEIGHT);//Grass.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
//        }

//        private int ManageKey(Keys touche)
//        {
//            return InputMgr.IsPressed(touche) ? 1 : 0;
//        }
//        //const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
//        //const float ACCELERATION = 0.001f;
//        //const float INITIAL_ROTATION_SPEED = 5f;
//        //const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
//        //const float TRANSLATION_INITIAL_SPEED = 0.5f;
//        //const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
//        //const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
//        //const float DELTA_PITCH = MathHelper.Pi / 180; // 1 degré à la fois
//        //const float COLLISION_RADIUS = 1f;
//        //const int CHARACTER_HEIGHT = 10;

//        //Vector3 Direction { get; set; }
//        //Vector3 Lateral { get; set; }
//        //Grass Grass { get; set; }
//        //float TranslationSpeed { get; set; }
//        //float SpeedRotation { get; set; }
//        //Point PreviousMousePosition { get; set; }
//        //Point CurrentMousePosition { get; set; }
//        //Vector2 DisplacementMouse { get; set; }

//        //float UpdateInterval { get; set; }
//        //float TimeElapsedSinceUpdate { get; set; }
//        //InputManager InputMgr { get; set; }

//        //public PlayerCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation, float updateInterval)
//        //   : base(game)
//        //{
//        //    UpdateInterval = updateInterval;
//        //    CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
//        //    CreateLookAt(cameraPosition, target, orientation);
//        //}

//        //public override void Initialize()
//        //{
//        //    SpeedRotation = INITIAL_ROTATION_SPEED;
//        //    TranslationSpeed = TRANSLATION_INITIAL_SPEED;
//        //    TimeElapsedSinceUpdate = 0;
//        //    base.Initialize();
//        //    InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
//        //    Grass = Game.Services.GetService(typeof(Grass)) as Grass;
//        //    CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//        //    PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
//        //    Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
//        //}

//        //protected override void CreateLookAt()
//        //{
//        //    Vector3.Normalize(Direction);
//        //    Vector3.Normalize(VerticalOrientation);
//        //    Vector3.Normalize(Lateral);

//        //    View = Matrix.CreateLookAt(Position, Position + Direction, VerticalOrientation);
//        //}

//        //protected override void CreateLookAt(Vector3 position, Vector3 target, Vector3 orientation)
//        //{
//        //    Position = position;
//        //    Target = target;
//        //    VerticalOrientation = orientation;

//        //    Direction = target - Position;

//        //    Vector3.Normalize(Target);

//        //    CreateLookAt();
//        //}

//        //public override void Update(GameTime gameTime)
//        //{
//        //    float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
//        //    TimeElapsedSinceUpdate += timeElapsed;
//        //    if (TimeElapsedSinceUpdate >= UpdateInterval)
//        //    {
//        //        MouseFunctions();
//        //        KeyboardFunctions();

//        //        ManageHeight();
//        //        CreateLookAt();


//        //        ManageGrabbing();

//        //        //Game.Window.Title = Position.ToString();

//        //        TimeElapsedSinceUpdate = 0;
//        //    }
//        //    base.Update(gameTime);
//        //}

//        ////Mouse
//        //#region
//        //private void MouseFunctions()
//        //{
//        //    PreviousMousePosition = CurrentMousePosition;
//        //    CurrentMousePosition = InputMgr.GetPositionMouse();
//        //    DisplacementMouse = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
//        //                                    CurrentMousePosition.Y - PreviousMousePosition.Y);

//        //    ManageMouseRotation();

//        //    CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//        //    Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

//        //}

//        //private void ManageMouseRotation()
//        //{
//        //    ManageMouseYaw();
//        //    ManageMouseRoll();
//        //}

//        //private void ManageMouseYaw()
//        //{
//        //    Matrix yawMatrix = Matrix.Identity;

//        //    yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.X);

//        //    Direction = Vector3.Transform(Direction, yawMatrix);
//        //}

//        //private void ManageMouseRoll()
//        //{
//        //    Matrix rollMatrix = Matrix.Identity;

//        //    rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -DisplacementMouse.Y);

//        //    Direction = Vector3.Transform(Direction, rollMatrix);
//        //}
//        //#endregion

//        ////Keyboard
//        //#region
//        //private void KeyboardFunctions()
//        //{
//        //    ManageDisplacement();
//        //    ManageKeyboardRotation();
//        //}

//        //private void ManageDisplacement()
//        //{
//        //    float displacementDirection = (ManageKey(Keys.W) - ManageKey(Keys.S)) * TranslationSpeed;
//        //    float lateralDisplacement = (ManageKey(Keys.A) - ManageKey(Keys.D)) * TranslationSpeed;

//        //    Direction = Vector3.Normalize(Direction);
//        //    Position += displacementDirection * Direction;

//        //    Lateral = Vector3.Cross(Direction, VerticalOrientation);
//        //    Position -= lateralDisplacement * Lateral;
//        //}

//        //private void ManageKeyboardRotation()
//        //{
//        //    ManageKeyboardYaw();
//        //    ManageKeyboardRoll();
//        //}

//        //private void ManageKeyboardYaw()
//        //{
//        //    Matrix yawMatrix = Matrix.Identity;

//        //    if (InputMgr.IsPressed(Keys.Left))
//        //    {
//        //        yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED);
//        //    }
//        //    if (InputMgr.IsPressed(Keys.Right))
//        //    {
//        //        yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, -DELTA_YAW * INITIAL_ROTATION_SPEED);
//        //    }

//        //    Direction = Vector3.Transform(Direction, yawMatrix);
//        //}

//        //private void ManageKeyboardRoll()
//        //{
//        //    Matrix rollMatrix = Matrix.Identity;

//        //    if (InputMgr.IsPressed(Keys.Down))
//        //    {
//        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL * INITIAL_ROTATION_SPEED);
//        //    }
//        //    if (InputMgr.IsPressed(Keys.Up))
//        //    {
//        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED);
//        //    }

//        //    Direction = Vector3.Transform(Direction, rollMatrix);
//        //}
//        //#endregion

//        //private void ManageHeight()
//        //{
//        //    Position = Grass.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
//        //}

//        //private int ManageKey(Keys touche)
//        //{
//        //    return InputMgr.IsPressed(touche) ? 1 : 0;
//        //}

//        //private void ManageGrabbing()
//        //{
//        //    //Ray visor = new Ray(Position, Direction);

//        //    //foreach (SphereRamassable grabbableSphere in Game.Components.Where(component => component is GrabbableSphere))
//        //    //{
//        //    //    Game.Window.Title = grabbableSphere.IsColliding(visor).ToString();
//        //    //    //if (grabbableSphere.IsColliding(visor) != null)
//        //    //    //{
//        //    //    //    Game.Window.Title = "true";
//        //    //    //}
//        //    //    //else
//        //    //    //{
//        //    //    //    Game.Window.Title = "false";
//        //    //    //}
//        //    //}
//        //}
//    }
//}


































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
        const float TRANSLATION_INITIAL_SPEED = 0.5f;
        const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_PITCH = MathHelper.Pi / 180; // 1 degré à la fois
        const float COLLISION_RADIUS = 1f;
        const int CHARACTER_HEIGHT = 10;
        const int MAXIMAL_RUN_FACTOR = 4;
        const int MINIMAL_DISTANCE_POUR_RAMASSAGE = 45;

        public Vector3 Direction { get; private set; }//
        public Vector3 Lateral { get; private set; }//
        Grass Grass { get; set; }
        protected float TranslationSpeed { get; set; }
        float SpeedRotation { get; set; }
        Point PreviousMousePosition { get; set; }
        Point CurrentMousePosition { get; set; }
        Vector2 DisplacementMouse { get; set; }

        float UpdateInterval { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
        InputManager InputMgr { get; set; }
        GamePadManager GamePadMgr { get; set; }

        bool Jump { get; set; }
        bool Run { get; set; }
        bool Grab { get; set; }

        public bool IsMouseCameraActivated { get; set; }
        bool EstDisplacementEtAutresKeyboardActivated { get; set; }
        bool IsKeyboardCameraActivated { get; set; }

        Ray Visor { get; set; }

        float Height { get; set; }

        public PlayerCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation, float updateInterval) : base(game)
        {
            UpdateInterval = updateInterval;
            CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
            CreateLookAt(cameraPosition, target, orientation);
            Height = cameraPosition.Y;
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
                GamePadFunctions();

                ManageHeight();
                CreateLookAt();

                PopulateCommands();

                ManageGrabbing();
                ManageRun();
                ManageJump();

                //Game.Window.Title = GamePadMgr.PositionsGâchettes.X.ToString();
                TimeElapsedSinceUpdate = 0;
            }
            base.Update(gameTime);

        }

        //Mouse
        #region
        private void MouseFunctions()
        {
            if (IsMouseCameraActivated)
            {
                PreviousMousePosition = CurrentMousePosition;
                CurrentMousePosition = InputMgr.GetPositionMouse();
                DisplacementMouse = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
                                                CurrentMousePosition.Y - PreviousMousePosition.Y);

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
            ManageMouseRoll();
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

            Grab = InputMgr.IsNewLeftClick() ||
                       InputMgr.IsOldLeftClick() ||
                       InputMgr.IsPressed(Keys.E) && EstDisplacementEtAutresKeyboardActivated ||
                       GamePadMgr.IsNewButton(Buttons.RightStick);
        }

        private void ManageHeight()
        {
            //Position = Grass.GetPositionWithHeight(Position, (int)Height);
            Position = new Vector3(Position.X, Height, Position.Z);
        }

        private int ManageKey(Keys touche)
        {
            return InputMgr.IsPressed(touche) ? 1 : 0;
        }

        private void ManageGrabbing()
        {
            Visor = new Ray(Position, Direction);

            foreach (GrabbableSphere grabbableSphere in Game.Components.Where(component => component is GrabbableSphere))
            {
                Grab = grabbableSphere.IsColliding(Visor) <= MINIMAL_DISTANCE_POUR_RAMASSAGE &&
                           grabbableSphere.IsColliding(Visor) != null &&
                           Grab;

                //Game.Window.Title = grabbableSphere.IsColliding(Visor).ToString();
                if (Grab)
                {
                    grabbableSphere.IsGrabbed = true;
                }
            }
        }

        //Jump
        #region
        private void ManageJump()
        {
            if (Jump)
            {
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
        float Height { get; set; }

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

        private void ManageRun()
        {
            TranslationSpeed = Run ? (GamePadMgr.PositionsGâchettes.X > 0 ? GamePadMgr.PositionsGâchettes.X : 1) * MAXIMAL_RUN_FACTOR * TRANSLATION_INITIAL_SPEED : TRANSLATION_INITIAL_SPEED;
        }
    }
}

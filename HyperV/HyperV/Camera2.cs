//using XNAProject;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using System.Collections.Generic;
//using System.Linq;

//namespace HyperV
//{
//    public class Camera2 : Camera
//    {
//        //const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
//        //const float ACCELERATION = 0.001f;
//        //const float INITIAL_ROTATION_SPEED = 5f;
//        //const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
//        //const float TRANSLATION_INITIAL_SPEED = 0.5f;
//        //const float DELTA_YAW = MathHelper.Pi / 180; // 1 degree at a time
//        //const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degree at a time
//        //const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degree at a time
//        //const float RAYON_COLLISION = 1f;
//        //const int CHARACTER_HEIGHT = -6;

//        //Vector3 Direction { get; set; }
//        //Vector3 Lateral { get; set; }
//        //Maze Maze { get; set; }
//        //Walls Walls { get; set; }
//        //float TranslationSpeed { get; set; }
//        //float SpeedRotation { get; set; }
//        //Point PreviousMousePosition { get; set; }
//        //Point CurrentMousePosition { get; set; }
//        //Vector2 MouseDisplacement { get; set; }

//        //float UpdateInterval { get; set; }
//        //float TimeElapsedSinceUpdate { get; set; }
//        //InputManager InputMgr { get; set; }
//        //float Height { get; set; }
//        //List<Character> Characters { get; set; }

//        //public Camera2(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval) : base(game)
//        //{
//        //    UpdateInterval = updateInterval;
//        //    CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLAN_DISTANCE, FAR_PLANE_DISTANCE);
//        //    CreateLookAt(positionCamera, target, orientation);
//        //    Height = positionCamera.Y;
//        //}

//        //public override void Initialize()
//        //{
//        //    SpeedRotation = INITIAL_ROTATION_SPEED;
//        //    TranslationSpeed = TRANSLATION_INITIAL_SPEED;
//        //    TimeElapsedSinceUpdate = 0;
//        //    base.Initialize();
//        //    InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
//        //    Maze = Game.Services.GetService(typeof(Maze)) as Maze;
//        //    Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
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
//        //    //Direction = target;

//        //    Vector3.Normalize(Target);

//        //    CreateLookAt();
//        //}

//        //public override void Update(GameTime gameTime)
//        //{
//        //    float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
//        //    TimeElapsedSinceUpdate += elapsedTime;
//        //    if (TimeElapsedSinceUpdate >= UpdateInterval)
//        //    {
//        //        MouseFunctions();
//        //        KeyboardFunctions();

//        //        ManageHeight();
//        //        CreateLookAt();




//        //        Game.Window.Title = Position.ToString();
//        //        Position = new Vector3(Position.X, Height, Position.Z);
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
//        //    MouseDisplacement = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
//        //                                    CurrentMousePosition.Y - PreviousMousePosition.Y);

//        //    ManageRotationMouse();

//        //    CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//        //    Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

//        //}

//        //private void ManageRotationMouse()
//        //{
//        //    ManageMouseYaw();
//        //    ManageMousePitch();
//        //}

//        //private void ManageMouseYaw()
//        //{
//        //    Matrix yawMatrix = Matrix.Identity;

//        //    yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -MouseDisplacement.X);

//        //    Direction = Vector3.Transform(Direction, yawMatrix);
//        //}

//        //private void ManageMousePitch()
//        //{
//        //    Matrix rollMatrix = Matrix.Identity;

//        //    rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -MouseDisplacement.Y);

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
//        //    float displacementLateral = (ManageKey(Keys.A) - ManageKey(Keys.D)) * TranslationSpeed;

//        //    Direction = Vector3.Normalize(Direction);
//        //    Lateral = Vector3.Cross(Direction, VerticalOrientation);
//        //    Position += displacementDirection * Direction;
//        //    Position -= displacementLateral * Lateral;
//        //    if (Maze.CheckForCollisions(Position))
//        //    {
//        //        Position -= displacementDirection * Direction;
//        //        Position += displacementLateral * Lateral;
//        //    }
//        //}

//        //const float MAX_DISTANCE = 4.5f;

//        //bool CheckForCharacterCollision()
//        //{
//        //    bool result = false;
//        //    int i;

//        //    for (i = 0; i < Characters.Count && !result; ++i)
//        //    {
//        //        result = Vector3.Distance(Characters[i].GetPosition(), Position) < MAX_DISTANCE;
//        //    }

//        //    return result;
//        //}

//        //private void ManageKeyboardRotation()
//        //{
//        //    ManageKeyboardYaw();
//        //    ManageKeyboardPitch();
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

//        //private void ManageKeyboardPitch()
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
//        //    Position = Maze.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
//        //}

//        //private int ManageKey(Keys key)
//        //{
//        //    return InputMgr.IsPressed(key) ? 1 : 0;
//        //}

//        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
//        const float ACCELERATION = 0.001f;
//        const float INITIAL_ROTATION_SPEED = 5f;
//        const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
//        const float TRANSLATION_INITIAL_SPEED = 0.5f;
//        const float DELTA_YAW = MathHelper.Pi / 180; // 1 degree at a time
//        const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degree at a time
//        const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degree at a time
//        const float RAYON_COLLISION = 1f;
//        const int CHARACTER_HEIGHT = 10;
//        const int RUN_FACTOR = 4;
//        const int MINIMAL_DISTANCE_TO_GRAB = 45;

//        public Vector3 Direction { get; private set; }//
//        public Vector3 Lateral { get; private set; }//
//        Grass Grass { get; set; }
//        float TranslationSpeed { get; set; }
//        float SpeedRotation { get; set; }
//        Point PreviousMousePosition { get; set; }
//        Point CurrentMousePosition { get; set; }
//        Vector2 MouseDisplacement { get; set; }

//        float UpdateInterval { get; set; }
//        float TimeElapsedSinceUpdate { get; set; }
//        InputManager InputMgr { get; set; }
//        GamePadManager GamePadMgr { get; set; }


//        bool Jump { get; set; }
//        bool Run { get; set; }
//        bool Grab { get; set; }

//        Ray Visor { get; set; }

//        //Added from first Camera1
//        float Height { get; set; }
//        Maze Maze { get; set; }
//        List<Character> Characters { get; set; }

//        public Camera2(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval) : base(game)
//        {
//            UpdateInterval = updateInterval;
//            CreateViewingFrustum(OBJECTIVE_OPENNESS, NEAR_PLAN_DISTANCE, FAR_PLANE_DISTANCE);
//            CreateLookAt(positionCamera, target, orientation);
//            //Added from first Camera1
//            Height = positionCamera.Y;
//        }

//        public override void Initialize()
//        {
//            SpeedRotation = INITIAL_ROTATION_SPEED;
//            TranslationSpeed = TRANSLATION_INITIAL_SPEED;
//            TimeElapsedSinceUpdate = 0;

//            Run = false;
//            Jump = false;
//            Grab = false;
//            Visor = new Ray();

//            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//            PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
//            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
//            //NEW*****************************************
//            InGame = true;

//            base.Initialize();
//            LoadContent();

//            InitializeComplexObjectsJump();
//            Height = Height;//CHARACTER_HEIGHT;
//        }

//        Boss Boss { get; set; }

//        private void LoadContent()
//        {
//            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
//            GamePadMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
//            //Grass = Game.Services.GetService(typeof(Grass)) as Grass;
//            Maze = Game.Services.GetService(typeof(Maze)) as Maze;
//            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
//            Boss = Game.Services.GetService(typeof(Boss)) as Boss;
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

//            Vector3.Normalize(Target);

//            CreateLookAt();
//        }

//        public override void Update(GameTime gameTime)
//        {
//            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
//            TimeElapsedSinceUpdate += elapsedTime;
//            if (TimeElapsedSinceUpdate >= UpdateInterval)
//            {
//                MouseFunctions();
//                KeyboardFunctions();
//                GamePadFunctions();

//                ManageHeight();
//                CreateLookAt();

//                PopulateCommands();

//                ManageGrabbing();
//                ManageRun();
//                ManageJump();

//                //Game.Window.Title = Position.ToString();
//                //Position = new Vector3(Position.X, Height, Position.Z);
//                TimeElapsedSinceUpdate = 0;
//            }
//            base.Update(gameTime);

//        }

//        //Mouse
//        #region

//        //NEW*****************************
//        bool InGame { get; set; }

//        public void SetInGame(bool inGame)
//        {
//            InGame = inGame;
//        }

//        private void MouseFunctions()
//        {
//            PreviousMousePosition = CurrentMousePosition;
//            CurrentMousePosition = InputMgr.GetPositionMouse();
//            MouseDisplacement = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
//                                            CurrentMousePosition.Y - PreviousMousePosition.Y);

//            ManageRotationMouse();

//            //NEW**********************
//            if (InGame)
//            {
//                CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
//                Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
//            }
//        }

//        private void ManageRotationMouse()
//        {
//            ManageMouseYaw();
//            ManageMousePitch();
//        }

//        private void ManageMouseYaw()
//        {
//            Matrix yawMatrix = Matrix.CreateFromAxisAngle(VerticalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -MouseDisplacement.X);

//            Direction = Vector3.Transform(Direction, yawMatrix);
//        }

//        private void ManageMousePitch()
//        {
//            Matrix rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -MouseDisplacement.Y);

//            Direction = Vector3.Transform(Direction, rollMatrix);
//        }
//        #endregion

//        //Keyboard
//        #region
//        private void KeyboardFunctions()
//        {
//            ManageDisplacement((ManageKey(Keys.W) - ManageKey(Keys.S)),
//                             (ManageKey(Keys.A) - ManageKey(Keys.D)));
//            ManageKeyboardRotation();
//        }

//        private void ManageDisplacement(float direction, float lateral)
//        {
//            float displacementDirection = direction * TranslationSpeed;
//            float displacementLateral = lateral * TranslationSpeed;

//            Direction = Vector3.Normalize(Direction);
//            Position += displacementDirection * Direction;

//            Lateral = Vector3.Cross(Direction, VerticalOrientation);
//            Position -= displacementLateral * Lateral;

//            //Added from first Camera2
//            if (Maze.CheckForCollisions(Position) || CheckForBossCollision())
//            {
//                Position -= displacementDirection * Direction;
//                Position += displacementLateral * Lateral;
//            }
//        }

//        //Added from first Camera
//        const float MAX_DISTANCE = 4.5f, MAX_DISTANCE_BOSS = 80f;

//        bool CheckForBossCollision()
//        {
//            return Vector3.Distance(Boss.GetPosition(), Position) < MAX_DISTANCE_BOSS;
//        }

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
//            ManageKeyboardPitch();
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

//        private void ManageKeyboardPitch()
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

//        //GamePad
//        #region
//        private void GamePadFunctions()
//        {
//            if (GamePadMgr.IsGamePadActivated)
//            {
//                ManageDisplacement(GamePadMgr.PositionThumbStickLeft.Y,
//                                 -GamePadMgr.PositionThumbStickLeft.X);

//                MouseDisplacement = new Vector2(35, -35) * GamePadMgr.PositionThumbStickRight;
//                ManageRotationMouse();
//            }
//        }
//        #endregion

//        private void PopulateCommands()
//        {
//            Run = InputMgr.IsPressed(Keys.RightShift) ||
//                      InputMgr.IsPressed(Keys.LeftShift) ||
//                      GamePadMgr.IsPressed(Buttons.LeftStick);

//            Jump = InputMgr.IsPressed(Keys.R/*Keys.Space*/) ||
//                     GamePadMgr.IsPressed(Buttons.A);

//            Grab = InputMgr.IsNewLeftClick() ||
//                       InputMgr.IsOldLeftClick() ||
//                       GamePadMgr.IsNewButton(Buttons.RightStick);
//        }

//        private void ManageHeight()
//        {
//            //Position = Grass.GetPositionWithHeight(Position, (int)Height);
//            Position = new Vector3(Position.X, Height, Position.Z);
//        }

//        private int ManageKey(Keys key)
//        {
//            return InputMgr.IsPressed(key) ? 1 : 0;
//        }

//        private void ManageGrabbing()
//        {
//            Visor = new Ray(Position, Direction);

//            foreach (GrabbableSphere grabbableSphere in Game.Components.Where(component => component is GrabbableSphere))
//            {
//                Grab = grabbableSphere.IsColliding(Visor) <= MINIMAL_DISTANCE_TO_GRAB &&
//                           grabbableSphere.IsColliding(Visor) != null &&
//                           Grab;

//                //Game.Window.Title = grabbableSphere.IsColliding(Visor).ToString();
//                if (Grab)
//                {
//                    grabbableSphere.IsGrabbed = true;
//                }
//            }
//        }

//        //Jump
//        #region
//        private void ManageJump()
//        {
//            if (Jump)
//            {
//                ContinueJump = true;
//            }

//            if (ContinueJump)
//            {
//                if (t > 60)
//                {
//                    InitializeComplexObjectsJump();
//                    ContinueJump = false;
//                    t = 0;
//                }
//                Height = ComputeBezier(t * (1f / 60f), ControlPts).Y;
//                ++t;
//            }
//        }

//        bool ContinueJump { get; set; }
//        float t { get; set; }
//        float Height { get; set; }

//        Vector3 ControlPositionPts { get; set; }
//        Vector3 ControlPositionPtsPlusOne { get; set; }
//        Vector3[] ControlPts { get; set; }

//        void InitializeComplexObjectsJump()
//        {
//            Position = new Vector3(Position.X, Height/*CHARACTER_HEIGHT*/, Position.Z);
//            ControlPositionPts = new Vector3(Position.X, Position.Y, Position.Z);
//            ControlPositionPtsPlusOne = Position + Vector3.Normalize(new Vector3(Direction.X, 0, Direction.Z)) * 25;
//            //Position = new Vector3(ControlPositionPts.X, ControlPositionPts.Y, ControlPositionPts.Z);//******
//            //Direction = ControlPositionPtsPlusOne - ControlPositionPts;//******
//            ControlPts = ComputeControlPoints();
//        }

//        private Vector3[] ComputeControlPoints()
//        {
//            Vector3[] pts = new Vector3[4];
//            pts[0] = ControlPositionPts;
//            pts[3] = ControlPositionPtsPlusOne;
//            pts[1] = new Vector3(pts[0].X, pts[0].Y + 20, pts[0].Z);
//            pts[2] = new Vector3(pts[3].X, pts[3].Y + 20, pts[3].Z);
//            return pts;
//        }

//        private Vector3 ComputeBezier(float t, Vector3[] ControlPts)
//        {
//            float x = (1 - t);
//            return ControlPts[0] * (x * x * x) +
//                   3 * ControlPts[1] * t * (x * x) +
//                   3 * ControlPts[2] * t * t * x +
//                   ControlPts[3] * t * t * t;

//        }
//        #endregion

//        private void ManageRun()
//        {
//            TranslationSpeed = Run ? RUN_FACTOR * TRANSLATION_INITIAL_SPEED : TRANSLATION_INITIAL_SPEED;
//        }
//    }
//}



using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class Camera2 : PlayerCamera
    {
        //Added from first Camera1
        //float Height { get; set; }

        List<Maze> Maze { get; set; }
        List<Character> Characters { get; set; }
        Boss Boss { get; set; }
        List<HeightMap> HeightMap { get; set; }
        Water Water { get; set; }
        Grass Grass { get; set; }
        List<Walls> Walls { get; set; }
        List<Portal> Portals { get; set; }
        List<House> Houses { get; set; }

        public Camera2(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval, float renderDistance)
            : base(game, positionCamera, target, orientation, updateInterval, renderDistance)
        { }

        protected override void LoadContent()
        {
            base.LoadContent();
            Maze = Game.Services.GetService(typeof(List<Maze>)) as List<Maze>;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
            Boss = Game.Services.GetService(typeof(Boss)) as Boss;
            HeightMap = Game.Services.GetService(typeof(List<HeightMap>)) as List<HeightMap>;
            Grass = Game.Services.GetService(typeof(Grass)) as Grass;
            ManageHeight();
            Water = Game.Services.GetService(typeof(Water)) as Water;
            Walls = Game.Services.GetService(typeof(List<Walls>)) as List<Walls>;
            Houses = Game.Services.GetService(typeof(List<House>)) as List<House>;
            Portals = Game.Services.GetService(typeof(List<Portal>)) as List<Portal>;
        }

        //NO WATER
        //protected override void ManageHeight()
        //{
        //    //Height = HeightMap.GetHeight(Position);
        //    //NO WATER
        //    //    if (!LifeBars[1].Water)
        //    //    {
        //    //        Height = HeightMap.GetHeight(Position); //HERE
        //    //        base.ManageHeight();
        //    //    }
        //}

        protected override void ManageHeight()
        {
            if (HeightMap.Count > 0)
            {
                float height = 5;
                for(int i = 0; i < HeightMap.Count && height == 5; ++i)
                {
                    height = HeightMap[i].GetHeight(Position);
                }
                Height = height;
            }
            base.ManageHeight();
        }

        protected override void ManageDisplacement(float direction, float lateral)
        {
            base.ManageDisplacement(direction, lateral);

            if ((Maze.Count > 0 ? CheckForMazeCollision() : false) || (Walls.Count > 0 ? CheckForWallsCollision() : false) || (Characters.Count > 0 ? CheckForCharacterCollision() : false) || (Portals.Count > 0 ? CheckForPortalCollision() : false) || (Boss != null ? CheckForBossCollision() : false) || (Houses.Count > 0 ? CheckForHouseCollision() : false))
            {
                Position -= direction * TranslationSpeed * Direction;
                Position += lateral * TranslationSpeed * Lateral;
            }
            // NO WATER
            //if (LifeBars[1].Water)
            //{
            //    Position -= direction * TranslationSpeed * Direction;
            //    Position += lateral * TranslationSpeed * Lateral;
            //    Position += direction * TRANSLATION_INITIAL_SPEED * Direction;
            //    Position -= lateral * TRANSLATION_INITIAL_SPEED * Lateral;
            //}
            //if (!LifeBars[1].Water && Position.Y <= Water.AdjustedHeight)
            //{
            //    LifeBars[1].TurnWaterOn();
            //}
            //else if (LifeBars[1].Water && Position.Y > Water.AdjustedHeight)
            //{
            //    LifeBars[1].TurnWaterOff();
            //}
            //if (LifeBars[1].Drowned)
            //{
            //    LifeBars[0].Attack(1);
            //}
        }
        //NO WATER
        //protected override void ManageJump()
        //{
        //    if (LifeBars[1].Water)
        //    {
        //        if (Jump)
        //        {
        //            Height += 0.4f;
        //            if (Height > Water.AdjustedHeight)
        //            {
        //                Height = Water.AdjustedHeight;
        //                LifeBars[1].Restore();
        //            }
        //            Position = new Vector3(Position.X, Height/*CHARACTER_HEIGHT*/, Position.Z);
        //            //++Height;
        //        }
        //        else
        //        {
        //            Height -= 0.4f;
        //            if (Height < HeightMap.GetHeight(Position))
        //            {
        //                Height = HeightMap.GetHeight(Position);
        //            }
        //            Position = new Vector3(Position.X, Height/*CHARACTER_HEIGHT*/, Position.Z);
        //            //--Height;
        //        }
        //    }
        //    else
        //    {
        //        base.ManageJump();
        //    }
        //}

        bool CheckForWallsCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Walls.Count && !result; ++i)
            {
                result = Walls[i].CheckForCollisions(Position);
            }

            return result;
        }

        bool CheckForMazeCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Maze.Count && !result; ++i)
            {
                result = Maze[i].CheckForCollisions(Position);
            }

            return result;
        }

        const float MAX_DISTANCE = 4.5f, MAX_DISTANCE_BOSS = 80f;

        bool CheckForBossCollision()
        {
            return Vector3.Distance(Boss.GetPosition(), Position) < MAX_DISTANCE_BOSS;
        }

        bool CheckForPortalCollision()
        {
            Game.Window.Title = Position.ToString();
            bool result = false;
            int i;

            for (i = 0; i < Portals.Count && !result; ++i)
            {
                result = Portals[i].CheckForCollisions(Position);
            }

            return result;
        }

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

        bool CheckForHouseCollision()
        {
            bool result = false;
            float? d;
            int i;

            for (i = 0; i < Houses.Count && !result; ++i)
            {
                result = Houses[i].Collision(new BoundingSphere(Position, 7));
            }

            return result;
        }
    }
}


using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class Camera1 : Camera
    {
        //const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        //const float ACCÉLÉRATION = 0.001f;
        //const float INITIAL_ROTATION_SPEED = 5f;
        //const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
        //const float TRANSLATION_INITIAL_SPEED = 0.5f;
        //const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
        //const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
        //const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degré à la fois
        //const float RAYON_COLLISION = 1f;
        //const int CHARACTER_HEIGHT = -6;

        //public Vector3 Direction { get; private set; }
        //Vector3 Lateral { get; set; }
        //Grass Grass { get; set; }
        //Walls Walls { get; set; }
        //float TranslationSpeed { get; set; }
        //float SpeedRotation { get; set; }
        //Point PreviousMousePosition { get; set; }
        //Point CurrentMousePosition { get; set; }
        //Vector2 DéplacementMouse { get; set; }

        //float UpdateInterval { get; set; }
        //float TimeElapsedSinceUpdate { get; set; }
        //InputManager InputMgr { get; set; }
        //float Height { get; set; }
        //List<Character> Characters { get; set; }

        //bool Jump { get; set; }
        //bool Run { get; set; }
        //bool Grab { get; set; }

        //Ray Visor { get; set; }
        //GamePadManager GamePadMgr { get; set; }

        //Vector3 ControlPositionPts { get; set; }
        //Vector3 ControlPositionPtsPlusUn { get; set; }
        //Vector3[] ControlPts { get; set; }

        //public Camera1(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval) : base(game)
        //{
        //    UpdateInterval = updateInterval;
        //    CréerVolumeDeVisualisation(OBJECTIVE_OPENNESS, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
        //    CréerPointDeView(positionCamera, target, orientation);
        //    Height = positionCamera.Y;
        //}

        //public override void Initialize()
        //{
        //    SpeedRotation = INITIAL_ROTATION_SPEED;
        //    TranslationSpeed = TRANSLATION_INITIAL_SPEED;
        //    TimeElapsedSinceUpdate = 0;
        //    base.Initialize();
        //    InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
        //    GamePadMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
        //    Grass = Game.Services.GetService(typeof(Grass)) as Grass;
        //    Walls = Game.Services.GetService(typeof(Walls)) as Walls;
        //    Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
        //    Run = false;
        //    Jump = false;
        //    Grab = false;
        //    Visor = new Ray();
        //    CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
        //    PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
        //    Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
        //    InitializeComplexObjectsJump();
        //}

        //void InitializeComplexObjectsJump()
        //{
        //    Position = new Vector3(Position.X, CHARACTER_HEIGHT, Position.Z);
        //    ControlPositionPts = new Vector3(Position.X, Position.Y, Position.Z);
        //    ControlPositionPtsPlusUn = Position + Vector3.Normalize(new Vector3(Direction.X, 0, Direction.Z)) * 25;
        //    //Position = new Vector3(ControlPositionPts.X, ControlPositionPts.Y, ControlPositionPts.Z);//******
        //    //Direction = ControlPositionPtsPlusUn - ControlPositionPts;//******
        //    ControlPts = ComputeControlPoints();
        //}

        //Vector3[] ComputeControlPoints()
        //{
        //    Vector3[] pts = new Vector3[4];
        //    pts[0] = ControlPositionPts;
        //    pts[3] = ControlPositionPtsPlusUn;
        //    pts[1] = new Vector3(pts[0].X, pts[0].Y + 20, pts[0].Z);
        //    pts[2] = new Vector3(pts[3].X, pts[3].Y + 20, pts[3].Z);
        //    return pts;
        //}

        //protected override void CréerPointDeView()
        //{
        //    Vector3.Normalize(Direction);
        //    Vector3.Normalize(GreenicalOrientation);
        //    Vector3.Normalize(Lateral);

        //    View = Matrix.CreateLookAt(Position, Position + Direction, GreenicalOrientation);
        //}

        //protected override void CréerPointDeView(Vector3 position, Vector3 target, Vector3 orientation)
        //{
        //    Position = position;
        //    Target = target;
        //    GreenicalOrientation = orientation;

        //    Direction = target - Position;
        //    //Direction = target;

        //    Vector3.Normalize(Target);

        //    CréerPointDeView();
        //}

        //public override void Update(GameTime gameTime)
        //{
        //    float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    TimeElapsedSinceUpdate += elapsedTime;
        //    if (TimeElapsedSinceUpdate >= UpdateInterval)
        //    {
        //        MouseFunctions();
        //        KeyboardFunctions();
        //        ManageHeight();
        //        CréerPointDeView();
        //        Game.Window.Title = Position.ToString();
        //        Position = new Vector3(Position.X, Height, Position.Z);
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
        //    DéplacementMouse = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
        //                                    CurrentMousePosition.Y - PreviousMousePosition.Y);

        //    GérerRotationMouse();

        //    CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
        //    Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

        //}

        //private void GérerRotationMouse()
        //{
        //    GérerLacetMouse();
        //    GérerTangageMouse();
        //}

        //void GérerLacetMouse()
        //{
        //    Matrix yawMatrix = Matrix.CreateFromAxisAngle(GreenicalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -DéplacementMouse.X);

        //    Direction = Vector3.Transform(Direction, yawMatrix);
        //}

        //void GérerTangageMouse()
        //{
        //    Matrix rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -DéplacementMouse.Y);

        //    Direction = Vector3.Transform(Direction, rollMatrix);
        //}
        //#endregion

        ////Clavier
        //#region
        //private void KeyboardFunctions()
        //{
        //    ManageDisplacement();
        //    GérerRotationClavier();
        //}

        //private void ManageDisplacement()
        //{
        //    float displacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * TranslationSpeed;
        //    float displacementLateral = (GérerTouche(Keys.A) - GérerTouche(Keys.D)) * TranslationSpeed;

        //    Direction = Vector3.Normalize(Direction);
        //    Lateral = Vector3.Cross(Direction, GreenicalOrientation);
        //    Position += displacementDirection * Direction;
        //    Position -= displacementLateral * Lateral;
        //    if (Walls.CheckForCollisions(Position) || CheckForCharacterCollision())
        //    {
        //        Position -= displacementDirection * Direction;
        //        Position += displacementLateral * Lateral;
        //    }
        //}

        //const float MAX_DISTANCE = 4.5f;

        //bool CheckForCharacterCollision()
        //{
        //    bool result = false;
        //    int i;

        //    for (i = 0; i < Characters.Count && !result; ++i)
        //    {
        //        result = Vector3.Distance(Characters[i].GetPosition(), Position) < MAX_DISTANCE;
        //    }

        //    return result;
        //}

        //private void GérerRotationClavier()
        //{
        //    GérerLacetClavier();
        //    GérerTangageClavier();
        //}

        //private void GérerLacetClavier()
        //{
        //    Matrix yawMatrix = Matrix.Identity;

        //    if (InputMgr.IsPressede(Keys.Left))
        //    {
        //        yawMatrix = Matrix.CreateFromAxisAngle(GreenicalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED);
        //    }
        //    if (InputMgr.IsPressede(Keys.Right))
        //    {
        //        yawMatrix = Matrix.CreateFromAxisAngle(GreenicalOrientation, -DELTA_YAW * INITIAL_ROTATION_SPEED);
        //    }

        //    Direction = Vector3.Transform(Direction, yawMatrix);
        //}

        //private void GérerTangageClavier()
        //{
        //    Matrix rollMatrix = Matrix.Identity;

        //    if (InputMgr.IsPressede(Keys.Down))
        //    {
        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL * INITIAL_ROTATION_SPEED);
        //    }
        //    if (InputMgr.IsPressede(Keys.Up))
        //    {
        //        rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED);
        //    }

        //    Direction = Vector3.Transform(Direction, rollMatrix);
        //}
        //#endregion

        //private void ManageHeight()
        //{
        //    //Position = Grass.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
        //}

        //private int GérerTouche(Keys touche)
        //{
        //    return InputMgr.IsPressede(touche) ? 1 : 0;
        //}

        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        const float ACCÉLÉRATION = 0.001f;
        const float INITIAL_ROTATION_SPEED = 5f;
        const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
        const float TRANSLATION_INITIAL_SPEED = 0.5f;
        const float DELTA_YAW = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degré à la fois
        const float RAYON_COLLISION = 1f;
        const int CHARACTER_HEIGHT = 10;
        const int FACTEUR_COURSE = 4;
        const int MINIMAL_DISTANCE_POUR_RAMASSAGE = 45;

        public Vector3 Direction { get; private set; }//
        public Vector3 Lateral { get; private set; }//
        Gazon Gazon { get; set; }
        float TranslationSpeed { get; set; }
        float SpeedRotation { get; set; }
        Point PreviousMousePosition { get; set; }
        Point CurrentMousePosition { get; set; }
        Vector2 DéplacementMouse { get; set; }

        float UpdateInterval { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
        InputManager InputMgr { get; set; }
        GamePadManager GamePadMgr { get; set; }


        bool Jump { get; set; }
        bool Run { get; set; }
        bool Grab { get; set; }

        Ray Visor { get; set; }

        //Added from first Camera1
        float Height { get; set; }
        Grass Grass { get; set; }
        Walls Walls { get; set; }
        List<Character> Characters { get; set; }

        public Camera1(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval) : base(game)
        {
            UpdateInterval = updateInterval;
            CréerVolumeDeVisualisation(OBJECTIVE_OPENNESS, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            CréerPointDeView(positionCamera, target, orientation);
            //Added from first Camera1
            Height = positionCamera.Y;
        }

        public override void Initialize()
        {
            SpeedRotation = INITIAL_ROTATION_SPEED;
            TranslationSpeed = TRANSLATION_INITIAL_SPEED;
            TimeElapsedSinceUpdate = 0;

            Run = false;
            Jump = false;
            Grab = false;
            Visor = new Ray();

            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

            base.Initialize();
            LoadContent();

            InitializeComplexObjectsJump();
            Height = Height;//CHARACTER_HEIGHT;
        }

        private void LoadContent()
        {
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GamePadMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            //Gazon = Game.Services.GetService(typeof(Gazon)) as Gazon;
            Grass = Game.Services.GetService(typeof(Grass)) as Grass;
            Walls = Game.Services.GetService(typeof(Walls)) as Walls;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
        }


        protected override void CréerPointDeView()
        {
            Vector3.Normalize(Direction);
            Vector3.Normalize(GreenicalOrientation);
            Vector3.Normalize(Lateral);

            View = Matrix.CreateLookAt(Position, Position + Direction, GreenicalOrientation);
        }

        protected override void CréerPointDeView(Vector3 position, Vector3 target, Vector3 orientation)
        {
            Position = position;
            Target = target;
            GreenicalOrientation = orientation;

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
                GamePadFunctions();

                ManageHeight();
                CréerPointDeView();

                PopulateCommands();

                ManageGrabbing();
                GérerCourse();
                GérerJump();

                Game.Window.Title = Position.ToString();
                //Position = new Vector3(Position.X, Height, Position.Z);
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

            GérerRotationMouse();

            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
        }

        private void GérerRotationMouse()
        {
            GérerLacetMouse();
            GérerTangageMouse();
        }

        private void GérerLacetMouse()
        {
            Matrix yawMatrix = Matrix.CreateFromAxisAngle(GreenicalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -DéplacementMouse.X);

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void GérerTangageMouse()
        {
            Matrix rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -DéplacementMouse.Y);

            Direction = Vector3.Transform(Direction, rollMatrix);
        }
        #endregion

        //Clavier
        #region
        private void KeyboardFunctions()
        {
            ManageDisplacement((GérerTouche(Keys.W) - GérerTouche(Keys.S)),
                             (GérerTouche(Keys.A) - GérerTouche(Keys.D)));
            GérerRotationClavier();
        }

        private void ManageDisplacement(float direction, float lateral)
        {
            float displacementDirection = direction * TranslationSpeed;
            float displacementLateral = lateral * TranslationSpeed;

            Direction = Vector3.Normalize(Direction);
            Position += displacementDirection * Direction;

            Lateral = Vector3.Cross(Direction, GreenicalOrientation);
            Position -= displacementLateral * Lateral;

            //Added from first Camera1
            if (Walls.CheckForCollisions(Position) || CheckForCharacterCollision())
            {
                Position -= displacementDirection * Direction;
                Position += displacementLateral * Lateral;
            }
        }

        //Added from first Camera1
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

        private void GérerRotationClavier()
        {
            GérerLacetClavier();
            GérerTangageClavier();
        }

        private void GérerLacetClavier()
        {
            Matrix yawMatrix = Matrix.Identity;

            if (InputMgr.IsPressede(Keys.Left))
            {
                yawMatrix = Matrix.CreateFromAxisAngle(GreenicalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED);
            }
            if (InputMgr.IsPressede(Keys.Right))
            {
                yawMatrix = Matrix.CreateFromAxisAngle(GreenicalOrientation, -DELTA_YAW * INITIAL_ROTATION_SPEED);
            }

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void GérerTangageClavier()
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

        //GamePad
        #region
        private void GamePadFunctions()
        {
            if (GamePadMgr.EstGamepadActivé)
            {
                ManageDisplacement(GamePadMgr.PositionThumbStickLeft.Y,
                                 -GamePadMgr.PositionThumbStickLeft.X);

                DéplacementMouse = new Vector2(35, -35) * GamePadMgr.PositionThumbStickRight;
                GérerRotationMouse();
            }
        }
        #endregion

        private void PopulateCommands()
        {
            Run = InputMgr.IsPressede(Keys.RightShift) ||
                      InputMgr.IsPressede(Keys.LeftShift) ||
                      GamePadMgr.IsPressed(Buttons.LeftStick);

            Jump = InputMgr.IsPressede(Keys.R/*Keys.Space*/) ||
                     GamePadMgr.IsPressed(Buttons.A);

            Grab = InputMgr.IsNewLeftClick() ||
                       InputMgr.IsOldLeftClick() ||
                       GamePadMgr.IsNewButton(Buttons.RightStick);
        }

        private void ManageHeight()
        {
            //Position = Gazon.GetPositionWithHeight(Position, (int)Height);
            Position = new Vector3(Position.X, Height, Position.Z);
        }

        private int GérerTouche(Keys touche)
        {
            return InputMgr.IsPressede(touche) ? 1 : 0;
        }

        private void ManageGrabbing()
        {
            Visor = new Ray(Position, Direction);

            foreach (SphèreRamassable grabbableSphere in Game.Components.Where(component => component is SphèreRamassable))
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
        private void GérerJump()
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

        private void GérerCourse()
        {
            TranslationSpeed = Run ? FACTEUR_COURSE * TRANSLATION_INITIAL_SPEED : TRANSLATION_INITIAL_SPEED;
        }
    }
}

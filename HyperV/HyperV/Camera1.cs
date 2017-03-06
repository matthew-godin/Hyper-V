using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HyperV
{
    public class Camera1 : Camera
    {
        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        const float ACC�L�RATION = 0.001f;
        const float INITIAL_ROTATION_SPEED = 5f;
        const float INITIAL_ROTATION_SPEED_SOURIS = 0.1f;
        const float TRANSLATION_INITIAL_SPEED = 0.5f;
        const float DELTA_YAW = MathHelper.Pi / 180; // 1 degr� � la fois
        const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degr� � la fois
        const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degr� � la fois
        const float RAYON_COLLISION = 1f;
        const int CHARACTER_HEIGHT = -6;

        Vector3 Direction { get; set; }
        Vector3 Lateral { get; set; }
        Grass Grass { get; set; }
        Walls Walls { get; set; }
        float TranslationSpeed { get; set; }
        float SpeedRotation { get; set; }
        Point PreviousMousePosition { get; set; }
        Point CurrentMousePosition { get; set; }
        Vector2 D�placementMouse { get; set; }

        float UpdateInterval { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
        InputManager InputMgr { get; set; }
        float Height { get; set; }
        List<Character> Characters { get; set; }

        public Camera1(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval) : base(game)
        {
            UpdateInterval = updateInterval;
            Cr�erVolumeDeVisualisation(OBJECTIVE_OPENNESS, DISTANCE_PLAN_RAPPROCH�, DISTANCE_PLAN_�LOIGN�);
            Cr�erPointDeView(positionCamera, target, orientation);
            Height = positionCamera.Y;
        }

        public override void Initialize()
        {
            SpeedRotation = INITIAL_ROTATION_SPEED;
            TranslationSpeed = TRANSLATION_INITIAL_SPEED;
            TimeElapsedSinceUpdate = 0;
            base.Initialize();
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Grass = Game.Services.GetService(typeof(Grass)) as Grass;
            Walls = Game.Services.GetService(typeof(Walls)) as Walls;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            PreviousMousePosition = new Point(CurrentMousePosition.X, CurrentMousePosition.Y);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);
        }

        protected override void Cr�erPointDeView()
        {
            Vector3.Normalize(Direction);
            Vector3.Normalize(GreenicalOrientation);
            Vector3.Normalize(Lateral);

            View = Matrix.CreateLookAt(Position, Position + Direction, GreenicalOrientation);
        }

        protected override void Cr�erPointDeView(Vector3 position, Vector3 target, Vector3 orientation)
        {
            Position = position;
            Target = target;
            GreenicalOrientation = orientation;

            Direction = target - Position;
            //Direction = target;

            Vector3.Normalize(Target);

            Cr�erPointDeView();
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
                Cr�erPointDeView();




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
            D�placementMouse = new Vector2(CurrentMousePosition.X - PreviousMousePosition.X,
                                            CurrentMousePosition.Y - PreviousMousePosition.Y);

            G�rerRotationMouse();

            CurrentMousePosition = new Point(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Mouse.SetPosition(CurrentMousePosition.X, CurrentMousePosition.Y);

        }

        private void G�rerRotationMouse()
        {
            G�rerLacetMouse();
            G�rerTangageMouse();
        }

        private void G�rerLacetMouse()
        {
            Matrix yawMatrix = Matrix.Identity;

            yawMatrix = Matrix.CreateFromAxisAngle(GreenicalOrientation, DELTA_YAW * INITIAL_ROTATION_SPEED_SOURIS * -D�placementMouse.X);

            Direction = Vector3.Transform(Direction, yawMatrix);
        }

        private void G�rerTangageMouse()
        {
            Matrix rollMatrix = Matrix.Identity;

            rollMatrix = Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * INITIAL_ROTATION_SPEED_SOURIS * -D�placementMouse.Y);

            Direction = Vector3.Transform(Direction, rollMatrix);
        }
        #endregion

        //Clavier
        #region
        private void KeyboardFunctions()
        {
            ManageDisplacement();
            G�rerRotationClavier();
        }

        private void ManageDisplacement()
        {
            float displacementDirection = (G�rerTouche(Keys.W) - G�rerTouche(Keys.S)) * TranslationSpeed;
            float displacementLateral = (G�rerTouche(Keys.A) - G�rerTouche(Keys.D)) * TranslationSpeed;

            Direction = Vector3.Normalize(Direction);
            Lateral = Vector3.Cross(Direction, GreenicalOrientation);
            Position += displacementDirection * Direction;
            Position -= displacementLateral * Lateral;
            if (Walls.CheckForCollisions(Position) || CheckForCharacterCollision())
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

        private void G�rerRotationClavier()
        {
            G�rerLacetClavier();
            G�rerTangageClavier();
        }

        private void G�rerLacetClavier()
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

        private void G�rerTangageClavier()
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
            Position = Grass.GetPositionWithHeight(Position, CHARACTER_HEIGHT);
        }

        private int G�rerTouche(Keys touche)
        {
            return InputMgr.IsPressede(touche) ? 1 : 0;
        }
    }
}

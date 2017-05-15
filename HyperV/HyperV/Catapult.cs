using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNAProject;


namespace HyperV
{
    public class Catapult : ModelCreator
    {
        const float UPDATE_INTERVAL = 1 / 60f;

        float UpdateTimeElapsed { get; set; }
        float UpdateTimeElapsed2 { get; set; }
        float ThrowCooldown { get; set; }
        InputManager InputMgr { get; set; }
        Camera2 Camera { get; set; }
        AmmunitionCatapult Ammunition { get; set; }
        bool IsActivated { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }
        SoundEffect CatapultLaunched { get; set; }

        float angle_;
        float Angle
        {
            get { return angle_; }
            set
            {
                if (angle_ < 0)
                {
                    value = 0;
                }
                if (angle_ > 90)
                {
                    value = 90;
                }
                angle_ = value;
            }
        }

        int speed_;
        int Speed
        {
            get { return speed_; }
            set
            {
                if (speed_ < 10)
                {
                    value = 10;
                }
                if (speed_ > 150)
                {
                    value = 150;
                }
                speed_ = value;
            }
        }

        Vector2 PreviousVector { get; set; }

        public Catapult(Game game, string model3D, Vector3 position, float scale, float rotation)
            : base(game, model3D, position, scale, rotation)
        { }

        public override void Initialize()
        {
            ThrowCooldown = 5;
            Speed = 20;
            Angle = 45;
            base.Initialize();
            IsActivated = false;
            PreviousVector = new Vector2(Camera.Direction.X, Camera.Direction.Z);
            CatapultLaunched = SoundManager.Find("CatapultLaunched");

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Camera = Game.Services.GetService(typeof(Camera)) as Camera2;
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputMgr.IsNewLeftClick())
            {
                Camera.DeactivateCamera();
                IsActivated = !IsActivated;
                Rotation = 0;
            }

            ManageTrajectory(gameTime);
            ManageThrow(gameTime);
        }

        private void RotateModel()
        {
            Vector2 currentVector = new Vector2(Camera.Direction.X, Camera.Direction.Z);
            if (currentVector != PreviousVector)
            {
                Rotation -= Camera.MouseGamePadStickDisplacement.X * MathHelper.Pi / 180 * 0.1f;
                PreviousVector = currentVector;
            }
        }

        private void ModifyAngle()
        {
            if (InputMgr.IsPressede(Keys.W))
            {
                Angle += 1;
            }
            if (InputMgr.IsPressede(Keys.S))
            {
                Angle -= 1;
            }
        }
        private void ModifySpeed()
        {
            if (InputMgr.IsPressede(Keys.A))
            {
                Speed -= 1;
            }
            if (InputMgr.IsPressede(Keys.D))
            {
                Speed += 1;
            }
        }

        private void ManageTrajectory(GameTime gameTime)
        {
            UpdateTimeElapsed2 += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (UpdateTimeElapsed2 >= 0.5f)
            {
                if (IsActivated)
                {
                    AmmunitionCatapult trajectory = new AmmunitionCatapult(Game, "Models_Ammunition", new Vector3(Position.X, Position.Y + 15, Position.Z), 0.4f, 180);
                    Game.Components.Add(new Displayer3D(Game));
                    Game.Components.Add(trajectory);
                    trajectory.ThrowProjectile(MathHelper.ToRadians(Angle), Camera.Direction, Speed);
                }
                UpdateTimeElapsed2 = 0;
            }
        }

        private void ManageThrow(GameTime gameTime)
        {
            ThrowCooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (UpdateTimeElapsed >= UPDATE_INTERVAL)
            {
                if (IsActivated)
                {
                    RotateModel();
                    ModifyAngle();
                    ModifySpeed();
                    if (InputMgr.IsPressede(Keys.Space))
                    {
                        if (ThrowCooldown >= 5)
                        {
                            AmmunitionCatapult Ammunition = new AmmunitionCatapult(Game, "Models_Ammunition", new Vector3(Position.X, Position.Y + 15, Position.Z), 2, 180);
                            Game.Components.Add(new Displayer3D(Game));
                            Game.Components.Add(Ammunition);
                            Ammunition.ThrowProjectile(MathHelper.ToRadians(Angle), Camera.Direction, Speed);
                            Ammunition.IsAmmunition = true;
                            ThrowCooldown = 0;
                            CatapultLaunched.Play();
                        }
                    }
                }
                UpdateTimeElapsed = 0;
            }
        }
    }
}

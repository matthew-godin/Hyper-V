using GameProjectXNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class AmmunitionCatapult : ModelCreator
    {
        bool IsThrown { get; set; }
        public bool IsAmmunition { get; set; }

        const float GRAVITY = -9.81f;
        const float Weight = 0.1f;
        const float UPDATE_INTERVAL = 1 / 60f;

        int SpeedY { get; set; }
        float TimeElapsed = 0;
        float TimeElapsedSinceUpdate = 0;
        Vector3 InitialPosition { get; set; }

        float AirFriction { get; set; }
        float Angle { get; set; }

        Vector3 Displacement { get; set; }
        Vector2 Speed { get; set; }
        SoundEffect TowerDestroyed { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }



        public AmmunitionCatapult(Game game, string model3D, Vector3 position, float scale, float rotation)
            : base(game, model3D, position, scale, rotation)
        { }

        public override void Initialize()
        {
            base.Initialize();
            Displacement = Vector3.Zero;
            InitialPosition = Position;
            IsThrown = true;
            IsAmmunition = false;
            TowerDestroyed = SoundManager.Find("TowerDestroyed");
        }

        public override void Update(GameTime gameTime)
        {
            if (IsThrown)
            {
                TimeElapsedSinceUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TimeElapsedSinceUpdate >= UPDATE_INTERVAL)
                {
                    TimeElapsed += UPDATE_INTERVAL;
                    Vector3 Displacement = ProjectilePosition(TimeElapsed);
                    Position = InitialPosition + Displacement;
                    base.Update(gameTime);
                    TimeElapsedSinceUpdate = 0;
                }
            }
            if (Position.Y < -100)
            {
                Game.Components.Remove(this);
                //Game.Components.RemoveAt(0);  //remove the Displayer3D that will be at the rock -1 ... 
            }
            if (IsAmmunition)
            {
                ManageCollision();
            }
        }

        protected override void LoadContent()
        {
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
            base.LoadContent();
        }

        public void ThrowProjectile(float angle, Vector3 speed, int SpeedModifier)
        {
            IsThrown = true;
            Angle = angle;
            speed.Normalize();
            SpeedY = SpeedModifier;
            Speed = new Vector2((float)Math.Cos(speed.X), (float)Math.Sin(speed.Z)) * SpeedModifier;
        }

        private Vector3 ProjectilePosition(float time)
        {
            return new Vector3(DisplacementX(Speed.X, time), DisplacementY(Angle, SpeedY, time), DisplacementZ(Speed.Y, time));
        }

        private float DisplacementX(float speed, float time)
        {
            return speed * time;
        }

        private float DisplacementZ(float speed, float time)
        {
            return speed * time;
        }

        private float DisplacementY(float angle, float speed, float time)
        {
            return (speed * (float)Math.Sin(angle) * time + 0.5f * GRAVITY * time * time);
        }

        private void ManageCollision()
        {
            bool toDestroy = false;
            List<ModelCreator> modelToDestroy = new List<ModelCreator>();
            foreach (ModelCreator model in Game.Components.Where(x => x is ModelCreator))
            {
                if (model.IsTower)
                {
                    if (Position.X < model.GetPosition().X + 8 && Position.X > model.GetPosition().X - 8) //test X
                    {
                        if (Position.Z < model.GetPosition().Z + 8 && Position.Z > model.GetPosition().Z - 8) //test z
                        {
                            if (Position.Y < model.GetPosition().Y + 120 && Position.Y > model.GetPosition().Y) //test y
                            {
                                modelToDestroy.Add(model);
                                toDestroy = true;
                                TowerDestroyed.Play();
                                (Game as GameProject).NUM_TOWERS--;
                            }
                        }
                    }
                }
            }
            if (toDestroy)
            {
                Game.Components.Remove(this);
                foreach (ModelCreator model in modelToDestroy)
                {
                    Game.Components.Remove(model);
                }
            }
        }
    }
}

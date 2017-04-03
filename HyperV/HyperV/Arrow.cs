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


namespace HyperV
{
    public class Arrow : GrabbableModel
    {
        const float FPS_60_INTERVAL = 1f / 60f;

        Vector3 Direction { get; set; }

        float TimeElapsedSinceUpdate { get; set; }
        float TotalTime { get; set; }
        Boss Boss { get; set; }
        Enemy Enemy { get; set; }

        public Arrow(Game game, string modelName, float initialScale,
                    Vector3 initialRotation, Vector3 initialPosition, Vector3 direction)
            : base(game, modelName, initialScale, initialRotation, initialPosition)
        {
            Direction = Vector3.Normalize(direction);
        }

        public override void Initialize()
        {
            TotalTime = 0;
            base.Initialize();
            Boss = Game.Services.GetService(typeof(Boss)) as Boss;
            Enemy = Game.Services.GetService(typeof(Enemy)) as Enemy;
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TotalTime += elapsedTime;
            TimeElapsedSinceUpdate += elapsedTime;
            if (TimeElapsedSinceUpdate >= FPS_60_INTERVAL)
            {
                Boss.CheckForArrowAttack(Position, Direction, 1, this);
                Enemy.CheckForArrowAttack(Position, 1, this);
                if (TotalTime >= 5)
                {
                    Game.Components.Remove(this);
                }
                Position += Direction;
                ComputeWorld();
                TimeElapsedSinceUpdate = 0;
            }
            base.Update(gameTime);
        }

        private void ComputeWorld()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);

           // Game.Window.Title = PlayerCamera.Direction.ToString() + "      " + MathHelper.ToDegrees(angleX).ToString() + "       " + MathHelper.ToDegrees(angleY).ToString().ToString();
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameProjectXNA;


namespace HyperV
{
   public class Bow : GrabbableModel
    {
        const float FPS_60_INTERVAL = 1f / 60f;

        bool ThrowArrow { get; set; }
        InputManager InputMgr { get; set; }
        GamePadManager GamePadMgr { get; set; }
        float TimeElapsedSinceUpdate { get; set; }

        public Bow(Game game, string modelName, float initialScale,
                    Vector3 initialRotation, Vector3 initialPosition)
            : base(game, modelName, initialScale, initialRotation, initialPosition)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            ThrowArrow = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GamePadMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
        }

        public override void Update(GameTime gameTime)
        {
            ThrowArrow = (InputMgr.IsPressed(Keys.T) || InputMgr.IsNewRightClick()) && IsGrabbed;

            float TimeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += TimeElapsed;
            if (TimeElapsedSinceUpdate >= FPS_60_INTERVAL)
            {
                if (ThrowArrow)
                {
                    Game.Components.Add(new Fleche(Game, "Robot", 0.002f, new Vector3(angleY, angleX + (float)Math.PI / 2, Rotation.Z),
                                                   PlayerCamera.Position, PlayerCamera.Direction));
                }
            }
            base.Update(gameTime);

        }
    }
}

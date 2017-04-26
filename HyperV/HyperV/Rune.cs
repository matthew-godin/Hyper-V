using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAProject;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;

namespace HyperV
{
    public class Rune : PlanTexturé
    {
        float RuneActivationTime { get; set; }
        float UpdateTimeElapsed { get; set; }
        bool IsUnderPlayer { get; set; }
        public bool IsActivated { get; private set; }
        Camera2 Camera { get; set; }
        ModelCreator RuneCubeActivated { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }
        SoundEffect RuneActivated { get; set; }
        SoundEffect RuneDeactivated { get; set; }

        public Rune(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector2 étendue, Vector2 dimensions, string nomTileTexture, float updateInterval)
            : base(game, initialScale, initialRotation, initialPosition, étendue, dimensions, nomTileTexture, updateInterval)
        { }

        public override void Initialize()
        {
            base.Initialize();
            UpdateTimeElapsed = 4;
            IsActivated = false;
            RuneActivated = SoundManager.Find("Rune_Activated");
            RuneDeactivated = SoundManager.Find("Rune_Deactivated");
        }

        protected override void LoadContent()
        {
            Camera = Game.Services.GetService(typeof(Camera)) as Camera2;
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            UpdateTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;      
            RuneActivationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (UpdateTimeElapsed >= 1 / 60f)
            {
                TestPlayerPosition();
                if (IsUnderPlayer)
                {
                    if(RuneActivationTime > 2)
                    ActiverRune();
                }
                UpdateTimeElapsed = 0;
            }
            base.Update(gameTime);

        }

        private void TestPlayerPosition()
        {
            if (Camera.Position.X < InitialPosition.X && Camera.Position.X > InitialPosition.X - 3 && Camera.Position.Z > InitialPosition.Z && Camera.Position.Z < InitialPosition.Z + 3)
            {
                IsUnderPlayer = true;
            }
            else
            {
                IsUnderPlayer = false;
            }
        }

        private void ActiverRune()
        {
            if (IsActivated)
            {
                Game.Components.Remove(RuneCubeActivated);
                RuneDeactivated.Play();
                IsActivated = false;
            }
            else
            {
                RuneCubeActivated = new ModelCreator(Game, "Cube", new Vector3(InitialPosition.X - 1, InitialPosition.Y + 2, InitialPosition.Z + 1), 0.6f, 0);
                Game.Components.Add(new Displayer3D(Game));
                Game.Components.Add(RuneCubeActivated);
                RuneActivated.Play();
                IsActivated = true;
            }
            RuneActivationTime = 0;
        }
    }
}

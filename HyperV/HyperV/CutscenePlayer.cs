/*
CutscenePlayer.cs
-----------------

By Matthew Godin

Role : Creates a cutscene

Created : 2/25/17
*/
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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CutscenePlayer : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Video Video { get; set; }
        VideoPlayer Player { get; set; }
        Texture2D VideoTexture { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        RessourcesManager<Video> VideoManager { get; set; }
        Rectangle Screen { get; set; }
        string VideoName { get; set; }
        InputManager InputManager { get; set; }
        SkipCutsceneLabel Label { get; set; }

        public CutscenePlayer(Game game, string videoName) : base(game)
        {
            VideoName = videoName;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Player = new VideoPlayer();
            Video = VideoManager.Find(VideoName);
            Screen = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Player.Play(Video);
        }

        protected override void LoadContent()
        {
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            VideoManager = Game.Services.GetService(typeof(RessourcesManager<Video>)) as RessourcesManager<Video>;
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Label = new SkipCutsceneLabel(Game);
            Game.Components.Add(Label);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //if (Player.State == MediaState.Stopped)
            //{
            //    Player.IsLooped = true;
            //    Player.Play(Video);
            //}
            if (InputManager.IsKeyboardActivated && InputManager.IsNewKey(Keys.Space))
            {
                Player.Stop();
            }
            if (Player.State == MediaState.Stopped)
            {
                Game.Components.Remove(this);
                Game.Components.Remove(Label);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            VideoTexture = Player.GetTexture();
            if (VideoTexture != null)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(VideoTexture, Screen, Color.White);
                SpriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}

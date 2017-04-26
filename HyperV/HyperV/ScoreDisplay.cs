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
using GameProjectXNA;


namespace HyperV
{
    public class ScoreDisplay : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //Constantes
        const int BOTTOM_MARGIN = 200;
        //const int RIGHT_MARGIN = 700;
        const float NO_ROTATION = 0f;
        const float NO_SCALE = 1f;
        const float FOREGROUND = 0f;

        //Constructor
        readonly string FontName;
        readonly Color FPSColor;
        readonly float UpdateInterval;

        //Initialize
        float TimeElapsedSinceUpdate { get; set; }
        public string Str { get; set; }
        Vector2 LeftCenterPosition { get; set; }
        Vector2 StrPosition { get; set; }

        //LoadContent
        SpriteBatch SpriteMgr { get; set; }
        RessourcesManager<SpriteFont> FontMgr { get; set; }
        SpriteFont CharacterFont { get; set; }
        

        public ScoreDisplay(Game game, string fontName, Color fpsColor, float updateInterval)
           : base(game)
        {
            FontName = fontName;
            FPSColor = fpsColor;
            UpdateInterval = updateInterval;
        }

        public override void Initialize()
        {
            DrawOrder = 1000;
            TimeElapsedSinceUpdate = 0;
            Str = "";
            LeftCenterPosition = new Vector2(30,
                                            Game.Window.ClientBounds.Height - BOTTOM_MARGIN);
            StrPosition = LeftCenterPosition;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            SpriteMgr = new SpriteBatch(Game.GraphicsDevice);
            FontMgr = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            CharacterFont = FontMgr.Find(FontName);
        }

        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += timeElapsed;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                PerformUpdate();
                TimeElapsedSinceUpdate = 0;
            }
        }

        void PerformUpdate()
        {
            //Vector2 dimension = CharacterFont.MeasureString(StrFPS);
            //StrPosition = LeftCenterPosition - dimension;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteMgr.Begin();
            SpriteMgr.DrawString(CharacterFont, Str, StrPosition, FPSColor, NO_ROTATION,
                                      Vector2.Zero, NO_SCALE, SpriteEffects.None, FOREGROUND);
            SpriteMgr.End();
        }
    }
}


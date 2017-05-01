/*
CharacterScript.cs
------------------

By Matthew Godin

Role : Shows the script of the chracter

Created : 2/28/17
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
using System.IO;

namespace HyperV
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CharacterScript : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D FaceImage { get; set; }
        Texture2D ScriptRectangle { get; set; }
        string FaceImageName { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        string TextFile { get; set; }
        InputManager InputManager { get; set; }
        GamePadManager GamePadMgr { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }
        RessourcesManager<SpriteFont> FontManager { get; set; }
        SpriteFont Font { get; set; }
        Rectangle FaceImageRectangle { get; set; }
        Rectangle ScriptRectanglePosition { get; set; }
        string ScriptRectangleName { get; set; }
        Vector2 TextPosition { get; set; }
        Camera2 Camera { get; set; }
        Character Character { get; set; }
        string FontName { get; set; }
        public PressSpaceLabel PressSpaceLabel { get; private set; }

        public CharacterScript(Game game, Character character, string faceImageName, string textFile, string scriptRectangleName, string fontName, float interval) : base(game)
        {
            Character = character;
            FaceImageName = faceImageName;
            TextFile = textFile;
            ScriptRectangleName = scriptRectangleName;
            FontName = fontName;
            Interval = interval;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Visible = false;
            base.Initialize();
            float height = (160f / 600f) * Game.Window.ClientBounds.Height;
            FaceImageRectangle = new Rectangle(10, (int)(Game.Window.ClientBounds.Height - height - 10) - 20, (int)((250f / 800f) * Game.Window.ClientBounds.Width) - 100, (int)height + 20);
            ScriptRectanglePosition = new Rectangle(FaceImageRectangle.X + FaceImageRectangle.Width + 10, FaceImageRectangle.Y + 10, (int)((510f / 800f) * Game.Window.ClientBounds.Width) + 140, (int)((143f / 600f) * Game.Window.ClientBounds.Height) + 25);
            TextPosition = new Vector2(ScriptRectanglePosition.X + 10, ScriptRectanglePosition.Y + 10);
            PressSpaceLabel = new PressSpaceLabel(Game);
            Game.Components.Add(PressSpaceLabel);
        } // 800x600 510x143

        protected override void LoadContent()
        {
            base.LoadContent();
            SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            FaceImage = TextureManager.Find(FaceImageName);
            ScriptRectangle = TextureManager.Find(ScriptRectangleName);
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GamePadMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            FontManager = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            Camera = Game.Services.GetService(typeof(Camera)) as Camera2;
            Font = FontManager.Find(FontName);
            ReadScript();
        }

        void ReadScript()
        {
            StreamReader reader = new StreamReader(TextFile);
            TextFile = "";
            while(!reader.EndOfStream)
            {
                TextFile += reader.ReadLine() + "\n";
            }
            reader.Close();
        }

        float Timer { get; set; }
        float Interval { get; set; }
        bool First { get; set; }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if ((InputManager.IsNewKey(Keys.Space)|| GamePadMgr.IsNewButton(Buttons.Y)))
            {
                Visible = !Visible;
                PressSpaceLabel.Visible = !Visible;
                ManageCollision();
            }
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                ManageCollision();
                Timer = 0;
            }
            base.Update(gameTime);
        }

        void ManageCollision()
        {
            float? collision = Character.Collision(new Ray(Camera.Position, Camera.Direction));
            if (collision > 0.25f || collision == null)
            {
                Visible = false;
                PressSpaceLabel.Visible = false;
                First = true;
            }
            else
            {
                if (First)
                {
                    First = false;
                    PressSpaceLabel.Visible = true;
                }
            }
        }

        //void ManageGrabbing()
        //{
        //    Ray visor = new Ray(Position, Direction);

        //    foreach (GrabbableSphere grabbableSphere in Game.Components.Where(component => component is GrabbableSphere))
        //    {
        //        Game.Window.Title = grabbableSphere.IsColliding(visor).ToString();
        //        if (grabbableSphere.IsColliding(visor) <= 45 &&
        //            grabbableSphere.IsColliding(visor) != null &&
        //            (InputMgr.IsNewLeftClick() || InputMgr.IsOldLeftClick()))
        //        {
        //            grabbableSphere.IsGrabbed = true;
        //        }
        //    }
        //}

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(ScriptRectangle, ScriptRectanglePosition, Color.White);
            SpriteBatch.Draw(FaceImage, FaceImageRectangle, Color.White);
            SpriteBatch.DrawString(Font, TextFile, TextPosition, Color.Black, 0, Vector2.Zero, (float)(ScriptRectanglePosition.Width) / (Game.Window.ClientBounds.Width * 1.6f), SpriteEffects.None, 0);
            SpriteBatch.End();
        }
    }
}

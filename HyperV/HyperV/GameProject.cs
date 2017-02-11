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
using System.IO;
using System.Diagnostics;
using GameProjectXNA;

namespace HyperV
{
    public class GameProject : Microsoft.Xna.Framework.Game
    {
        const float FPS_COMPUTE_INTERVAL = 1f;
        const float UPDATE_INTERVAL_STANDARD = 1f / 60f;
        GraphicsDeviceManager GraphicsMgr { get; set; }

        SubjectiveCamera GameCamera { get; set; }                
        InputManager InputMgr { get; set; }

        //GraphicsDeviceManager GraphicsMgr { get; set; }
        SpriteBatch SpriteMgr { get; set; }

        RessourcesManager<SpriteFont> FontMgr { get; set; }
        RessourcesManager<Texture2D> TextureMgr { get; set; }
        RessourcesManager<Model> ModelMgr { get; set; }
        //Camera GameCamera { get; set; }

        public GameProject()
        {
            GraphicsMgr = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GraphicsMgr.SynchronizeWithGreenicalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            const float OBJECT_SCALE = 0.01f;
            Vector3 objectPosition = new Vector3(0, -10, -50);
            Vector3 objectRotation = new Vector3(0, MathHelper.PiOver2, 0);
            FontMgr = new RessourcesManager<SpriteFont>(this, "Fonts");
            TextureMgr = new RessourcesManager<Texture2D>(this, "Textures");
            ModelMgr = new RessourcesManager<Model>(this, "Models");
            //GameCamera = new StableCamera(this, Vector3.Zero, objectPosition, Vector3.Up);
            GameCamera = new SubjectiveCamera(this, Vector3.Zero, objectPosition, Vector3.Up, UPDATE_INTERVAL_STANDARD);
            InputMgr = new InputManager(this);
            Components.Add(InputMgr);
            Components.Add(new NightSkyBackground(this, "NightSky", UPDATE_INTERVAL_STANDARD));
            Components.Add(GameCamera);
            Components.Add(new Displayer3D(this));
            Components.Add(new BaseObject(this, "ship", OBJECT_SCALE, objectRotation, objectPosition));
            //Components.Add(new TexturePlane(this, 1f, Vector3.Zero, new Vector3(4, 4, -5), new Vector2(20, 20), new Vector2(40, 40), "Grass", UPDATE_INTERVAL_STANDARD));
            Services.AddService(typeof(RessourcesManager<Texture2D>), TextureMgr);
            Components.Add(new Grass(this, 1f, Vector3.Zero, new Vector3(0, 0, 0), new Vector2(256, 256), "Grass", UPDATE_INTERVAL_STANDARD));
            Components.Add(new FPSDisplay(this, "Arial", Color.Tomato, FPS_COMPUTE_INTERVAL));
            Services.AddService(typeof(RessourcesManager<SpriteFont>), FontMgr);
            Services.AddService(typeof(RessourcesManager<Model>), ModelMgr);
            Services.AddService(typeof(InputManager), InputMgr);
            Services.AddService(typeof(Camera), GameCamera);
            SpriteMgr = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), SpriteMgr);
            base.Initialize();
        }
        protected override void Update(GameTime gameTime)
        {
            ManageKeyboard();
            base.Update(gameTime);            
        }

        private void ManageKeyboard()
        {
            if (InputMgr.IsPressed(Keys.Escape))
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}


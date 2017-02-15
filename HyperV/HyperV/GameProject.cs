using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
            InputMgr = new InputManager(this);
            Components.Add(InputMgr);
            //GameCamera = new SubjectiveCamera(this, Vector3.Zero, Vector3.One, Vector3.Up, UPDATE_INTERVAL_STANDARD);
            //Components.Add(GameCamera);
            Components.Add(new Displayer3D(this));
            Components.Add(new FPSDisplay(this, "Arial", Color.Red, FPS_COMPUTE_INTERVAL));
            Components.Add(new Jeu(this));
            Components.Add(new PlayerCamera(this, Vector3.Zero, Vector3.One, Vector3.Up, UPDATE_INTERVAL_STANDARD));
            //Components.Add(new Niveau(this, "ship", new Vector3(0, -10, 0)));
            //Components.Add(new Skybox(this, "Texture_Skybox"));

            Services.AddService(typeof(Random), new Random());

            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(RessourcesManager<SoundEffect>), new RessourcesManager<SoundEffect>(this, "Sounds"));
            Services.AddService(typeof(RessourcesManager<Song>), new RessourcesManager<Song>(this, "Songs"));
            Services.AddService(typeof(RessourcesManager<Texture2D>), new RessourcesManager<Texture2D>(this, "Textures"));
            Services.AddService(typeof(RessourcesManager<TextureCube>), new RessourcesManager<TextureCube>(this, "Textures"));
            Services.AddService(typeof(RessourcesManager<Model>), new RessourcesManager<Model>(this, "Models"));
            Services.AddService(typeof(RessourcesManager<Effect>), new RessourcesManager<Effect>(this, "Effects"));

            Services.AddService(typeof(InputManager), InputMgr);
            //Services.AddService(typeof(SubjectiveCamera), GameCamera);
            Services.AddService(typeof(SpriteBatch), new SpriteBatch(GraphicsDevice));
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


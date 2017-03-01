// By Matthew Godin
// Created on January 2017

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

        Camera Camera { get; set; }
        Maze Maze { get; set; }
        InputManager InputManager { get; set; }

        //GraphicsDeviceManager GraphicsMgr { get; set; }
        SpriteBatch SpriteBatch { get; set; }

        RessourcesManager<SpriteFont> FontManager { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }
        RessourcesManager<Model> ModelManager { get; set; }
        //Camera GameCamera { get; set; }

        public GameProject()
        {
            GraphicsMgr = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GraphicsMgr.SynchronizeWithGreenicalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = false;
            GraphicsMgr.PreferredBackBufferHeight = 800;
            GraphicsMgr.PreferredBackBufferWidth = 1500;

        }

        Grass Grass0 { get; set; }

        RessourcesManager<Video> VideoManager { get; set; }
        CutscenePlayer CutscenePlayer { get; set; }
        Walls Walls { get; set; }
        Character Robot { get; set; }
        List<Character> Characters { get; set; }
        int SaveNumber { get; set; }
        int Level { get; set; }
        Vector3 Position { get; set; }

        void LoadSave()
        {
            StreamReader reader = new StreamReader("F:/programming/HyperV/WPFINTERFACE/Launching Interface/Saves/save.txt");
            SaveNumber = int.Parse(reader.ReadLine());
            reader.Close();
            reader = new StreamReader("F:/programming/HyperV/WPFINTERFACE/Launching Interface/Saves/save" + SaveNumber.ToString() + ".txt");
            string line = reader.ReadLine();
            char[] separator = new char[] { ' ' };
            string[] parts = line.Split(separator);
            Level = int.Parse(parts[1]);
            line = reader.ReadLine();
            reader.Close();
            //parts = line.Split(separator);
            //int startInd = parts[1].IndexOf("X:") + 2;
            //float aXPosition = float.Parse(parts[1].Substring(startInd, parts[1].IndexOf(" Y") - startInd));
            //startInd = parts[1].IndexOf("Y:") + 2;
            //float aYPosition = float.Parse(parts[1].Substring(startInd, parts[1].IndexOf(" Z") - startInd));
            //startInd = parts[1].IndexOf("Z:") + 2;
            //float aZPosition = float.Parse(parts[1].Substring(startInd, parts[1].IndexOf("}") - startInd));
            //Position = new Vector3(aXPosition, aYPosition, aZPosition);
        }

        void SelectWorld()
        {
            switch (Level)
            {
                case 0:
                    Level0();
                    break;
                case 1:
                    Level1();
                    break;
            }
        }

        void Level1()
        {
            //Components.Add(new NightSkyBackground(this, "NightSky", UPDATE_INTERVAL_STANDARD));
            Components.Add(new Displayer3D(this));
            Camera = new PlayerCamera(this, new Vector3(0, -16, 60), new Vector3(20, 0, 0), Vector3.Up, UPDATE_INTERVAL_STANDARD);
            Services.AddService(typeof(Camera), Camera);
            Robot = new Character(this, "Robot", 0.02f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3(-50, -20, 60), "../../../CharacterScripts/Robot.txt", "FaceImages/Robot", "ScriptRectangle");
            Characters.Add(Robot);
            Components.Add(Robot);
            Grass grass = new Grass(this, 1f, Vector3.Zero, new Vector3(20, -20, 50), new Vector2(20, 20), "Grass", UPDATE_INTERVAL_STANDARD);
            Components.Add(grass);
            Services.AddService(typeof(Grass), grass);
            Walls = new Walls(this, UPDATE_INTERVAL_STANDARD, "Rockwall", "../../../Data.txt");
            Components.Add(Walls);
            Services.AddService(typeof(Walls), Walls);
            Components.Add(Camera);
            for (int i = 0; i < 15; ++i)
            {
                for (int j = 0; j < 15; ++j)
                {
                    Components.Add(new Grass(this, 1f, Vector3.Zero, new Vector3(60 - i * 20, -20, 10 + j * 20), new Vector2(20, 20), "Grass", UPDATE_INTERVAL_STANDARD));
                }
            }
            for (int i = 0; i < 15; ++i)
            {
                for (int j = 0; j < 15; ++j)
                {
                    Components.Add(new Ceiling(this, 1f, Vector3.Zero, new Vector3(60 - i * 20, 0, 10 + j * 20), new Vector2(20, 20), "Grass", UPDATE_INTERVAL_STANDARD));
                }
            }
        }

        void Level0()
        {
            CutscenePlayer = new CutscenePlayer(this, "test1", false);
            Components.Add(CutscenePlayer);
        }

        protected override void Initialize()
        {
            TextureManager = new RessourcesManager<Texture2D>(this, "Textures");
            Services.AddService(typeof(RessourcesManager<Texture2D>), TextureManager);
            ModelManager = new RessourcesManager<Model>(this, "Models");
            Services.AddService(typeof(RessourcesManager<Model>), ModelManager);
            FontManager = new RessourcesManager<SpriteFont>(this, "Fonts");
            InputManager = new InputManager(this);
            Components.Add(InputManager);
            Services.AddService(typeof(RessourcesManager<SpriteFont>), FontManager);
            Services.AddService(typeof(InputManager), InputManager);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), SpriteBatch);
            VideoManager = new RessourcesManager<Video>(this, "Videos");
            Services.AddService(typeof(RessourcesManager<Video>), VideoManager);
            Characters = new List<Character>();
            Services.AddService(typeof(List<Character>), Characters);
            LoadSave();
            SelectWorld();

            //const float OBJECT_SCALE = 0.02f;
            //Vector3 objectPosition = new Vector3(-50, -20, 60);
            //Vector3 objectRotation = new Vector3(0, MathHelper.PiOver2, 0);
            ////GameCamera = new StableCamera(this, Vector3.Zero, objectPosition, Vector3.Up);
            ////GameCamera = new SubjectiveCamera(this, new Vector3(0, 0, 0), objectPosition, Vector3.Up, UPDATE_INTERVAL_STANDARD);
            //Components.Add(new NightSkyBackground(this, "NightSky", UPDATE_INTERVAL_STANDARD));
            ////Components.Add(new BaseObject(this, "Robot", OBJECT_SCALE, objectRotation, objectPosition));
            
            //Robot = new Character(this, "Robot", OBJECT_SCALE, objectRotation, objectPosition, "../../../CharacterScripts/Robot.txt", "FaceImages/Robot", "ScriptRectangle");
            //Components.Add(Robot);
            //Characters.Add(Robot);
            //Services.AddService(typeof(List<Character>), Characters);
            ////Components.Add(new TexturePlane(this, 1f, Vector3.Zero, new Vector3(4, 4, -5), new Vector2(20, 20), new Vector2(40, 40), "Grass", UPDATE_INTERVAL_STANDARD));
            
            //Grass grass = new Grass(this, 1f, Vector3.Zero, new Vector3(20, -20, 50), new Vector2(20, 20), "Grass", UPDATE_INTERVAL_STANDARD);
            //Components.Add(grass);
            //for (int i = 0; i < 15; ++i)
            //{
            //    for (int j = 0; j < 15; ++j)
            //    {
            //        Components.Add(new Grass(this, 1f, Vector3.Zero, new Vector3(60 - i * 20, -20, 10 + j * 20), new Vector2(20, 20), "Grass", UPDATE_INTERVAL_STANDARD));
            //    }
            //}
            //for (int i = 0; i < 15; ++i)
            //{
            //    for (int j = 0; j < 15; ++j)
            //    {
            //        Components.Add(new Ceiling(this, 1f, Vector3.Zero, new Vector3(60 - i * 20, 0, 10 + j * 20), new Vector2(20, 20), "Grass", UPDATE_INTERVAL_STANDARD));
            //    }
            //}
            //Services.AddService(typeof(RessourcesManager<TextureCube>), new RessourcesManager<TextureCube>(this, "Textures"));
            //Services.AddService(typeof(RessourcesManager<Effect>), new RessourcesManager<Effect>(this, "Effects"));
            //Maze = new Maze(this, 1f, Vector3.Zero, new Vector3(0, 0, 0), new Vector3(256, 5, 256), "GrassFence", UPDATE_INTERVAL_STANDARD, "Maze");
            //Walls = new Walls(this, UPDATE_INTERVAL_STANDARD, "Rockwall", "../../../Data.txt");
            //Components.Add(Walls);
            //Services.AddService(typeof(Walls), Walls);
            ////Components.Add(Maze);
            ////Services.AddService(typeof(Maze), Maze);
            //Services.AddService(typeof(Grass), grass);
            //Camera = new PlayerCamera(this, new Vector3(0, -16, 60), new Vector3(20, 0, 0), Vector3.Up, UPDATE_INTERVAL_STANDARD);
            //Services.AddService(typeof(Camera), Camera);
            //Components.Add(Camera);
            //Services.AddService(typeof(RessourcesManager<Model>), ModelManager);
            //////Components.Add(new Skybox(this, "Texture_Skybox"));

            //Components.Add(new FPSDisplay(this, "Arial", Color.Tomato, FPS_COMPUTE_INTERVAL));
            //Services.AddService(typeof(RessourcesManager<SpriteFont>), FontMgr);
            //Services.AddService(typeof(InputManager), InputMgr);
            //SpriteMgr = new SpriteBatch(GraphicsDevice);
            //Services.AddService(typeof(SpriteBatch), SpriteMgr);
            //VideoManager = new RessourcesManager<Video>(this, "Videos");
            //Services.AddService(typeof(RessourcesManager<Video>), VideoManager);
            //CutscenePlayer = new CutscenePlayer(this, "test1");
            ////Components.Add(CutscenePlayer);
            base.Initialize();
        }

        float Timer { get; set; }

        protected override void Update(GameTime gameTime)
        {
            ManageKeyboard();
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= UPDATE_INTERVAL_STANDARD)
            {
                CheckForCutscene();
                Timer = 0;
            }
            //Window.Title = GameCamera.Position.ToString();
            base.Update(gameTime);
        }

        void CheckForCutscene()
        {
            if (CutscenePlayer.CutsceneFinished)
            {
                ++Level;
                SelectWorld();
                CutscenePlayer.ResetCutsceneFinished();
            }
        }

        void ManageKeyboard()
        {
            if (InputManager.IsPressed(Keys.Escape))
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




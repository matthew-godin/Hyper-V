using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    public class RythmLevel : Microsoft.Xna.Framework.GameComponent
    {
        //Constructeur
        readonly string NomFichierLecture;
        readonly string TextureName;
        readonly float UpdateInterval;

        float TimeElapsedSinceUpdate { get; set; }
        List<Vector3> Positions { get; set; }

        bool ButtonOne { get; set; }
        bool ButtonTwo { get; set; }
        bool ButtonThree { get; set; }

        int cpt { get; set; }
        int numGotten { get; set; }
        Random RandomNumberGenerator { get; set; }

        InputManager InputMgr { get; set; }
        GamePadManager GamePadMgr { get; set; }

        public Vector3? RedCubePosition { get; set; }

        AfficheurScore Score { get; set; }

        public RythmLevel(Game game, string fileNameLecture, string textureName, float updateInterval)
            : base(game)
        {
            NomFichierLecture = fileNameLecture;
            TextureName = textureName;
            UpdateInterval = updateInterval;
        }

        public override void Initialize()
        {
            base.Initialize();

            ButtonOne = false;
            ButtonTwo = false;
            ButtonThree = false;

            RedCubePosition = null;

            RandomNumberGenerator = new Random();
            numGotten = 0;
            cpt = 0;
            TimeElapsedSinceUpdate = 0;
            Positions = new List<Vector3>();
            InitializePositions();
            Score = new AfficheurScore(Game, "Arial50", Color.Black, UpdateInterval);

            InitializeComponents();
            LoadContent();
        }

        void InitializePositions()
        {
            string lineRead;
            int startIndicator;
            float componentX, componentY, componentZ;

            StreamReader fileReader = new StreamReader(NomFichierLecture);

            while (!fileReader.EndOfStream)
            {
                lineRead = fileReader.ReadLine();

                startIndicator = lineRead.IndexOf("X:") + 2;
                componentX = float.Parse(lineRead.Substring(startIndicator, lineRead.IndexOf(" Y") - startIndicator));

                startIndicator = lineRead.IndexOf("Y:") + 2;
                componentY = float.Parse(lineRead.Substring(startIndicator, lineRead.IndexOf(" Z") - startIndicator));

                startIndicator = lineRead.IndexOf("Z:") + 2;
                componentZ = float.Parse(lineRead.Substring(startIndicator, lineRead.IndexOf("}") - startIndicator));

                Positions.Add(new Vector3(componentX, componentY, componentZ));
            }
            fileReader.Close();
        }

        protected virtual void LoadContent()
        {
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GamePadMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
        }

        void InitializeComponents()
        {

            Game.Components.Add(Score);
            Game.Components.Add(new Displayer3D(Game));

            for(int i = 0; i < Positions.Count; i += 2)
            {
                Game.Components.Add(new TexturedCylinder(Game, 1, new Vector3(0, 0, 0),
                                    Vector3.Zero, new Vector2(1, 1), new Vector2(20, 20),
                                    "Electric Cable", UpdateInterval, Positions[i],
                                    Positions[i+1]));

                Game.Components.Add(new TexturedCube(Game, 1, Vector3.Zero, Positions[i+1],
                                    "White", new Vector3(3, 3, 3), UpdateInterval));

                Game.Components.Add(new TexturedTile(Game, 1, new Vector3(0, -MathHelper.PiOver2, 0),
                                    Positions[i+1] - 1.65f * Vector3.UnitX, new Vector2(3, 3), (i/2+1).ToString()));
            }
        }

        public override void Update(GameTime gameTime)
        {
            LookUpKeys();

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += elapsedTime;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                PerformUpdate();
                TimeElapsedSinceUpdate = 0;
            }
            base.Update(gameTime);
        }

        void LookUpKeys()
        {
            ButtonOne = InputMgr.IsNewKey(Keys.NumPad1)|| 
                      InputMgr.IsNewKey(Keys.D1) ||
                      GamePadMgr.IsPressed(Buttons.DPadLeft) || ButtonOne;
            ButtonTwo = InputMgr.IsNewKey(Keys.NumPad2) || 
                        InputMgr.IsNewKey(Keys.D2) ||
                      GamePadMgr.IsPressed(Buttons.DPadDown) || ButtonTwo;
            ButtonThree = InputMgr.IsNewKey(Keys.NumPad3) || 
                        InputMgr.IsNewKey(Keys.D3) ||
                      GamePadMgr.IsPressed(Buttons.DPadRight) || ButtonThree;
        }

        void PerformUpdate()
        {
            cpt++;

            foreach (TexturedCube cube in Game.Components.Where(component => component is TexturedCube))
            {
                if (AreEqualVectors(RedCubePosition, cube.Position))
                {
                    cube.TextureNameCube = "Red";
                    cube.InitializeBscEffectParameters();
                    RedCubePosition = null;
                }

                foreach (RythmSphere sp in Game.Components.Where(component => component is RythmSphere))
                {
                    if (sp.IsColliding(cube))
                    {
                        if (AreEqualVectors(sp.Extremity1, Positions[0]) && ButtonOne ||
                        AreEqualVectors(sp.Extremity1, Positions[2]) && ButtonTwo ||
                        AreEqualVectors(sp.Extremity1, Positions[4]) && ButtonThree)
                        {
                            sp.ToDestroy = true;
                            cube.TextureNameCube = "Green";
                            cube.InitializeBscEffectParameters();
                            ++numGotten;
                        }
                    }
                }
            }

            Score.Val = numGotten.ToString() + "/15";

            if (cpt > 120)
            {
                int slopeChoice = RandomNumberGenerator.Next(0, 3) * 2;
                //Game.Components.Add(new Displayer3D(Game));
                Game.Components.Add(new RythmSphere(Game, 1, Vector3.Zero,
                                    Positions[slopeChoice], 1, new Vector2(20, 20),
                                    "BlueWhiteRed", UpdateInterval, Positions[slopeChoice + 1]));
                cpt = 0;

                foreach (TexturedCube cube in Game.Components.Where(component => component is TexturedCube))
                {
                    cube.TextureNameCube = "White";
                    cube.InitializeBscEffectParameters();
                }
            }

            ButtonOne = false;
            ButtonTwo = false;
            ButtonThree = false;
        }



        bool AreEqualVectors(Vector3? a, Vector3 b)
        {
            if(a == null)
            {
                return false;
            }

            Vector3 c = (Vector3)a - b;

            return (c.X < 1 && c.X > -1) && (c.Y < 1 && c.Y > -1) && (c.Z < 1 && c.Z > -1);
        }
    }
}

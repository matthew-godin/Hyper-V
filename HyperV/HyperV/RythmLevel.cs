using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XNAProject;

namespace HyperV
{
    //void RythmLevel()
    //{
    //    RythmLevel circuit = new RythmLevel(this, "Electric Cable", "../../../Data3.txt",
    //                                            3, "White", "Red",
    //                                            "Green", "BlueWhiteRed", "Arial50",
    //                                            Color.Black, 15, 1,
    //                                            FpsInterval);
    //    Components.Add(circuit);
    //    Services.AddService(typeof(RythmLevel), circuit);
    //}


        //Take lives if presses key and no collision
    public class RythmLevel : Microsoft.Xna.Framework.GameComponent
    {
        //CONSTRUCTOR
        //Cylinder
        readonly string CylinderTexture,
                        CylinderPositionFileNameReading;
        //Cube
        readonly float CubeEdgeLength;
        readonly string CubeBaseTexture,
                        CubeFailureTexture,
                        CubeSuccessTexture;
        //RythmSphere
        readonly string TextureRythmSphere;
        //Score
        readonly string ScoreFontName;
        readonly Color ScoreColor;
        readonly int NumBallsToSucceed,
                     Difficulty;

        readonly float UpdateInterval;


        //Initialize
        bool ButtonOne { get; set; }
        bool ButtonTwo { get; set; }
        bool ButtonThree { get; set; }
        bool LevelIsCompleted { get; set; }

        float TimeElapsedSinceUpdate { get; set; }
        List<Vector3> Positions { get; set; }
        int i { get; set; }
        int j { get; set; }
        int numGotten { get; set; }
        public Vector3? RedCubePosition { get; set; }
        AfficheurScore Score { get; set; }
        int MaximalThreshold_i { get; set; }
        int MaximalThreshold_j { get; set; }

        //LoadContent
        Random RandomNumberGenerator { get; set; }
        InputManager InputMgr { get; set; }
        GamePadManager GamePadMgr { get; set; }
        List<UnlockableWall> WallToRemove { get; set; }
        List<Portal> PortalList { get; set; }
        

        public RythmLevel(Game game, string cylinderTexture, string cylinderPositionsFileName,
                            float cubeEdgeLength, string cubeBaseTexture, string cubeFailureTexture,
                            string cubeSuccessTexture, string textureRythmSphere, string scoreFontName,
                            Color couleurScore, int numBallsToSucceed, int difficulty, 
                            float updateInterval)
            : base(game)
        {
            CylinderTexture = cylinderTexture;
            CylinderPositionFileNameReading = cylinderPositionsFileName;

            CubeEdgeLength = cubeEdgeLength;
            CubeBaseTexture = cubeBaseTexture;
            CubeFailureTexture = cubeFailureTexture;
            CubeSuccessTexture = cubeSuccessTexture;

            TextureRythmSphere = textureRythmSphere;

            ScoreFontName = scoreFontName;
            ScoreColor = couleurScore;
            NumBallsToSucceed = numBallsToSucceed;
            Difficulty = difficulty;

            UpdateInterval = updateInterval;
        }

        public override void Initialize()
        {
            base.Initialize();

            ButtonOne = false;
            ButtonTwo = false;
            ButtonThree = false;
            LevelIsCompleted = false;
            RedCubePosition = null;
            numGotten = 0;
            i = 0;
            j = 0;
            MaximalThreshold_i = 120;
            MaximalThreshold_j = 20;  // CONSTANTS ________________________________________________________
            TimeElapsedSinceUpdate = 0;

            Positions = new List<Vector3>();
            InitializePositions();
            Score = new AfficheurScore(Game, ScoreFontName, ScoreColor, UpdateInterval);
            LoadContent();
            InitializeComponents();
            
        }

        void InitializePositions()
        {
            string lineRead;
            int startIndicator;
            float componentX, componentY, componentZ;

            StreamReader fileReader = new StreamReader(CylinderPositionFileNameReading);

            while (!fileReader.EndOfStream)
            {
                lineRead = fileReader.ReadLine();
                 // Make function one line
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
            RandomNumberGenerator = Game.Services.GetService(typeof(Random)) as Random;
            WallToRemove = Game.Services.GetService(typeof(List<UnlockableWall>)) as List<UnlockableWall>;
            PortalList = Game.Services.GetService(typeof(List<Portal>)) as List<Portal>;
        }

        void InitializeComponents()
        {
          
            Game.Components.Add(Score);
            Game.Components.Add(new Displayer3D(Game));

            for(int i = 0; i < Positions.Count; i += 2)
            {
                // constants ---------------------------------------------------------------------

                Game.Components.Add(new TexturedCylinder(Game, 1, Vector3.Zero,
                                    Vector3.Zero, new Vector2(1, 1), new Vector2(20, 20),
                                    CylinderTexture, UpdateInterval, Positions[i],
                                    Positions[i+1]));

                Game.Components.Add(new TexturedCube(Game, 1, Vector3.Zero, Positions[i+1],
                                    CubeBaseTexture, new Vector3(CubeEdgeLength, CubeEdgeLength, CubeEdgeLength), UpdateInterval));

                Game.Components.Add(new TexturedTile(Game, 1, new Vector3(0, -MathHelper.PiOver2, 0),
                                    Positions[i+1] - 1.65f * Vector3.UnitX, new Vector2(CubeEdgeLength, CubeEdgeLength), (i/2+1).ToString()));
            }
        }

        public override void Update(GameTime gameTime)
        {
            LookUpKeys();

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += elapsedTime;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                if (!LevelIsCompleted)
                {
                    PerformUpdate();
                }
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
            i++;
            j++;

            foreach (TexturedCube cube in Game.Components.Where(component => component is TexturedCube))
            {
                PutBackInitialCubeTextures(cube);
                ManageFailure(cube);

                foreach (RythmSphere sp in Game.Components.Where(component => component is RythmSphere))
                {
                    if (sp.IsColliding(cube))
                    {
                        ManageSuccess(sp, cube);
                    }
                }
            }

            ManageScore();
            AddSpheres();

            ButtonOne = false;
            ButtonTwo = false;
            ButtonThree = false;
        }

        void ManageScore()
        {
            Score.Val = numGotten.ToString() + "/" + NumBallsToSucceed.ToString();

            if (numGotten >= NumBallsToSucceed)
            {

                // constants  ----------------------------------


                LevelIsCompleted = true;
                i = 1000;
                Game.Components.Remove(WallToRemove[0]);
                PortalList.Add(new Portal(Game, 1, new Vector3(0, MathHelper.PiOver2, 0),
                                  new Vector3(170, -60, -10), new Vector2(40, 40), "Transparent",
                                  1, UpdateInterval));
                Game.Components.Add(PortalList.Last());
            }
        }

        void AddSpheres()
        {
            if (i > MaximalThreshold_i)
            {
                // constants ---------------------------------------------


                if (!LevelIsCompleted)
                {
                    MaximalThreshold_i = RandomNumberGenerator.Next(30/Difficulty, 90/Difficulty);
                    
                    int slopeChoice = RandomNumberGenerator.Next(0, 3) * 2;
                    Game.Components.Add(new Displayer3D(Game));
                    Game.Components.Add(new RythmSphere(Game, 1, Vector3.Zero,
                                        Positions[slopeChoice], 1, new Vector2(20, 20),
                                        TextureRythmSphere, UpdateInterval, Positions[slopeChoice + 1]));
                    
                }
                i = 0;
            }
        }

        void PutBackInitialCubeTextures(TexturedCube cube)
        {
            if (j > MaximalThreshold_j/Difficulty || LevelIsCompleted)
            {
                cube.TextureNameCube = CubeBaseTexture;
                cube.InitializeBscEffectParameters();

                //j = 0;
            }
        }

        void ManageFailure(TexturedCube cube)
        {
            if (AreEqualVectors(RedCubePosition, cube.Position))
            {
                cube.TextureNameCube = CubeFailureTexture;
                cube.InitializeBscEffectParameters();
                RedCubePosition = null;
                j = 0;
            }
        }

        void ManageSuccess(RythmSphere sp, TexturedCube cube)
        {
            if (AreEqualVectors(sp.Extremity1, Positions[0]) && ButtonOne ||
                                    AreEqualVectors(sp.Extremity1, Positions[2]) && ButtonTwo ||
                                    AreEqualVectors(sp.Extremity1, Positions[4]) && ButtonThree)
            {
                sp.ToDestroy = true;
                cube.TextureNameCube = CubeSuccessTexture;
                cube.InitializeBscEffectParameters();
                ++numGotten;
                j = 0;
            }
        }

        bool AreEqualVectors(Vector3? a, Vector3 b)
        {
            bool areEqual;

            if(a == null)
            {
                areEqual = false;
            }
            else
            {
                Vector3 c = (Vector3)a - b;
                areEqual = (c.X < 1 && c.X > -1) && (c.Y < 1 && c.Y > -1) && (c.Z < 1 && c.Z > -1);
            }

            return areEqual;
        }
    }
}

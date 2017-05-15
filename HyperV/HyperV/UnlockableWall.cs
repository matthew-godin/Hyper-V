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
using AtelierXNA;
using System.IO;

namespace HyperV
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class UnlockableWall : BasicPrimitive
    {
        Vector3 Position { get; set; }
        float UpdateInterval { get; set; }
        protected InputManager GestionInput { get; private set; }
        float TimeElapsedSinceUpdate { get; set; }

        const int NUM_TRIANGLES = 2;
        protected Vector3[,] PtsVertices { get; private set; }
        Vector3 Origin { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect BscEffect { get; private set; }

        //VertexPositionColor[] Vertices { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures;
        Texture2D TileTexture;
        VertexPositionTexture[] Vertices { get; set; }
        BlendState GestionAlpha { get; set; }

        Vector2[,] PtsTexture { get; set; }
        string NomTileTexture { get; set; }
        Vector3 CentrePosition { get; set; }

        Vector3 PlanePoint { get; set; }
        Vector2 FirstVertex { get; set; }
        Vector2 SecondVertex { get; set; }
        Vector3 PlaneEquation { get; set; }
        float Magnitude { get; set; }
        int NumberRunesOpenWall { get; set; }
        int NumLevelsCompleted { get; set; }
        int SaveIndex { get; set; }

        List<Rune> RunesList { get; set; }
        ButtonPuzzle ButtonsPuzzle { get; set; }


        private bool IsWhitin(float valeur, float thresholdA, float thresholdB)
        {
            return (valeur >= thresholdA && valeur <= thresholdB || valeur <= thresholdA && valeur >= thresholdB);
        }

        public UnlockableWall(Game game, float saveVal, Vector3 initialRotation, Vector3 initialPosition, Vector2 scale, string nomTileTexture, float updateInterval, int numberRunesOpenWall, int numLevelsCompleted, List<Rune> runesList, int saveIndex) : base(game, SaveIndex, initialRotation, initialPosition)
        {
            NomTileTexture = nomTileTexture;
            UpdateInterval = updateInterval;
            Delta = new Vector2(scale.X, scale.Y);
            Origin = new Vector3(0, -Delta.Y / 2, -Delta.X / 2); //pour centrer la primitive au point (0,0,0)
            NumberRunesOpenWall = numberRunesOpenWall;
            NumLevelsCompleted = numLevelsCompleted;
            RunesList = runesList;
            SaveIndex = saveIndex;
        }

        public override void Initialize()
        {
            NbVertices = NUM_TRIANGLES + 2;
            PtsVertices = new Vector3[2, 2];
            CreatePointArray();
            CreateVertexArray();
            Position = PositionInitiale;
            CentrePosition = Position + Origin;

            if (RotationInitiale.Y == -1.570796f)
            {
                FirstVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsVertices[0, 0].Z, PtsVertices[0, 0].X);
                SecondVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(-PtsVertices[1, 1].Z, PtsVertices[1, 1].X);
            }
            else if (RotationInitiale.Y == 3.141593f)
            {
                FirstVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsVertices[0, 0].X, PtsVertices[0, 0].Z);
                SecondVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(-PtsVertices[1, 1].X, -PtsVertices[1, 1].Z);
            }
            else if (RotationInitiale.Y == 1.570796f)
            {
                FirstVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsVertices[0, 0].Z, PtsVertices[0, 0].X);
                SecondVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsVertices[1, 1].Z, -PtsVertices[1, 1].X);
            }
            else if (RotationInitiale.Y == 0)
            {
                FirstVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsVertices[0, 0].X, PtsVertices[0, 0].Z);
                SecondVertex = new Vector2(PositionInitiale.X, PositionInitiale.Z) + new Vector2(PtsVertices[1, 1].X, PtsVertices[1, 1].Z);
            }
            PlanePoint = new Vector3(SecondVertex.X, 0, SecondVertex.Y);
            Vector2 u2 = SecondVertex - FirstVertex;
            Vector3 u = new Vector3(u2.X, 0, u2.Y);
            Vector3 v = new Vector3(0, Delta.Y * 2, 0);
            PlaneEquation = Vector3.Cross(u, v);
            Magnitude = PlaneEquation.Length();
            base.Initialize();
        }
        
        private void CreatePointArray()
        {
            PtsVertices[0, 0] = new Vector3(Origin.X, Origin.Y, Origin.Z);
            PtsVertices[1, 0] = new Vector3(Origin.X, Origin.Y, Origin.Z - Delta.X);
            PtsVertices[0, 1] = new Vector3(Origin.X, Origin.Y + Delta.Y, Origin.Z);
            PtsVertices[1, 1] = new Vector3(Origin.X, Origin.Y + Delta.Y, Origin.Z - Delta.X);
        }

        protected void CreateVertexArray()
        {
            PtsTexture = new Vector2[2, 2];
            Vertices = new VertexPositionTexture[NbVertices];
            CreatePointArrayTexture();
        }

        private void CreatePointArrayTexture()
        {
            PtsTexture[0, 0] = new Vector2(0, 1);
            PtsTexture[1, 0] = new Vector2(1, 1);
            PtsTexture[0, 1] = new Vector2(0, 0);
            PtsTexture[1, 1] = new Vector2(1, 0);
        }

        protected override void LoadContent()
        {
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            ButtonsPuzzle = Game.Services.GetService(typeof(ButtonPuzzle)) as ButtonPuzzle;
            TileTexture = GestionnaireDeTextures.Find(NomTileTexture);
            BscEffect = new BasicEffect(GraphicsDevice);
            InitializeBscEffectParameters();
            base.LoadContent();
        }

        protected void InitializeBscEffectParameters()
        {
            //BscEffect.VertexColorEnabled = true;
            BscEffect.TextureEnabled = true;
            BscEffect.Texture = TileTexture;
            GestionAlpha = BlendState.AlphaBlend; // Be cautious of this...
        }

        protected override void InitializeVertices() // Is called by base.Initialize()
        {
            int VertexIndex = -1;
            for (int j = 0; j < 1; ++j)
            {
                for (int i = 0; i < 2; ++i)
                {
                    Vertices[++VertexIndex] = new VertexPositionTexture(PtsVertices[i, j], PtsTexture[i, j]);
                    Vertices[++VertexIndex] = new VertexPositionTexture(PtsVertices[i, j + 1], PtsTexture[i, j + 1]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            BlendState oldBlendState = GraphicsDevice.BlendState; // ... and this!
            GraphicsDevice.BlendState = GestionAlpha;
            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.Vue;
            BscEffect.Projection = GameCamera.Projection;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                DrawtriangleStrip();
            }
            GraphicsDevice.BlendState = oldBlendState;
        }

        protected void DrawtriangleStrip()
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices, 0, NUM_TRIANGLES);
        }

        public Vector3 GetPositionWithHeight(Vector3 position, int hauteur)
        {
            Vector3 positionWithHeight;
            if (IsWhitin(position.Z, PtsVertices[0, 0].Z, PtsVertices[PtsVertices.GetLength(0) - 1, PtsVertices.GetLength(1) - 1].Z) &&
                IsWhitin(position.X, PtsVertices[0, 0].X, PtsVertices[PtsVertices.GetLength(0) - 1, PtsVertices.GetLength(1) - 1].X))
            {
                positionWithHeight = new Vector3(position.X, PtsVertices[0, 0].Y + hauteur, position.Z);
            }
            else
            {
                positionWithHeight = position;
            }
            return positionWithHeight;
        }

        const int MAX_DISTANCE = 1;

        public bool CheckForCollisions(Vector3 Position)
        {
            Vector3 AP;
            bool result = false;
            float wallDistance;
            if (Visible)
            {
                AP = Position - PlanePoint;
                wallDistance = Vector2.Distance(FirstVertex, SecondVertex);
                result = Math.Abs(Vector3.Dot(AP, PlaneEquation)) / Magnitude < MAX_DISTANCE && (Position - new Vector3(FirstVertex.X, Position.Y, FirstVertex.Y)).Length() < wallDistance && (Position - new Vector3(SecondVertex.X, Position.Y, SecondVertex.Y)).Length() < wallDistance;
            }
            return result;
        }

        public bool RunePuzzleCompleted()
        {
            StreamReader save = new StreamReader("../../../WPFINTERFACE/Launching Interface/Saves/SavePuzzleRunes" + SaveIndex + ".txt");
            string saveLine = save.ReadLine();
            save.Close();
            return (RunesList[0].IsActivated && !RunesList[1].IsActivated && RunesList[2].IsActivated && !RunesList[3].IsActivated && !RunesList[4].IsActivated && RunesList[5].IsActivated) || (saveLine == "True");
        }

        public override void Update(GameTime gameTime)
        {
            if (NumberRunesOpenWall <= NumLevelsCompleted)
            {
                if (NumberRunesOpenWall == 1 && RunePuzzleCompleted())
                {
                    Visible = false;
                    Game.Components.Remove(this);
                }
                if (NumberRunesOpenWall == 2 || NumberRunesOpenWall == 5)
                {
                    Visible = false;
                    Game.Components.Remove(this);
                }
                if (NumberRunesOpenWall == 3 && ButtonsPuzzle.IsCompleted)
                {
                    Visible = false;
                    Game.Components.Remove(this);
                }
            }
        }
    }
}

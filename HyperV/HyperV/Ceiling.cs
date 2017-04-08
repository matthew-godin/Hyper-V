/*
Ceiling.cs
----------

By Matthew Godin

Role : Used to create a flat ceiling surface

Created : 2/27/17
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
    public class Ceiling : BasicPrimitive
    {
        const int NUM_TRIANGLES_PER_TILE = 2, NUM_VERTICES_PER_TRIANGLE = 3;

        Vector3 Position { get; set; }
        float UpdateInterval { get; set; }
        protected InputManager InputMgr { get; private set; }
        float TimeElapsedSinceUpdate { get; set; }

        const int NUM_TRIANGLES = 2;
        protected Vector3[,] VerticesPositions { get; private set; }
        Vector3 Origin { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect BasicEffect { get; private set; }

        //VertexPositionColor[] Vertices { get; set; }
        RessourcesManager<Texture2D> TextureManager;
        Texture2D TileTexture;
        VertexPositionTexture[] Vertices { get; set; }
        BlendState AlphaMgr { get; set; }

        Vector2[,] TileTexturePositions { get; set; }
        string NomTileTexture { get; set; }

        int NumRows { get; set; }
        int NumColumns { get; set; }

        public Vector3 GetPositionWithHeight(Vector3 position, int height)
        {
            Vector3 positionWithHeight;
            if(IsWithin(position.Z, VerticesPositions[0,0].Z, VerticesPositions[VerticesPositions.GetLength(0)-1, VerticesPositions.GetLength(1)-1].Z) &&
                IsWithin(position.X, VerticesPositions[0, 0].X, VerticesPositions[VerticesPositions.GetLength(0) - 1, VerticesPositions.GetLength(1) - 1].X))
            {
                positionWithHeight = new Vector3(position.X, VerticesPositions[0, 0].Y + height, position.Z);
            }
            else
            {
                positionWithHeight = position;
            }
            return positionWithHeight;
        }

        private bool IsWithin(float value, float thresholdA, float thresholdB)
        {
            return (value >= thresholdA && value <= thresholdB || value <= thresholdA && value >= thresholdB);
        }

        public Ceiling(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector2 span, string nomTileTexture, Vector2 numbers, float updateInterval) : base(game, initialScale, initialRotation, initialPosition)
        {
            NomTileTexture = nomTileTexture;
            UpdateInterval = updateInterval;
            Delta = new Vector2(span.X, span.Y);
            //Origin = new Vector3(-Delta.X / 2, 0, -Delta.Y / 2); //to center the primitive to point (0,0,0)
            NumRows = (int)numbers.X;
            NumColumns = (int)numbers.Y;
        }

        public override void Initialize()
        {
            NumTriangles = NumRows * NumColumns * NUM_TRIANGLES_PER_TILE;
            NumVertices = NumTriangles * NUM_VERTICES_PER_TRIANGLE;
            Origin = new Vector3(0, 0, 0);
            VerticesPositions = new Vector3[NumRows * 2, NumColumns * 2];
            CreateVerticesPositions();
            TileTexturePositions = new Vector2[2, 2];
            Vertices = new VertexPositionTexture[NumVertices];
            CreateTexturePositions();
            Position = InitialPosition;
            base.Initialize();
        }

        private void CreateVerticesPositions()
        {
            for (int i = 0; i < VerticesPositions.GetLength(0); i += 2)
            {
                for (int j = 0; j < VerticesPositions.GetLength(1); j += 2)
                {
                    VerticesPositions[i, j] = Origin + new Vector3(Delta.X * i - Delta.X, Origin.Y, Delta.Y * j - Delta.Y);
                    VerticesPositions[i, j + 1] = Origin + new Vector3(Delta.X * i + Delta.X, Origin.Y, Delta.Y * j - Delta.Y);
                    VerticesPositions[i + 1, j] = Origin + new Vector3(Delta.X * i - Delta.X, Origin.Y, Delta.Y * j + Delta.Y);
                    VerticesPositions[i + 1, j + 1] = Origin + new Vector3(Delta.X * i + Delta.X, Origin.Y, Delta.Y * j + Delta.Y);
                }
            }
            //VerticesPositions[0, 0] = new Vector3(Origin.X, Origin.Y, Origin.Z);
            //VerticesPositions[1, 0] = new Vector3(Origin.X - Delta.X, Origin.Y, Origin.Z);
            //VerticesPositions[0, 1] = new Vector3(Origin.X, Origin.Y, Origin.Z + Delta.Y);
            //VerticesPositions[1, 1] = new Vector3(Origin.X - Delta.X, Origin.Y, Origin.Z + Delta.Y);
        }

        private void CreateTexturePositions()
        {
            TileTexturePositions[0, 0] = new Vector2(0, 1);
            TileTexturePositions[1, 0] = new Vector2(1, 1);
            TileTexturePositions[0, 1] = new Vector2(0, 0);
            TileTexturePositions[1, 1] = new Vector2(1, 0);
        }

        protected override void LoadContent()
        {
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            TileTexture = TextureManager.Find(NomTileTexture);
            BasicEffect = new BasicEffect(GraphicsDevice);
            InitializeBasicEffectParameters();
            base.LoadContent();
        }

        protected void InitializeBasicEffectParameters()
        {
            //BscEffect.VertexColorEnabled = true;
            BasicEffect.TextureEnabled = true;
            BasicEffect.Texture = TileTexture;
            AlphaMgr = BlendState.AlphaBlend; // Be careful of this...
        }

        protected override void InitializeVertices() // Is called by base.Initialize()
        {
            int cpt = -1, maxJ = VerticesPositions.GetLength(1), maxI = VerticesPositions.GetLength(0);
            for (int j = 0; j < NumColumns * 2 ; j += 2)
            {
                for (int i = 0; i < NumRows * 2 ; i += 2)
                {
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j], TileTexturePositions[0, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j], TileTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1 == maxJ ? j : j + 1], TileTexturePositions[1, 0]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j], TileTexturePositions[0, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i + 1 == maxI ? i : i + 1, j + 1 == maxJ ? j : j + 1], TileTexturePositions[1, 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1 == maxJ ? j : j + 1], TileTexturePositions[1, 0]);
                    //Vertices[++VertexIndex] = new VertexPositionColor(VerticesPts[i, j], Color.LawnGreen);
                    //Vertices[++VertexIndex] = new VertexPositionColor(VerticesPts[i, j + 1], Color.LawnGreen);
                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j], TileTexturePositions[i, j]);
                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPositions[i, j + 1], TileTexturePositions[i, j + 1]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //BlendState oldBlendState = GraphicsDevice.BlendState; // ... and this!
            //GraphicsDevice.BlendState = AlphaMgr;
            //BasicEffect.World = GetWorld();
            //BasicEffect.View = GameCamera.View;
            //BasicEffect.Projection = GameCamera.Projection;
            //foreach (EffectPass passEffect in BasicEffect.CurrentTechnique.Passes)
            //{
            //    passEffect.Apply();
            //    DrawTriangleStrip();
            //}
            //GraphicsDevice.BlendState = oldBlendState;
            BasicEffect.World = GetWorld();
            BasicEffect.View = GameCamera.View;
            BasicEffect.Projection = GameCamera.Projection;
            foreach (EffectPass passEffect in BasicEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, 0, NumTriangles);
            }
            //GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, 0, NumTriangles);
        }

        //protected void DrawTriangleStrip()
        //{
        //    //GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Vertices, 0, NUM_TRIANGLES);
        //    GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, 0, NUM_TRIANGLES);
        //}

        public Vector3 GetPositionWithHeight(Vector3 position, int height)
        {
            Vector3 positionWithHeight;
            if (IsWithin(position.Z, VerticesPositions[0, 0].Z, VerticesPositions[VerticesPositions.GetLength(0) - 1, VerticesPositions.GetLength(1) - 1].Z) &&
                IsWithin(position.X, VerticesPositions[0, 0].X, VerticesPositions[VerticesPositions.GetLength(0) - 1, VerticesPositions.GetLength(1) - 1].X))
            {
                positionWithHeight = new Vector3(position.X, VerticesPositions[0, 0].Y + height, position.Z);
            }
            else
            {
                positionWithHeight = position;
            }
            return positionWithHeight;
        }
    }
}
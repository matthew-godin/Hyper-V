/*
Grass.cs
--------

By Matthew Godin

Role : Used to create a flat grass surface

Created : 2/13/17
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
    public class Grass : BasicPrimitive
    {
        Vector3 Position { get; set; }
        float UpdateInterval { get; set; }
        protected InputManager InputMgr { get; private set; }
        float TimeElapsedSinceUpdate { get; set; }

        const int NUM_TRIANGLES = 2;
        protected Vector3[,] VerticesPts { get; private set; }
        Vector3 Origin { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect BscEffect { get; private set; }

        //VertexPositionColor[] Vertices { get; set; }
        RessourcesManager<Texture2D> TextureMgr;
        Texture2D TileTexture;
        VertexPositionTexture[] Vertices { get; set; }
        BlendState AlphaMgr { get; set; }

        Vector2[,] TexturePts { get; set; }
        string NomTileTexture { get; set; }


        public Vector3 GetPositionWithHeight(Vector3 position, int height)
        {
            Vector3 positionWithHeight;
            if(IsWithin(position.Z, VerticesPts[0,0].Z, VerticesPts[VerticesPts.GetLength(0)-1, VerticesPts.GetLength(1)-1].Z) &&
                IsWithin(position.X, VerticesPts[0, 0].X, VerticesPts[VerticesPts.GetLength(0) - 1, VerticesPts.GetLength(1) - 1].X))
            {
                positionWithHeight = new Vector3(position.X, VerticesPts[0, 0].Y + height, position.Z);
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

        public Grass(Game game, float initialScale, Vector3 initialRotation,
                     Vector3 initialPosition, Vector2 span, string nomTileTexture,
                     float updateInterval) 
            : base(game, initialScale, initialRotation, initialPosition)
        {
            NomTileTexture = nomTileTexture;
            UpdateInterval = updateInterval;
            Delta = new Vector2(span.X, span.Y);
            Origin = new Vector3(-Delta.X / 2, 0, -Delta.Y / 2); //to center the primitive to point (0,0,0)
        }

        public override void Initialize()
        {
            NumVertices = NUM_TRIANGLES + 2;
            VerticesPts = new Vector3[2, 2];
            CreatePointArray();
            CreateVertexArray();
            Position = InitialPosition;
            base.Initialize();
        }

        private void CreatePointArray()
        {
            VerticesPts[0, 0] = new Vector3(Origin.X, Origin.Y, Origin.Z);
            VerticesPts[1, 0] = new Vector3(Origin.X - Delta.X, Origin.Y, Origin.Z);
            VerticesPts[0, 1] = new Vector3(Origin.X, Origin.Y, Origin.Z + Delta.Y);
            VerticesPts[1, 1] = new Vector3(Origin.X - Delta.X, Origin.Y, Origin.Z + Delta.Y);
        }

        protected void CreateVertexArray()
        {
            TexturePts = new Vector2[2, 2];
            Vertices = new VertexPositionTexture[NumVertices];
            CreatePointArrayTexture();
        }

        private void CreatePointArrayTexture()
        {
            TexturePts[0, 0] = new Vector2(0, 1);
            TexturePts[1, 0] = new Vector2(1, 1);
            TexturePts[0, 1] = new Vector2(0, 0);
            TexturePts[1, 1] = new Vector2(1, 0);
        }

        protected override void LoadContent()
        {
            TextureMgr = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            TileTexture = TextureMgr.Find(NomTileTexture);
            BscEffect = new BasicEffect(GraphicsDevice);
            InitializeBscEffectParameters();
            base.LoadContent();
        }

        protected void InitializeBscEffectParameters()
        {
            //BscEffect.VertexColorEnabled = true;
            BscEffect.TextureEnabled = true;
            BscEffect.Texture = TileTexture;
            AlphaMgr = BlendState.AlphaBlend; // Be careful of this...
        }

        protected override void InitializeVertices() // Is called by base.Initialize()
        {
            int VertexIndex = -1;
            for (int j = 0; j < 1; ++j)
            {
                for (int i = 0; i < 2; ++i)
                {
                    //Vertices[++VertexIndex] = new VertexPositionColor(VerticesPts[i, j], Color.LawnGreen);
                    //Vertices[++VertexIndex] = new VertexPositionColor(VerticesPts[i, j + 1], Color.LawnGreen);
                    Vertices[++VertexIndex] = new VertexPositionTexture(VerticesPts[i, j], TexturePts[i, j]);
                    Vertices[++VertexIndex] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[i, j + 1]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            BlendState oldBlendState = GraphicsDevice.BlendState; // ... and this!
            GraphicsDevice.BlendState = AlphaMgr;
            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.View;
            BscEffect.Projection = GameCamera.Projection;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                DrawTriangleStrip();
            }
            GraphicsDevice.BlendState = oldBlendState;
        }

        protected void DrawTriangleStrip()
        {
            //GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Vertices, 0, NUM_TRIANGLES);
            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices, 0, NUM_TRIANGLES);
        }
    }
}

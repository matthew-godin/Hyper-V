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
    public class TexturedCylinder : BasicAnimatedPrimitive//, ICollidable
    {
        //Initially managed by the constructor
        //readonly Vector2 Delta;
        readonly int NumColumns;
        readonly int NumLines;

        protected string TextureName { get; set; }

        readonly Vector3 Origin;

        //Initially managed by functions called by base.Initialize()
        Vector3[,] VerticesPts { get; set; }
        Vector2[,] TexturePts { get; set; }
        VertexPositionTexture[] Vertices { get; set; }
        BasicEffect BscEffect { get; set; }

        int NumTrianglesPerStrip { get; set; }

        //Initially managed by LoadContent()
        RessourcesManager<Texture2D> TextureMgr { get; set; }
        Texture2D CylinderTexture { get; set; }

        //public bool IsColliding(object otherObject)
        //{
        //    return CollisionSphere.Intersects((otherObject as TexturedCube).CollisionSphere);
        //}

        //public BoundingSphere CollisionSphere { get { return new BoundingSphere(Position, Radius); } }

        Vector3 Extremity1 { get; set; }
        Vector3 Extremity2 { get; set; }



        public TexturedCylinder(Game game, float initialScale, Vector3 initialRotation,
                              Vector3 initialPosition, Vector2 span, Vector2 dimensions,
                              string textureName, float updateInterval, Vector3 extremity1, Vector3 extremity2)
            : base(game, initialScale, initialRotation, initialPosition, updateInterval)
        {
            //Delta = span / dimensions;
            NumColumns = (int)dimensions.X;
            NumLines = (int)dimensions.Y;
            TextureName = textureName;

            //Origin = new Vector3(0,0,0);
            Origin = new Vector3(-span.X / 2, -span.Y / 2, 0);

            Extremity1 = extremity1;
            Extremity2 = extremity2;

        }

        public override void Initialize()
        {
            NumTrianglesPerStrip = NumColumns * 2;
            NumVertices = (NumTrianglesPerStrip + 2) * NumLines;

            AllocateArrays();
            base.Initialize();
            InitializeBscEffectParameters();
        }

        void AllocateArrays()
        {
            VerticesPts = new Vector3[NumColumns + 1, NumLines + 1];
            TexturePts = new Vector2[NumColumns + 1, NumLines + 1];
            Vertices = new VertexPositionTexture[NumVertices];
        }

        protected void InitializeBscEffectParameters()
        {
            CylinderTexture = TextureMgr.Find(TextureName);
            BscEffect = new BasicEffect(GraphicsDevice);
            BscEffect.TextureEnabled = true;
            BscEffect.Texture = CylinderTexture;
        }

        protected override void InitializeVertices()
        {
            PopulateVerticesPts();
            PopulateTexturePts();
            PopulateVertices();
        }

        void PopulateVerticesPts()
        {
            for (int i = 0; i < VerticesPts.GetLength(0); ++i)
            {
                for (int j = 0; j < VerticesPts.GetLength(1); ++j)
                {
                    VerticesPts[i, j] = new Vector3(Origin.X - (i/NumColumns * (Extremity2 - Extremity1).Length()* (Vector3.Normalize(Extremity1 - Extremity2).X)) + Extremity1.X,
                                                   Origin.Y - (i / NumColumns * (Extremity2 - Extremity1).Length() * (Vector3.Normalize(Extremity1 - Extremity2).Y)) + (float)Math.Cos(j * 2 * Math.PI / NumLines) + Extremity1.Y,
                                                   Origin.Z - (i / NumColumns * (Extremity2 - Extremity1).Length() * (Vector3.Normalize(Extremity1 - Extremity2).Z))+(float)Math.Sin(j * 2 * Math.PI / NumLines) + Extremity1.Z);
                }
            }
        }

        void PopulateTexturePts()
        {
            for (int i = 0; i < TexturePts.GetLength(0); ++i)
            {
                for (int j = 0; j < TexturePts.GetLength(1); ++j)
                {
                    TexturePts[i, j] = new Vector2(i / (float)NumColumns, -j / (float)NumLines);
                }
            }
        }

        protected void PopulateVertices()
        {
            int VertexIndex = -1;
            for (int j = 0; j < NumLines; ++j)
            {
                for (int i = 0; i < NumColumns + 1; ++i)
                {
                    Vertices[++VertexIndex] = new VertexPositionTexture(VerticesPts[i, j], TexturePts[i, j]);
                    Vertices[++VertexIndex] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[i, j + 1]);
                }
            }
        }

        protected override void LoadContent()
        {
            TextureMgr = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.View;
            BscEffect.Projection = GameCamera.Projection;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                for (int i = 0; i < NumLines; ++i)
                {
                    DrawTriangleStrip(i);
                }
            }
        }

        void DrawTriangleStrip(int stripIndex)
        {
            int vertexOffset = (stripIndex * NumVertices) / NumLines;
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertices, vertexOffset, NumTrianglesPerStrip);
        }

    }
}

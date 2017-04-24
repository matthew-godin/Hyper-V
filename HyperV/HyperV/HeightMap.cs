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
    public class HeightMap : BasicPrimitive
    {
        const int NUM_TRIANGLES_PAR_TUILE = 2;
        const int NUM_VERTICES_PER_TRIANGLE = 3;
        const float MAX_COLOR = 255f;

        const int NO_VERTEX_OFFSET = 0;
        const int BEFORE_FIRST_VERTEX = -1;
        const int NUM_TRIANGLES = 2;
        const int NULL_SIDE = 0;
        const int FIRST_VERTICES_OF_STRIP = 2;

        const int SUPPLEMENTARY_VERTEX_FOR_LINE = 1;
        const int NULL_COMPENSATION = 0;
        const int HALF_DIVISOR = 2;
        const int NUM_TEXTURE_PTS_POSSIBLE = 4;
        const int NUM_VERTICES_PER_TILE = 4;
        const int NULL_Y = 0;
        const int TEXEL_POSITION_REMOVED_RELATIVE_TO_DIMENSION = 1;
        const int J_MAX_VALUE_TOP_TILE = 1;
        const int TOP_TILE_VALUE = 0;
        const int BOTTOM_TILE_VALUE = 1;

        Vector3 Span { get; set; }
        string GroundMapName { get; set; }
        string[] TextureNameTerrain { get; set; }
        int NumTextureLevels { get; set; }

        BasicEffect BscEffect { get; set; }
        RessourcesManager<Texture2D> TextureMgr { get; set; }
        Texture2D GroundMap { get; set; }
        Vector3 Origin { get; set; }

        // to complete with the properties that will be necessary to implement the component
        int NumRows { get; set; }
        int NumColumns { get; set; }
        Color[] TextureMapData { get; set; }
        int TileWidth { get; set; }
        Vector3[,] VerticesPts { get; set; }
        Vector2[,] TexturePts { get; set; }
        VertexPositionTexture[] Vertices { get; set; }
        Vector2 Delta { get; set; }
        //int NumTexels { get; set; }
        float[,] Heights { get; set; }

        Texture2D CombinedTexture { get; set; }
        Texture2D SandTexture { get; set; }
        Texture2D GrassTexture { get; set; }

        public HeightMap(Game game, float initialScale, Vector3 initialRotation,
                        Vector3 initialPosition, Vector3 span, string nomGroundMap,
                        string[] textureNameTerrain)
            : base(game, initialScale, initialRotation, 
                   initialPosition)
      {
            Span = span;
            GroundMapName = nomGroundMap;
            TextureNameTerrain = textureNameTerrain;
        }

        void InitializeMapData()
        {
            GroundMap = TextureMgr.Find(GroundMapName);
            TextureMapData = new Color[GroundMap.Width * GroundMap.Height];
            GroundMap.GetData<Color>(TextureMapData);
        }

        void InitializeTextureData()
        {
            SandTexture = TextureMgr.Find(TextureNameTerrain[0]);
            GrassTexture = TextureMgr.Find(TextureNameTerrain[1]);
            TileWidth = (int)(SandTexture.Height / (float)NumTextureLevels);
        }

        public override void Initialize()
        {
            TextureMgr = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;

            InitializeMapData();
            InitializeTextureData();

            NumRows = GroundMap.Width - SUPPLEMENTARY_VERTEX_FOR_LINE;
            NumColumns = GroundMap.Height - SUPPLEMENTARY_VERTEX_FOR_LINE;
            NumTriangles = NumRows * NumColumns * NUM_TRIANGLES_PAR_TUILE;
            NumVertices = NumTriangles * NUM_VERTICES_PER_TRIANGLE;
            Heights = new float[GroundMap.Width, GroundMap.Height];
            Origin = new Vector3(/*-Span.X / HALF_DIVISOR, 0, -Span.Z / HALF_DIVISOR*/0, 0, 0); //to center the primitive to point (0,0,0)##########################Moins � Z ajout�

            AllocateArrays(); // ################## INVERS�
            CreateArrayVerticesPts(); // ############### INVERS�
            CreateArrayTexturePts();
            CreateCombinedTexture();
            InitializeVertices();

            base.Initialize();
        }

        void AllocateArrays()
        {
            Vertices = new VertexPositionTexture[NumVertices];
            TexturePts = new Vector2[GroundMap.Width, GroundMap.Height];
            //TexturePts = new Vector2[2, 2];
            VerticesPts = new Vector3[GroundMap.Width, GroundMap.Height];
            //VerticesPts = new Vector3[GroundMap.Width, GroundMap.Height];
            //Delta = new Vector2(Span.X / NumRows, Span.Z / NumColumns);

        }

        void CreateArrayTexturePts()
        {
            for (int i = 0; i < TexturePts.GetLength(0); ++i)
            {
                for (int j = 0; j < TexturePts.GetLength(1); ++j)
                {
                    TexturePts[i, j] = new Vector2(i / (float)NumColumns, -j / (float)NumRows);
                }
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            BscEffect = new BasicEffect(GraphicsDevice);
            InitializeBscEffectParameters();
        }

        void InitializeBscEffectParameters()
        {
            BscEffect.TextureEnabled = true;
            BscEffect.Texture = CombinedTexture;
        }

        //
        // Cr�ation du tableau des points de sommets (on cr�e les points)
        // Ce processus implique la transformation des points 2D de la texture en coordonn�es 3D du terrain
        //
        private void CreateArrayVerticesPts()
        {
            Delta = new Vector2(Span.X / NumRows, Span.Z / NumColumns);
            for (int i = 0; i < VerticesPts.GetLength(0); ++i)
            {
                for (int j = 0; j < VerticesPts.GetLength(1); ++j)
                {
                    VerticesPts[i, j] = Origin + new Vector3(Delta.X * i, TextureMapData[j * VerticesPts.GetLength(1) + i].B / MAX_COLOR * Span.Y, Delta.Y * j);
                }
            }
        }

        //
        // Creation of the vertices.
        // Don't forget this is a TriangleList...
        //
        protected override void InitializeVertices()
        {
            // FireBall
            int cpt = -1;
            for (int j = 0; j < NumRows; ++j)
            {
                for (int i = 0; i < NumColumns; ++i)
                {
                    //int val1 = (int)(VerticesPts[i, j].Y + VerticesPts[i + 1, j].Y + VerticesPts[i, j + 1].Y) / 3;
                    //int woho = (int)(val1 / MAX_COLOR);

                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j], TexturePts[i, j]);
                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j], TexturePts[i + 1, j]);
                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[i, j + 1]);

                    ////Associ�Truc(ref Vertices[cpt - 2], ref Vertices[cpt - 1], ref Vertices[cpt]);

                    //int val2 = (int)(((VerticesPts[i + 1, j].Y + VerticesPts[i + 1, j + 1].Y + VerticesPts[i, j + 1].Y) / 3 - Origin.Y) / Delta.Y);
                    //woho = (int)(val2 / MAX_COLOR);

                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j], TexturePts[i + 1, j]);
                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j + 1], TexturePts[i + 1, j + 1]);
                    //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[i, j + 1]);
                    AffecterTuile(ref cpt, i, j);
                }
            }
        }

        void AffecterTuile(ref int cpt, int i, int j)
        {
            int noCase = (int)((VerticesPts[i, j].Y + VerticesPts[i + 1, j].Y + VerticesPts[i, j + 1].Y + VerticesPts[i + 1, j + 1].Y) / 4.0f / Span.Y * 19) / 4 * 4;

            //for (int k = 0; k < 4; ++k)
            //{
            //    Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j], TexturePts[k + noCase * ]);
            //} 

            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j], TexturePts[i, j]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j], TexturePts[i + 1, j]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[i, j + 1]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j], TexturePts[i + 1, j]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j + 1], TexturePts[i + 1, j + 1]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[i, j + 1]);

            //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j], TexturePts[i+1, j+1]);
            //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j], TexturePts[i, j+1]);
            //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[i+1, j]);
            //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j], TexturePts[i, j+1]);
            //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j + 1], TexturePts[i, j]);
            //Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[i+1, j]);


        }

        //
        // Deviner ce que fait cette m�thode...
        //
        public override void Draw(GameTime gameTime)
        {
            // FireBall
            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.View;
            BscEffect.Projection = GameCamera.Projection;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, NULL_COMPENSATION, Vertices.Length / NUM_VERTICES_PER_TRIANGLE);
            }
        }

        public float GetHeight(Vector3 positionReceived)
        {
            Vector3 position = positionReceived - InitialPosition;
            int i = (int)(position.X / Delta.X), j = (int)(position.Z / Delta.Y);
            Vector3 n;
            float height;
            if (i >= 0 && j >= 0 && i + 1 < /*Heights.GetLength(0)*/VerticesPts.GetLength(0) && j + 1 < VerticesPts.GetLength(1)/*Heights.GetLength(1)*/)
            {
                n = Vector3.Cross(VerticesPts[i + 1, j] - VerticesPts[i, j], VerticesPts[i, j + 1] - VerticesPts[i, j]);
                height = (n.X * VerticesPts[i, j].X + n.Y * VerticesPts[i, j].Y + n.Z * VerticesPts[i, j].Z - n.X * position.X - n.Z * position.Z) / n.Y + 5;
                //height = Heights[i, j];
            }
            else
            {
                height = 5;//position.Y;
            }
            return height;
        }

        void CreateCombinedTexture()
        {
            CombinedTexture = new Texture2D(SandTexture.GraphicsDevice, VerticesPts.GetLength(0), VerticesPts.GetLength(1));
            int nbTexels = VerticesPts.GetLength(0) * VerticesPts.GetLength(0);
            Color[] texels = new Color[nbTexels];
            SandTexture.GetData(texels);

            Color[] sandTexels = new Color[SandTexture.Width * SandTexture.Height];
            SandTexture.GetData(sandTexels);

            Color[] grassTexels = new Color[GrassTexture.Width * GrassTexture.Height];
            GrassTexture.GetData(grassTexels);

            for (int texelIndex = 0; texelIndex < nbTexels; ++texelIndex)
            {
                float percent = GetPercent(texelIndex);

                texels[texelIndex].R = (byte)((percent * (byte)grassTexels[texelIndex].R) + (byte)((1 - percent) * (byte)sandTexels[texelIndex].R));
                texels[texelIndex].G = (byte)((percent * (byte)grassTexels[texelIndex].G) + (byte)((1 - percent) * (byte)sandTexels[texelIndex].G));
                texels[texelIndex].B = (byte)((percent * (byte)grassTexels[texelIndex].B) + (byte)((1 - percent) * (byte)sandTexels[texelIndex].B));
            }

            CombinedTexture.SetData<Color>(texels);
        }

        float GetPercent(int texelIndex)
        {
            return TextureMapData[texelIndex].B / MAX_COLOR;
        }
    }
}

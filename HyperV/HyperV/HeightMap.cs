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
        const int NUM_TEXTURE_PTS_POSSIBLE = 20;
        const int NUM_VERTICES_PER_TILE = 4;
        const int NULL_Y = 0;
        const int TEXEL_POSITION_REMOVED_RELATIVE_TO_DIMENSION = 1;
        const int J_MAX_VALUE_TOP_TILE = 1;
        const int TOP_TILE_VALUE = 0;
        const int BOTTOM_TILE_VALUE = 1;

        Vector3 Span { get; set; }
        string GroundMapName { get; set; }
        string TextureNameTerrain { get; set; }
        int NumTextureLevels { get; set; }

        BasicEffect BscEffect { get; set; }
        RessourcesManager<Texture2D> TextureMgr { get; set; }
        Texture2D GroundMap { get; set; }
        Texture2D TextureTerrain { get; set; }
        Vector3 Origin { get; set; }

        // to complete with the properties that will be necessary to implement the component
        int NumRows { get; set; }
        int NumColumns { get; set; }
        Color[] DataTexture { get; set; }
        int TileWidth { get; set; }
        Vector3[,] VerticesPts { get; set; }
        Vector2[] TexturePts { get; set; }
        VertexPositionTexture[] Vertices { get; set; }
        Vector2 Delta { get; set; }
        int NumTexels { get; set; }
        float[,] Heights { get; set; }

        public HeightMap(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector3 span, string nomGroundMap, string textureNameTerrain) : base(game, initialScale, initialRotation, initialPosition)
      {
            Span = span;
            GroundMapName = nomGroundMap;
            TextureNameTerrain = textureNameTerrain;
        }

        public override void Initialize()
        {
            TextureMgr = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            InitializeMapData();
            Heights = new float[GroundMap.Width, GroundMap.Height];
            InitializeTextureData();
            Origin = new Vector3(/*-Span.X / HALF_DIVISOR, 0, -Span.Z / HALF_DIVISOR*/0, 0, 0); //to center the primitive to point (0,0,0)##########################Moins à Z ajouté
            CreatePointArray(); // ############### INVERSÉ
            AllocateArrays(); // ################## INVERSÉ
            base.Initialize();
        }

        //
        // à partir de la texture servant de carte de height (HeightMap), on initialise les données
        // relatives à la structure de la carte
        //
        void InitializeMapData()
        {
            // FireBall
            GroundMap = TextureMgr.Find(GroundMapName);
            NumRows = GroundMap.Width - SUPPLEMENTARY_VERTEX_FOR_LINE;
            NumColumns = GroundMap.Height - SUPPLEMENTARY_VERTEX_FOR_LINE;
            NumTriangles = NumRows * NumColumns * NUM_TRIANGLES_PAR_TUILE;
            NumVertices = NumTriangles * NUM_VERTICES_PER_TRIANGLE;
            NumTexels = GroundMap.Width * GroundMap.Height;
            DataTexture = new Color[NumTexels];
            GroundMap.GetData<Color>(DataTexture);
        }

        //
        // à partir de la texture contenant les textures carte de height (HeightMap), on initialise les données
        // relatives à l'application des textures de la carte
        //
        void InitializeTextureData()
        {
            // FireBall
            TextureTerrain = TextureMgr.Find(TextureNameTerrain);
            TileWidth = (int)(TextureTerrain.Height / (float)NumTextureLevels);
        }

        //
        // Allocation des deux tableaux
        //    1) celui contenant les points de sommet (les points uniques), 
        //    2) celui contenant les sommets servant à dessiner les triangles
        void AllocateArrays()
        {
            // FireBall
            Vertices = new VertexPositionTexture[NumVertices];
            //TexturePts = new Vector2[GroundMap.Width, GroundMap.Height];
            TexturePts = new Vector2[NUM_TEXTURE_PTS_POSSIBLE];
            //VerticesPts = new Vector3[GroundMap.Width, GroundMap.Height];
            //Delta = new Vector2(Span.X / NumRows, Span.Z / NumColumns);
            AffecterPointsTexture();
            InitializeVertices();
        }

        void AffecterPointsTexture()
        {
            //for (int i = 0; i < TexturePts.GetLength(0); ++i)
            //{
            //    for (int j = 0; j < TexturePts.GetLength(1); ++j)
            //    {
            //        TexturePts[i, j] = new Vector2(0, VerticesPts[i, j].Y / Span.Y);
            //    }
            //}
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    TexturePts[NUM_VERTICES_PER_TILE * i + j] = new Vector2(j % NUM_TRIANGLES_PAR_TUILE, (i + (j > J_MAX_VALUE_TOP_TILE ? BOTTOM_TILE_VALUE : TOP_TILE_VALUE) * (1 - 1 / (float)TextureTerrain.Height)) / NumTextureLevels);
                    //TexturePts[NUM_VERTICES_PER_TILE * i + j] = new Vector2(0.5f, 0.9f);
                }
            }
            Game.Window.Title = TexturePts[11].ToString();
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
            BscEffect.Texture = TextureTerrain;
        }

        //
        // Création du tableau des points de sommets (on crée les points)
        // Ce processus implique la transformation des points 2D de la texture en coordonnées 3D du terrain
        //
        private void CreatePointArray()
        {
            // FireBall
            VerticesPts = new Vector3[GroundMap.Width, GroundMap.Height];
            Delta = new Vector2(Span.X / NumRows, Span.Z / NumColumns);
            for (int i = 0; i < VerticesPts.GetLength(0); ++i)
            {
                for (int j = 0; j < VerticesPts.GetLength(1); ++j)
                {
                    VerticesPts[i, j] = Origin + new Vector3(Delta.X * i, DataTexture[i * VerticesPts.GetLength(1) + j].B / MAX_COLOR * Span.Y, Delta.Y * j);
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

                    ////AssociéTruc(ref Vertices[cpt - 2], ref Vertices[cpt - 1], ref Vertices[cpt]);

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
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j], TexturePts[noCase]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j], TexturePts[noCase + 1]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[noCase + 2]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j], TexturePts[noCase + 1]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i + 1, j + 1], TexturePts[noCase + 3]);
            Vertices[++cpt] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[noCase + 2]);
        }

        //
        // Deviner ce que fait cette méthode...
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
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, NULL_COMPENSATION, NumTriangles);
            }
        }

        public float GetHeight(Vector3 position)
        {
            //Game.Window.Title = position.ToString();
            int i = (int)(position.X / Delta.X), j = (int)(position.Z / Delta.Y);
            Vector3 n;
            float height;
            if (i >= 0 && j >= 0 && i < Heights.GetLength(0) && j < Heights.GetLength(1))
            {
                n = Vector3.Cross(VerticesPts[i + 1, j] - VerticesPts[i, j], VerticesPts[i, j + 1] - VerticesPts[i, j]);
                height = (n.X * VerticesPts[i, j].X + n.Y * VerticesPts[i, j].Y + n.Z * VerticesPts[i, j].Z - n.X * position.X - n.Z * position.Z) / n.Y + 7;
                //height = Heights[i, j];
            }
            else
            {
                height = position.Y;
            }
            return height;
        }
    }
}

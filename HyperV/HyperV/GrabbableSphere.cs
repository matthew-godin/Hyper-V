using XNAProject;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HyperV
{
    class GrabbableSphere : BasicAnimatedPrimitive//, ICollidable
   {
        //Initially managed by the constructor
        readonly float Radius;
        readonly int NumColumns;
        readonly int NumLines;
        readonly string TextureName;

        readonly Vector3 Origin;
        Camera1 PlayerCamera { get; set; }

        //Initially managed by functions called by base.Initialize()
        Vector3[,] VerticesPts { get; set; }
        Vector2[,] TexturePts { get; set; }
        VertexPositionTexture[] Vertices { get; set; }
        BasicEffect BscEffect { get; set; }

        int NumTrianglesPerStrip { get; set; }

        //Initialement gérées par LoadContent()
        RessourcesManager<Texture2D> TextureMgr { get; set; }
        Texture2D SphereTexture { get; set; }

        public float? IsColliding(Ray otherObject)
        {
            return CollisionSphere.Intersects(otherObject);
        }

        public BoundingSphere CollisionSphere { get { return new BoundingSphere(Position, Radius); } }

        public bool IsGrabbed { get; set; }

        public GrabbableSphere(Game game, float initialScale, Vector3 initialRotation,
                              Vector3 initialPosition, float radius, Vector2 dimensions,
                              string textureName, float updateInterval)
            :base(game, initialScale, initialRotation, initialPosition, updateInterval)
        {
            Radius = radius;
            NumColumns = (int)dimensions.X;
            NumLines = (int)dimensions.Y;
            TextureName = textureName;

            Origin = new Vector3(0,0,0);

            Position = Origin;//*******************

            IsGrabbed = false;
        }

        public override void Initialize()
        {
            NumTrianglesPerStrip = NumColumns * 2;
            NumVertices = (NumTrianglesPerStrip + 2) * NumLines;

            AllocateArrays();
            base.Initialize();
            InitializeBscEffectParameters();
            PlayerCamera = GameCamera as Camera1;
        }

        protected override void PerformUpdate()
        {
            base.PerformUpdate();

            //if (IsGrabbed)
            //{
            //    Position = GameCamera.Position + 4 * Vector3.Normalize(PlayerCamera.Direction)
            //                + 2.5f * Vector3.Normalize(PlayerCamera.Lateral)
            //                - 1.5f * Vector3.Normalize(Vector3.Cross(PlayerCamera.Lateral, PlayerCamera.Direction));
            //    InitializeVertices();

            //    Game.Window.Title = Position.ToString();
            //}
        }

        void AllocateArrays()
        {
            VerticesPts = new Vector3[NumColumns + 1, NumLines + 1];
            TexturePts = new Vector2[NumColumns + 1, NumLines + 1];
            Vertices = new VertexPositionTexture[NumVertices];
        }

        void InitializeBscEffectParameters()
        {
            BscEffect = new BasicEffect(GraphicsDevice);
            BscEffect.TextureEnabled = true;
            BscEffect.Texture = SphereTexture;
        }

        protected override void InitializeVertices()
        {
            PopulateVerticesPts();
            PopulateTexturePts();
            PopulateVertices();
        }

        void PopulateVerticesPts()
        {
            float angle = (float)(2 * Math.PI) / NumColumns;
            float phi = 0;
            float theta = 0;

            for (int j = 0; j < VerticesPts.GetLength(0); ++j)
            {
                for (int i = 0; i < VerticesPts.GetLength(1); ++i)
                {
                    VerticesPts[i, j] = new Vector3(Position.X + Radius * (float)(Math.Sin(phi)*Math.Cos(theta)),
                                                   Position.Y + Radius * (float)(Math.Cos(phi)),
                                                   Position.Z + Radius * (float)(Math.Sin(phi) * Math.Sin(theta)));
                    theta += angle;
                }
                phi += (float)Math.PI / NumLines;
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

        void PopulateVertices()
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
            SphereTexture = TextureMgr.Find(TextureName);
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
            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices, vertexOffset, NumTrianglesPerStrip);
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAProject;

namespace HyperV
{
   /// <summary>
   /// This is a game component that implements IUpdateable.
   /// </summary>
   public class Water : BasicPrimitive
    {
        public Vector3 Position { get; private set; }
        float UpdateInterval { get; set; }
        protected InputManager InputMgr { get; private set; }
        float TimeElapsedSinceUpdate { get; set; }

        const int NUM_TRIANGLES = 2;
        protected Vector3[,] VerticesPts { get; private set; }
        Vector3 Origin { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect BscEffect { get; private set; }

        VertexPositionColor[] Vertices { get; set; }
        //VertexPositionTexture[] Vertices { get; set; }
        BlendState AlphaMgr { get; set; }

        //Vector2[,] TexturePts { get; set; }
        Displayer3D Display3D { get; set; }
        Color Color { get; set; }
        public float AdjustedHeight { get; private set; }

        public Vector3 GetPositionWithHeight(Vector3 position, int height)
        {
            Vector3 positionWithHeight;
            if (IsWithin(position.Z, VerticesPts[0, 0].Z, VerticesPts[VerticesPts.GetLength(0) - 1, VerticesPts.GetLength(1) - 1].Z) &&
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

        public Water(Game game, float initialScale, Vector3 initialRotation,
                     Vector3 initialPosition, Vector2 span,
                     float updateInterval)
            : base(game, initialScale, initialRotation, initialPosition)
        {
            UpdateInterval = updateInterval;
            Delta = new Vector2(span.X, span.Y);
            Origin = new Vector3(-Delta.X / 2, 0, -Delta.Y / 2); //to center the primitive to point (0,0,0)
            Color = new Color(20, 50, 250, 50);
        }

        public override void Initialize()
        {
            NumVertices = NUM_TRIANGLES + 2;
            VerticesPts = new Vector3[2, 2];
            CreatePointArray();
            CreateVertexArray();
            Position = InitialPosition;
            Display3D = Game.Services.GetService(typeof(Displayer3D)) as Displayer3D;
            AdjustedHeight = Position.Y + 7;
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
            //TexturePts = new Vector2[2, 2];
            Vertices = new VertexPositionColor[NumVertices];
            //Vertices = new VertexPositionTexture[NumVertices];
            //CreatePointArrayTexture();
        }

        //private void CreatePointArrayTexture()
        //{
        //    TexturePts[0, 0] = new Vector2(0, 1);
        //    TexturePts[1, 0] = new Vector2(1, 1);
        //    TexturePts[0, 1] = new Vector2(0, 0);
        //    TexturePts[1, 1] = new Vector2(1, 0);
        //}

        protected override void LoadContent()
        {
            BscEffect = new BasicEffect(GraphicsDevice);
            InitialiserParamètresBscEffect();
            base.LoadContent();
        }

        protected void InitialiserParamètresBscEffect()
        {
            BscEffect.VertexColorEnabled = true;
            //BscEffect.TextureEnabled = true;
            //BscEffect.Texture = TileTexture;
            //AlphaMgr = BlendState.AlphaBlend; // Be careful of this...
        }

        protected override void InitializeVertices() // Is called by base.Initialize()
        {
            int VertexIndex = -1;
            for (int j = 0; j < 1; ++j)
            {
                for (int i = 0; i < 2; ++i)
                {
                    Vertices[++VertexIndex] = new VertexPositionColor(VerticesPts[i, j], Color);
                    Vertices[++VertexIndex] = new VertexPositionColor(VerticesPts[i, j + 1], Color);
                    //Vertices[++VertexIndex] = new VertexPositionTexture(VerticesPts[i, j], TexturePts[i, j]);
                    //Vertices[++VertexIndex] = new VertexPositionTexture(VerticesPts[i, j + 1], TexturePts[i, j + 1]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //BlendState oldBlendState = GraphicsDevice.BlendState; // ... and this!
            //GraphicsDevice.BlendState = AlphaMgr;
            //BscEffect.World = GetWorld();
            //BscEffect.View = GameCamera.View;
            //BscEffect.Projection = GameCamera.Projection;
            //foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            //{
            //    passEffect.Apply();
            //    DrawTriangleStrip();
            //}
            //GraphicsDevice.BlendState = oldBlendState;
            RasterizerState s;// = Display3D.GameRasterizerState;
            s = new RasterizerState();
            s.CullMode = CullMode.None;
            s.FillMode = FillMode.Solid;
            Game.GraphicsDevice.RasterizerState = s;

            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.View;
            BscEffect.Projection = GameCamera.Projection;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                DrawTriangleStrip();
            }

            Game.GraphicsDevice.RasterizerState = Display3D.GameRasterizerState;
        }

        protected void DrawTriangleStrip()
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Vertices, 0, NUM_TRIANGLES);
            //GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices, 0, NUM_TRIANGLES);
        }

        public Vector3 GetPositionWithHeight(Vector3 position, int height)
        {
            Vector3 positionWithHeight;
            if (IsWithin(position.Z, VerticesPts[0, 0].Z, VerticesPts[VerticesPts.GetLength(0) - 1, VerticesPts.GetLength(1) - 1].Z) &&
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
    }
}

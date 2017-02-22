using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAProject;


namespace HyperV
{
    public class ModelCreator : Microsoft.Xna.Framework.DrawableGameComponent
    {
        RessourcesManager<Model> ModelManager { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }

        string Model3DName { get; set; }
        Model Model3D { get; set; } //the 3d model we want to place

        string TextureName2D { get; set; } //the texture that goes with the model
        Texture2D Texture2D { get; set; }

        Vector3 Position { get; set; } //the position of the model in the world
        float RotationModele { get; set; }
        PlayerCamera Camera { get; set; }
        float AspectRatio { get; set; }
        float Grosseur { get; set; }

        public ModelCreator(Game game) : base(game) { }

        public ModelCreator(Game game, string model3D, Vector3 position)
            : base(game)
        {
            Model3DName = model3D;
            Position = position;
        }

        protected override void LoadContent()
        {
            Camera = Game.Services.GetService(typeof(PlayerCamera)) as PlayerCamera;
            ModelManager = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            
            Model3D = ModelManager.Find(Model3DName);
        }

        public override void Initialize()
        {
            base.Initialize();
            RotationModele = 0.0f;
            AspectRatio = 1.0f;
            Grosseur = 1f;
        }
        
        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[Model3D.Bones.Count];
            Model3D.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in Model3D.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(new Vector3(0.05f, 0.05f, 0.05f)) * Matrix.CreateRotationY(RotationModele) * Matrix.CreateTranslation(Position);
                    effect.View = Camera.View;
                    effect.Projection = Camera.Projection;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}

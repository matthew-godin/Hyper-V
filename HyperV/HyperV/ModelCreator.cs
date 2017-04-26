using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAProject;
using System.Collections.Generic;
using System;

namespace HyperV
{
    public class ModelCreator : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public bool ButtonDisplacement = false;
        RessourcesManager<Model> ModelManager { get; set; }
        RessourcesManager<Texture2D> TextureManager { get; set; }

        List<BoundingSphere> CollisionSphere { get; set; }
        public List<BoundingSphere> GetCollisionSphere()
        {
            return CollisionSphere;
        }

        string Model3DName { get; set; }
        protected Model Model3D { get; set; } //the 3d model we want to place

        string TextureName2D { get; set; } //the texture that goes with the model
        Texture2D Texture2D { get; set; }
        public bool IsTower = false;

        protected Vector3 Position { get; set; } //the position of the model in the world
        public Vector3 GetPosition()
        {
            return Position;
        }
        public void DisplaceModel(Vector3 displacement)
        {
            Position += displacement;
        }

        protected float Rotation { get; set; }
        Camera Camera { get; set; }
        float Scale { get; set; }

        public ModelCreator(Game game) : base(game) { }

        public ModelCreator(Game game, string model3D, Vector3 position, float scale, float rotation)
            : base(game)
        {
            Model3DName = model3D;
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        public ModelCreator(Game game, string model3D, Vector3 position, float scale, float rotation, string textureName2D)
            : base(game)
        {
            Model3DName = model3D;
            Position = position;
            Scale = scale;
            Rotation = rotation;
            TextureName2D = textureName2D;
        }

        public override void Initialize()
        {
            base.Initialize();
            CollisionSphere = new List<BoundingSphere>();
        }

        protected override void LoadContent()
        {
            Camera = Game.Services.GetService(typeof(Camera)) as Camera;
            ModelManager = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;

            Model3D = ModelManager.Find(Model3DName);
            if (TextureName2D != null)
            {
                Texture2D = TextureManager.Find(TextureName2D);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[Model3D.Bones.Count];
            Model3D.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in Model3D.Meshes)
            {
                CollisionSphere.Add(mesh.BoundingSphere);
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (Texture2D != null)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = Texture2D;
                    }
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(Scale) * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(Position);
                    effect.View = Camera.View;
                    effect.Projection = Camera.Projection;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}

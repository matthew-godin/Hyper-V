﻿using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HyperV
{
    public class Skybox : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float GROSSEUR = 500f;

        public Model SkyBox;
        string SkyboxTexture { get; set; }
        Camera2 Camera { get; set; }

        RessourcesManager<Model> ModelManager { get; set; }
        RessourcesManager<TextureCube> TextureManager { get; set; }
        RessourcesManager<Effect> EffectManager { get; set; }

        private TextureCube SkyBoxTexture;
        private Effect SkyBoxEffect;
        SpriteBatch Graphics { get; set; }

        public Skybox(Game game, string skyboxTexture) : base(game)
        {
            SkyboxTexture = skyboxTexture;
        }

        protected override void LoadContent()
        {
            ModelManager = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            TextureManager = Game.Services.GetService(typeof(RessourcesManager<TextureCube>)) as RessourcesManager<TextureCube>;
            EffectManager = Game.Services.GetService(typeof(RessourcesManager<Effect>)) as RessourcesManager<Effect>;
            Camera = Game.Services.GetService(typeof(Camera)) as Camera2;
            Graphics = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            SkyBox = ModelManager.Find("Cube");
            SkyBoxTexture = TextureManager.Find(SkyboxTexture);
            SkyBoxEffect = EffectManager.Find("Skybox");
        }

        public override void Draw(GameTime gametime)
        {
            foreach (ModelMesh mesh in SkyBox.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = SkyBoxEffect;
                    part.Effect.Parameters["World"].SetValue(Matrix.CreateScale(GROSSEUR) * Matrix.CreateTranslation(Camera.Position));
                    part.Effect.Parameters["View"].SetValue(Camera.View);
                    part.Effect.Parameters["Projection"].SetValue(Camera.Projection);
                    part.Effect.Parameters["SkyBoxTexture"].SetValue(SkyBoxTexture);
                    part.Effect.Parameters["CameraPosition"].SetValue(Camera.Position);
                }
                mesh.Draw();
            }
        }
    }
}
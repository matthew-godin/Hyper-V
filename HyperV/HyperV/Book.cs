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
    public class Book : ModelCreator
    {
        float MINIMAL_DISTANCE = 10;
        string BookImage { get; set; }

        InputManager InputMgrs { get; set; }
        Camera2 Camera { get; set; }
        Sprite Text { get; set; }


        public Book(Game game, string model3D, Vector3 position, float scale, float rotation, string nameModel2D, string bookImage)
            : base(game, model3D, position, scale, rotation, nameModel2D)
        {
            BookImage = bookImage;
        }

        public override void Initialize()
        {           
            base.Initialize();
        }

        float? FindDistance(Ray otherObject, BoundingSphere CollisionSphere)
        {
            return CollisionSphere.Intersects(otherObject);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            InputMgrs = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Camera = Game.Services.GetService(typeof(Camera)) as Camera2;
        }

        public override void Update(GameTime gameTime)        
        {
            if (InputMgrs.IsNewLeftClick())
            {                
                if (IsWithinRightDistance(this))
                {
                    if (Game.Components.Contains(Text))
                    {
                        Game.Components.Remove(Text);
                    }
                    Text = new Sprite(Game, BookImage, new Vector2(GraphicsDevice.DisplayMode.Width / 2 - 450, GraphicsDevice.DisplayMode.Height / 2 - 350));
                    Game.Components.Add(Text);
                }
            }
            if (InputMgrs.IsOldRightClick())
            {
                Game.Components.Remove(Text);
            }
        }

        bool IsWithinRightDistance(ModelCreator model)
        {
            float? minDistance = float.MaxValue;
            BoundingSphere sphere = new BoundingSphere(model.GetPosition(), 3f);
            float? distance = FindDistance(new Ray(Camera.Position, Camera.Direction), sphere);
            if (minDistance > distance)
            {
                minDistance = distance;
            }
            return minDistance < MINIMAL_DISTANCE;
        }
    }
}

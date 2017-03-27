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
    //MUST UPDATE DLL TO WORK
    public class GrabbableModel : BaseObject
    {
        public static bool Taken { get; set; }

        static GrabbableModel()
        {
            Taken = false;
        }

        public bool Grab { get; set; }

        public bool IsGrabbed { get; set; }
        public bool Placed { get; set; }

        private float Radius { get; set; }

        PlayerCamera PlayerCamera { get; set; }

        //Matrix InitialWorld { get; set; }

        public BoundingSphere CollisionSphere
        {
            get { return new BoundingSphere(Position, Radius); }
        }

        public float? IsColliding(Ray otherObject)
        {
            return CollisionSphere.Intersects(otherObject);
        }

        public GrabbableModel(Game game, string modelName, float initialScale,
                                Vector3 initialRotation, Vector3 initialPosition)
            : base(game, modelName, initialScale, initialRotation, initialPosition)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            IsGrabbed = false;
            Placed = false;
            Radius = 4;
            //InitialWorld = base.GetWorld();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            PlayerCamera = Game.Services.GetService(typeof(Camera)) as PlayerCamera;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsGrabbed)
            {
                Position = PlayerCamera.Position + 4 * Vector3.Normalize(PlayerCamera.Direction)
                            + 2.5f * Vector3.Normalize(PlayerCamera.Lateral)
                            - 1.5f * Vector3.Normalize(Vector3.Cross(PlayerCamera.Lateral, PlayerCamera.Direction));
                ComputeWorld();
                //Game.Window.Title = Position.ToString();
            }
            base.Update(gameTime);
        }

        private void ComputeWorld()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);
        }


    }
}

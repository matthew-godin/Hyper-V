//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using XNAProject;


//namespace HyperV
//{
//    //MUST UPDATE DLL TO WORK
//    public class GrabbableModel : BaseObject
//    {
//        public static bool Taken { get; set; }

//        static GrabbableModel()
//        {
//            Taken = false;
//        }

//        public bool Grab { get; set; }

//        public bool IsGrabbed { get; set; }
//        public bool Placed { get; set; }

//        private float Radius { get; set; }

//        PlayerCamera PlayerCamera { get; set; }

//        //Matrix InitialWorld { get; set; }

//        public BoundingSphere CollisionSphere
//        {
//            get { return new BoundingSphere(Position, Radius); }
//        }

//        public float? IsColliding(Ray otherObject)
//        {
//            return CollisionSphere.Intersects(otherObject);
//        }

//        public GrabbableModel(Game game, string modelName, float initialScale,
//                                Vector3 initialRotation, Vector3 initialPosition)
//            : base(game, modelName, initialScale, initialRotation, initialPosition)
//        {

//        }

//        public override void Initialize()
//        {
//            base.Initialize();
//            IsGrabbed = false;
//            Placed = false;
//            Radius = 4;
//            //InitialWorld = base.GetWorld();
//        }

//        protected override void LoadContent()
//        {
//            base.LoadContent();
//            PlayerCamera = Game.Services.GetService(typeof(Camera)) as PlayerCamera;
//        }

//        public override void Update(GameTime gameTime)
//        {
//            if (IsGrabbed)
//            {
//                Position = PlayerCamera.Position + 4 * Vector3.Normalize(PlayerCamera.Direction)
//                            + 2.5f * Vector3.Normalize(PlayerCamera.Lateral)
//                            - 1.5f * Vector3.Normalize(Vector3.Cross(PlayerCamera.Lateral, PlayerCamera.Direction));
//                ComputeWorld();
//                //Game.Window.Title = Position.ToString();
//            }
//            base.Update(gameTime);
//        }

//        private void ComputeWorld()
//        {
//            Monde = Matrix.Identity;
//            Monde *= Matrix.CreateScale(Échelle);
//            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
//            Monde *= Matrix.CreateTranslation(Position);
//        }


//    }
//}





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
    public class GrabbableModel : BaseObject
    {
        public static bool Taken { get; set; }

        static GrabbableModel()
        {
            Taken = false;
        }
        public bool Placed { get; set; }
        public bool Grab { get; set; }
        //UP WAS COMMENTED

        public bool IsGrabbed { get; set; }

        private float Radius { get; set; }

        protected PlayerCamera PlayerCamera { get; set; }

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
            Radius = 10;
            //Placed = false;
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
                ComputeAngles();
                ComputeWorld();
            }
            base.Update(gameTime);
        }

        protected float angleX { set; get; }

        protected float angleY { set; get; }

        protected virtual void ComputeAngles()
        {
            Vector3 DirectionXYZ = Vector3.Normalize(new Vector3(PlayerCamera.Direction.X, PlayerCamera.Direction.Y, PlayerCamera.Direction.Z));

            angleX = -(float)Math.PI / 2f +//********
                (float)Math.PI * 3 / 2f +
                (DirectionXYZ.Z >= 0 ? -1 : 1) *
                (float)Math.Acos(Vector2.Dot((new Vector2(DirectionXYZ.X, DirectionXYZ.Z)),
                                              new Vector2(1, 0)));
            angleY =
                MathHelper.Pi / 2f -
                (float)Math.Acos(Vector2.Dot(new Vector2(DirectionXYZ.X, DirectionXYZ.Y),
                                             new Vector2(0, 1)));
        }

        private void ComputeWorld()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromYawPitchRoll(angleX, angleY, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);

            Game.Window.Title = PlayerCamera.Direction.ToString() + "      " + MathHelper.ToDegrees(angleX).ToString() + "       " + MathHelper.ToDegrees(angleY).ToString().ToString();
        }
    }
}

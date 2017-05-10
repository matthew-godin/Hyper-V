using Microsoft.Xna.Framework;
using XNAProject;


namespace HyperV
{
    public class RythmSphere : TexturedSphere, ICollidable
    {
        public bool ToDestroy { get; set; }  // only semicolon

        #region
        float Radius { get; set; }

        public new bool IsColliding(object otherObject)
        {
            BoundingSphere obj2 = (otherObject as ICollidable).CollisionSphere;
            return obj2.Intersects(CollisionSphere);
        }

        public new BoundingSphere CollisionSphere
        {
            get
            {
                return new BoundingSphere(Position, Radius);
            }
        }
        #endregion

        public Vector3 Extremity1 { get; set; }  // only semicolon
        Vector3 Extremity2 { get; set; }
        Vector3 DisplacementVector { get; set; }
        int i { get; set; }
        LifeBar[] LifeBars { get; set; }

        RythmLevel Level { get; set; }

        public RythmSphere(Game game, float initialScale, Vector3 initialRotation,
                       Vector3 initialPosition, float radius, Vector2 dimensions,
                       string textureName, float updateInterval, Vector3 extremity2)
            : base(game, initialScale, initialRotation,
                   initialPosition, radius, dimensions,
                   textureName, updateInterval)
        {
            Radius = radius;//****

            Extremity1 = initialPosition;
            Extremity2 = extremity2;
        }

        public override void Initialize()
        {
            base.Initialize();
            ToDestroy = false;
            i = 0;
            DisplacementVector = Vector3.Normalize(Extremity2 - Extremity1);
            Level = Game.Services.GetService(typeof(RythmLevel)) as RythmLevel;
            LifeBars = Game.Services.GetService(typeof(LifeBar[])) as LifeBar[];

        }

        protected override void PerformUpdate()
        {
            base.PerformUpdate();

            Position += DisplacementVector;
            ComputeWorldMatrix();
            i++;

            if(i > (Extremity1 - Extremity2).Length() + 3 || ToDestroy)
            {
                if (!ToDestroy)
                {
                    Level.RedCubePosition = Extremity2;
                    LifeBars[0].Attack(10);
                }
                Game.Components.Remove(this);

                //take lives
            }

        }


    }
}

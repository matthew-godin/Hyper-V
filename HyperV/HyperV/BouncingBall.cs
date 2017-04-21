using System;
using GameProjectXNA;
using Microsoft.Xna.Framework;

namespace HyperV
{
    class BouncingBall : TexturedSphere
    {
        const int DISPLACEMENT_MINIMAL_STARTING_ANGLE = 15,      //Angular constants
                  DISPLACEMENT_MAXIMAL_STARTING_ANGLE = 75,
                  RIGHT_ANGLE = 90,
                  MINIMAL_FACTOR_360_DEGREES_CIRCLE = 0,
                  MAXIMAL_FACTOR_360_DEGREES_CIRCLE_EXCLUDED = 4,
                  FLAT_ANGLE = 180, FULL_ANGLE = 360,
                  UNIT_X = 1,
                  UNIT_Y = 1;

        const int COLLISION_DISTANCE = 9,                     //Other constants
                  NO_TIME_ELAPSED = 0,
                  VISOR_LENGTH = 25,
                  ATTACK_VALUE = 10,
                  SPEED_MIN_X_TEN = 6,
                  SPEED_MAX_X_TEN = 10;

        const float RADIUS_SCALE = 1.4f,
                    SPEED_FACTOR_INTERVAL = 0.6f;

        float TimeElapsedSinceUpdateDisplacement { get; set; }
        float UpdateIntervalDéplacement { get; set; }
        Vector3 UpdateDisplacementVector { get; set; }
        Random RandomNumberGenerator { get; set; }
        float ThetaDisplacementAngle { get; set; }
        float PhiDisplacementAngle { get; set; }

        Sword Sword { get; set; }
        float Radius { get; set; }
        PlayerCamera CameraPrison { get; set; }
        float Speed { get; set; }

        int[] Margins { get; set; }

        public BoundingSphere BallCollisonSphere
        {
            get { return new BoundingSphere(Position, Radius * RADIUS_SCALE); }
        }

        public BouncingBall(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition,
                                 float rayon, Vector2 charpente, string nomTexture, float updateInterval)
           : base(game, initialScale, initialRotation, initialPosition, rayon, charpente, nomTexture, updateInterval)
        {
            UpdateIntervalDéplacement = updateInterval * SPEED_FACTOR_INTERVAL;
            Position = initialPosition;
            Radius = rayon;
        }

        public override void Initialize()
        {
            base.Initialize();
            Margins = new int[] { -200, 80, -40, 0, -50, 230 };    // MarginsX(2),MarginsY(2),MarginsZ(2)
            ComputeDisplacementVectort();
            TimeElapsedSinceUpdateDisplacement = NO_TIME_ELAPSED;

        }
        protected override void LoadContent()
        {
            base.LoadContent();
            RandomNumberGenerator = Game.Services.GetService(typeof(Random)) as Random;
            CameraPrison = Game.Services.GetService(typeof(Camera)) as PlayerCamera;
            Sword = Game.Services.GetService(typeof(Sword)) as Sword;

            ThetaDisplacementAngle = ComputeRandomDisplacementAngle();
            PhiDisplacementAngle = ComputeRandomDisplacementAngle();
            Speed = (RandomNumberGenerator.Next(SPEED_MIN_X_TEN, SPEED_MAX_X_TEN)) / 10f;
        }

        public override void Update(GameTime gameTime)
        {
            TimeElapsedSinceUpdateDisplacement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeElapsedSinceUpdateDisplacement >= UpdateIntervalDéplacement)
            {
                ComputeWorldMatrix();
                Position += UpdateDisplacementVector;
                ManageBallCollisions();
                TimeElapsedSinceUpdateDisplacement = NO_TIME_ELAPSED;
                CameraPrison.Ramasser = true;

            }
            base.Update(gameTime);
        }

        void ComputeDisplacementVectort()
        {
            float x = Speed * (float)Math.Cos(MathHelper.ToRadians(ThetaDisplacementAngle) * (float)Math.Sin(MathHelper.ToRadians(PhiDisplacementAngle)));
            float y = Speed * (float)Math.Sin(MathHelper.ToRadians(ThetaDisplacementAngle) * (float)Math.Sin(MathHelper.ToRadians(PhiDisplacementAngle)));
            float z = Speed * (float)Math.Cos(MathHelper.ToRadians(PhiDisplacementAngle));
            UpdateDisplacementVector = new Vector3(x, y, z);
        }

        void Borders(int minThreshold, int maxThreshold, float currentPosition, string indicator)
        {
            minThreshold += (int)Math.Ceiling(Radius);
            maxThreshold -= (int)Math.Ceiling(Radius);

            if (currentPosition <= minThreshold || currentPosition >= maxThreshold)
            {
                if (indicator == "x")
                {
                    ThetaDisplacementAngle = FLAT_ANGLE + ThetaDisplacementAngle;
                    PhiDisplacementAngle = FLAT_ANGLE + PhiDisplacementAngle;
                }
                else if (indicator == "z")
                {
                    PhiDisplacementAngle = FLAT_ANGLE + PhiDisplacementAngle;

                }
                else if (indicator == "y")
                {
                    ThetaDisplacementAngle = -ThetaDisplacementAngle;

                }
                ComputeDisplacementVectort();
            }
        }

        void ManageBallCollisions()
        {
            if (IsCollidingBall(CameraPrison.Viseur) < VISOR_LENGTH && Sword.ContinuerCoupDSword)
            {
                Game.Components.Remove(this);
            }
            if (BallCollisionCamera(COLLISION_DISTANCE))
            {
                CameraPrison.Attack(ATTACK_VALUE);
            }

            Borders(Margins[0], Margins[1], Position.X, "x");
            Borders(Margins[2], Margins[3], Position.Y, "y");
            Borders(Margins[4], Margins[5], Position.Z, "z");
        }
        public float? IsCollidingBall(Ray autreObjet)
        {
            return BallCollisonSphere.Intersects(autreObjet);
        }

        bool BallCollisionCamera(float distance)
        {
            return Vector3.Distance(CameraPrison.Position, Position) < distance;
        }

        float ComputeRandomDisplacementAngle()
        {
            return RandomNumberGenerator.Next(MINIMAL_FACTOR_360_DEGREES_CIRCLE, MAXIMAL_FACTOR_360_DEGREES_CIRCLE_EXCLUDED) * RIGHT_ANGLE +
                                   RandomNumberGenerator.Next(DISPLACEMENT_MINIMAL_STARTING_ANGLE, DISPLACEMENT_MAXIMAL_STARTING_ANGLE);
        }
    }
}

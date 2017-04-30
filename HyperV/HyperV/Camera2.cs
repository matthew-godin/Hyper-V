using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace HyperV
{
    public class Camera2 : PlayerCamera
    {
        //Added from first Camera1
        //float Height { get; set; }

        List<Maze> Maze { get; set; }
        List<Character> Characters { get; set; }
        Boss Boss { get; set; }
        List<HeightMap> HeightMap { get; set; }
        List<Water> Water { get; set; }
        Grass Grass { get; set; }
        List<Walls> Walls { get; set; }
        List<Portal> Portals { get; set; }
        List<House> Houses { get; set; }
        List<UnlockableWall> Unlockables { get; set; }
        bool SubjectiveCamera { get; set; }

        public Camera2(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval, float renderDistance)
            : base(game, positionCamera, target, orientation, updateInterval, renderDistance)
        { }

        protected override void LoadContent()
        {
            base.LoadContent();
            SubjectiveCamera = false;
            Maze = Game.Services.GetService(typeof(List<Maze>)) as List<Maze>;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
            Boss = Game.Services.GetService(typeof(Boss)) as Boss;
            HeightMap = Game.Services.GetService(typeof(List<HeightMap>)) as List<HeightMap>;
            Grass = Game.Services.GetService(typeof(Grass)) as Grass;
            ManageHeight();
            Water = Game.Services.GetService(typeof(List<Water>)) as List<Water>;
            Walls = Game.Services.GetService(typeof(List<Walls>)) as List<Walls>;
            Houses = Game.Services.GetService(typeof(List<House>)) as List<House>;
            Portals = Game.Services.GetService(typeof(List<Portal>)) as List<Portal>;
            Unlockables = Game.Services.GetService(typeof(List<UnlockableWall>)) as List<UnlockableWall>;
        }

        //NO WATER
        protected override void ManageHeight()
        {
            //Height = HeightMap.GetHeight(Position);
            //NO WATER
            if (!SubjectiveCamera)
            {
                if (!LifeBars[1].Water)
                {
                
                    if (HeightMap.Count > 0)
                    {
                        float height = 5;
                        for (int i = 0; i < HeightMap.Count && height == 5; ++i)
                        {
                            height = HeightMap[i].GetHeight(Position);
                        }
                        Height = height;
                    }
                }
                base.ManageHeight();
            }
        }

        //protected override void ManageHeight()
        //{
        //    if (!SubjectiveCamera)
        //    {
        //        if (HeightMap.Count > 0)
        //        {
        //            float height = 5;
        //            for (int i = 0; i < HeightMap.Count && height == 5; ++i)
        //            {
        //                height = HeightMap[i].GetHeight(Position);
        //            }
        //            Height = height;
        //        }
        //        base.ManageHeight();
        //    }
        //}

        protected override void ManageLifeBars()
        {
            if (!SubjectiveCamera)
            {
                base.ManageLifeBars();
            }
        }

        protected override void ManageDisplacement(float direction, float lateral)
        {
            base.ManageDisplacement(direction, lateral);

            if (!SubjectiveCamera)
            {
                if ((Maze.Count > 0 ? CheckForMazeCollision() : false) || (Walls.Count > 0 ? CheckForWallsCollision() : false) || (Characters.Count > 0 ? CheckForCharacterCollision() : false) || (Portals.Count > 0 ? CheckForPortalCollision() : false) || (Unlockables.Count > 0 ? CheckForUnlockableWallCollision() : false) /*|| (Boss != null ? CheckForBossCollision() : false)*/ || (Houses.Count > 0 ? CheckForHouseCollision() : false))
                {
                    Position -= direction * TranslationSpeed * Direction;
                    Position += lateral * TranslationSpeed * Lateral;
                }
            }
            // NO WATER
            if (LifeBars[1].Water)
            {
                Position -= direction * TranslationSpeed * Direction;
                Position += lateral * TranslationSpeed * Lateral;
                Position += direction * TRANSLATION_INITIAL_SPEED * Direction;
                Position -= lateral * TRANSLATION_INITIAL_SPEED * Lateral;
            }
            for (int i = 0; i < Water.Count /*&& height == 5*/; ++i)
            {
                if (!LifeBars[1].Water && Position.Y <= Water[i].AdjustedHeight)
                {
                    LifeBars[1].TurnWaterOn();
                    break;
                }
                else if (LifeBars[1].Water && Position.Y > Water[i].AdjustedHeight)
                {
                    LifeBars[1].TurnWaterOff();
                    break;
                }
            }
            if (LifeBars[1].Drowned)
            {
                LifeBars[0].Attack(1);
            }
        }
        //NO WATER
        protected override void ManageJump()
        {
            if (LifeBars[1].Water)
            {
                if (Jump)
                {
                    Height += 0.4f;
                    for (int i = 0; i < Water.Count /*&& height == 5*/; ++i)
                    {
                        if (Height > Water[i].AdjustedHeight)
                        {
                            Height = Water[i].AdjustedHeight;
                            LifeBars[1].Restore();
                            break;
                        }
                    }
                    Position = new Vector3(Position.X, Height/*CHARACTER_HEIGHT*/, Position.Z);
                    //++Height;
                }
                else
                {
                    Height -= 0.4f;
                    for (int i = 0; i < HeightMap.Count /*&& height == 5*/; ++i)
                    {
                        if (Height < HeightMap[i].GetHeight(Position))
                        {
                            Height = HeightMap[i].GetHeight(Position);
                            break;
                        }
                    }
                    
                    Position = new Vector3(Position.X, Height/*CHARACTER_HEIGHT*/, Position.Z);
                    //--Height;
                }
            }
            else
            {
                base.ManageJump();
            }
        }

        bool CheckForWallsCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Walls.Count && !result; ++i)
            {
                result = Walls[i].CheckForCollisions(Position);
            }

            return result;
        }

        bool CheckForMazeCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Maze.Count && !result; ++i)
            {
                result = Maze[i].CheckForCollisions(Position);
            }

            return result;
        }

        const float MAX_DISTANCE = 5.5f, MAX_DISTANCE_BOSS = 80f;

        bool CheckForBossCollision()
        {
            return Vector3.Distance(Boss.GetPosition(), Position) < MAX_DISTANCE_BOSS;
        }

        bool CheckForPortalCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Portals.Count && !result; ++i)
            {
                result = Portals[i].CheckForCollisions(Position);
            }

            return result;
        }

        bool CheckForUnlockableWallCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Unlockables.Count && !result; ++i)
            {
                result = Unlockables[i].CheckForCollisions(Position);
            }

            return result;
        }

        bool CheckForCharacterCollision()
        {
            bool result = false;
            int i;

            for (i = 0; i < Characters.Count && !result; ++i)
            {
                result = Vector3.Distance(Characters[i].GetPosition(), Position) < MAX_DISTANCE;
            }

            return result;
        }

        bool CheckForHouseCollision()
        {
            bool result = false;
            float? d;
            int i;

            for (i = 0; i < Houses.Count && !result; ++i)
            {
                result = Houses[i].Collision(new BoundingSphere(Position, 7));
            }

            return result;
        }

        public void DeactivateCamera()
        {
            DésactiverDéplacement = !DésactiverDéplacement;
            Direction = new Vector3(1, 0, 0);
        }

        bool placePlayer { get; set; }

        //public override void Update(GameTime gameTime) // To make the catapult work
        //{
        //    float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    TimeElapsedSinceUpdate += elapsedTime;
        //    if (TimeElapsedSinceUpdate >= UpdateInterval)
        //    {
        //        {
        //            if (!DésactiverDéplacement)
        //            {
        //                if (placePlayer)
        //                {
        //                    Height = 2;
        //                    placePlayer = false;
        //                    Position = new Vector3(-27, 2, -28);
        //                }
        //            }
        //            if (DésactiverDéplacement)
        //            {
        //                Height = 15;
        //                Position = new Vector3(-57, 15, -52);
        //                placePlayer = true;
        //            }
        //            Position = new Vector3(Position.X, Height, Position.Z);
        //        }
        //        TimeElapsedSinceUpdate = 0;
        //    }
        //    base.Update(gameTime);
        //}


    }
}


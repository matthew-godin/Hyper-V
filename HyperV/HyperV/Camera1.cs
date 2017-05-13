using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace HyperV
{
   public class Camera1 : PlayerCamera
    {
        const float MAX_DISTANCE = 4.5f;

        Grass Grass { get; set; }
        Walls Walls { get; set; }
        List<Character> Characters { get; set; }
        List<Portal> Portals { get; set; }

        public Camera1(Game game, Vector3 positionCamera, Vector3 target, Vector3 orientation, float updateInterval, float renderDistance)
            : base(game, positionCamera, target, orientation, updateInterval, renderDistance)
        { }

        protected override void LoadContent()
        {
            base.LoadContent();
            Grass = Game.Services.GetService(typeof(Grass)) as Grass;
            Walls = Game.Services.GetService(typeof(Walls)) as Walls;
            Characters = Game.Services.GetService(typeof(List<Character>)) as List<Character>;
            Portals = Game.Services.GetService(typeof(List<Portal>)) as List<Portal>;
        }

        //protected override void ManageHeight()
        //{
        //    //if (HeightMap != null)
        //    //{
        //    //    Height = HeightMap.GetHeight(Position);
        //    //}
        //    //base.ManageHeight();
        //}

        protected override void ManageDisplacement(float direction, float lateral)
        {
            base.ManageDisplacement(direction, lateral);

            if (Walls.CheckForCollisions(Position) || CheckForCharacterCollision() || CheckForPortalCollision())
            {
                Position -= direction * TranslationSpeed * Direction;
                Position += lateral * TranslationSpeed * Lateral;
            }
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
    }
}

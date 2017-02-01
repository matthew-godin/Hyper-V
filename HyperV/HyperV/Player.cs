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


namespace HyperV
{    
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        InputManager GestionInput { get; set; }
        float Temps�coul�DepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }

        public Player(Game game, float intervalleMAJ)
            : base(game)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            float Temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += Temps�coul�;
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                if(GestionInput.EstEnfonc�e(Keys.W) || GestionInput.EstEnfonc�e(Keys.S) || GestionInput.EstEnfonc�e(Keys.A) || GestionInput.EstEnfonc�e(Keys.D))
                {
                    GererDeplacement();
                }
                Temps�coul�DepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        private int G�rerTouche(Keys touche)
        {
            return GestionInput.EstEnfonc�e(touche) ? 1 : 0;
        }

        private void GererDeplacement()
        {
            float deplacementDirection = (G�rerTouche(Keys.W) - G�rerTouche(Keys.S));
            float deplacementLat�ral = (G�rerTouche(Keys.A) - G�rerTouche(Keys.D));
            Cam�raSubjective Camera = Game.Services.GetService(typeof(Cam�raSubjective)) as Cam�raSubjective;
            Camera.DeplacerCamera(deplacementDirection, deplacementLat�ral, 0.5f, false);
        }
    }
}

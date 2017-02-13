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
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float VITESSE_INITIALE_TRANSLATION = 0.5f;

        InputManager GestionInput { get; set; }
        float Temps�coul�DepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }
        Cam�raSubjective Camera { get; set; }
        float VitesseTranslation { get; set; }
        Point AnciennePositionSouris { get; set; }
        Point DeplacementSouris { get; set; }

        public Player(Game game, float intervalleMAJ)
            : base(game)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            base.Initialize();
            VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
            AnciennePositionSouris = GestionInput.GetPositionSouris();
        }

        protected override void LoadContent()
        {
            Camera = Game.Services.GetService(typeof(Cam�raSubjective)) as Cam�raSubjective;
        }

        public override void Update(GameTime gameTime)
        {
            float Temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += Temps�coul�;
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                G�rerRotation();
                if (GestionInput.EstEnfonc�e(Keys.W) || GestionInput.EstEnfonc�e(Keys.S) || GestionInput.EstEnfonc�e(Keys.A) || GestionInput.EstEnfonc�e(Keys.D))
                {
                    GererDeplacement();
                    GererSprint();
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
            float deplacementDirection = (G�rerTouche(Keys.W) - G�rerTouche(Keys.S)) * VitesseTranslation;
            float deplacementLat�ral = (G�rerTouche(Keys.A) - G�rerTouche(Keys.D)) * VitesseTranslation;
            Camera.GererDeplacementCamera(deplacementDirection, deplacementLat�ral);
        }

        private void GererSprint()
        {
            if(GestionInput.EstEnfonc�e(Keys.LeftShift) || GestionInput.EstEnfonc�e(Keys.RightShift))
            {
                Camera.G�rerAcc�l�ration(0.5f);
            }
        }

        private void G�rerRotation()
        {
            DeplacementSouris = new Point((GestionInput.GetPositionSouris().X - AnciennePositionSouris.X), GestionInput.GetPositionSouris().Y - AnciennePositionSouris.Y);
            if(DeplacementSouris.X != 0)
            {
                Camera.GererLacet(DeplacementSouris.X);
            }
            if(DeplacementSouris.Y != 0)
            {
                Camera.GererTangage(DeplacementSouris.Y);
            }
            AnciennePositionSouris = GestionInput.GetPositionSouris();
        }
    }
}

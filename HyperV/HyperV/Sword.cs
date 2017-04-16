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
    public class Sword : GrabbableModel
    {
        bool SwordHit { get; set; }
        bool ContinueSwordHit { get; set; }
        float t { get; set; }
        InputManager InputMgr { get; set; }
        GamePadManager GamePadMgr { get; set; }
        float DiffAngleX { get; set; }
        float DiffAngleY { get; set; }

        public Sword(Game game, string modelName, float initialScale,
                    Vector3 initialRotation, Vector3 initialPosition)
            : base(game, modelName, initialScale, initialRotation, initialPosition)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            SwordHit = false;
            ContinueSwordHit = false;
            t = 0;
            DiffAngleX = 0;
            DiffAngleY = 0;
        }

        Boss Boss { get; set; }
        List<Enemy> Enemy { get; set; }

        protected override void LoadContent()
        {
            base.LoadContent();
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GamePadMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            Boss = Game.Services.GetService(typeof(Boss)) as Boss;
            Enemy = Game.Services.GetService(typeof(List<Enemy>)) as List<Enemy>;
        }


        protected override void ComputeAngles()
        {
            base.ComputeAngles();

            if (SwordHit)
            {
                ContinueSwordHit = true;
            }

            if (ContinueSwordHit)
            {
                if (t < 30)
                {
                    DiffAngleX -= 0.03f;
                    DiffAngleY += 0.05f;
                }
                else
                {
                    DiffAngleX += 0.03f;
                    DiffAngleY -= 0.05f;

                    if (t > 60)
                    {
                        ContinueSwordHit = false;
                        DiffAngleX = 0;
                        DiffAngleY = 0;
                        t = 0;
                        if (Boss != null)
                        {
                            Boss.CheckForAttack(10);
                        }
                        if (Enemy.Count > 0)
                        {
                            foreach (Enemy e in Enemy)
                            {
                                e.CheckForAttack(10);
                            }
                        }
                    }
                }
                angleX += DiffAngleX;
                angleY += DiffAngleY;
                ++t;
            }

        }

        public override void Update(GameTime gameTime)
        {
            SwordHit = (InputMgr.IsPressede(Keys.T) || InputMgr.EstNouveauClicRight() )&& IsGrabbed;

            base.Update(gameTime);
        }
    }
}

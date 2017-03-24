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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    class Mill : TexturedTile
    {
        const int NUM_GEAR_WHEELS = 2;

        GearWheel[] GearWheels { get; set; }
        float Timer { get; set; }
        float Interval { get; set; }
        Vector3[] Positions { get; set; }
        float Radius { get; set; }
        Camera2 Camera { get; set; }
        PressSpaceLabel PressSpaceLabel { get; set; }
        GrabbableModel[] Takables { get; set; }
        InputManager InputManager { get; set; }
        bool[] Placed { get; set; }

        public Mill(Game game, float scale, Vector3 initialRotation, Vector3 initialPosition, Vector2 range, string textureName, float interval) : base(game, scale, initialRotation, initialPosition, range, textureName, interval)
        {
            // TODO: Construct any child components here
            Interval = interval;
            Timer = 0;
            Radius = 15;
            PressSpaceLabel = new PressSpaceLabel(Game);
            GearWheels = new GearWheel[NUM_GEAR_WHEELS];
            Positions = new Vector3[NUM_GEAR_WHEELS];
            Placed = new bool[NUM_GEAR_WHEELS];
            Takables = new GrabbableModel[NUM_GEAR_WHEELS];
            for (int i = 0; i < NUM_GEAR_WHEELS; ++i)
            {
                Placed[i] = false;
            }
            Positions[0] = new Vector3(299, 9, 102);
            Positions[1] = new Vector3(305.4f, 15.4f, 102);
            GearWheels[0] = new GearWheel(Game, "gearwheel5", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), Positions[0]);
            Game.Components.Add(GearWheels[0]);
            GearWheels[1] = new GearWheel(Game, "gearwheel2", 0.025f, new Vector3(0, MathHelper.ToRadians(90), 0), Positions[1]);
            Game.Components.Add(GearWheels[1]);
            Takables[0] = new GrabbableModel(Game, "gearwheel5", 0.01f, new Vector3(0, 0, MathHelper.ToRadians(90)), new Vector3(370, 10, 100));
            Game.Components.Add(Takables[0]);
            Takables[1] = new GrabbableModel(Game, "gearwheel2", 0.025f, new Vector3(0, 0, MathHelper.ToRadians(90)), new Vector3(420, 10, 100));
            Game.Components.Add(Takables[1]);
            GearWheels[1].Visible = false;
            GearWheels[0].Visible = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //Game.Components.Add(new TexturedTile(Game, 1, Vector3.Zero, new Vector3(Positions[1].X, Positions[1].Y, Positions[1].Z - 5), new Vector2(5, 5), "point", 1 / 60f));
            base.Initialize();
            Camera = Game.Services.GetService(typeof(Camera)) as Camera2;
            InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Game.Components.Add(PressSpaceLabel);
            PressSpaceLabel.Visible = false;
        }

        bool Space { get; set; }
        bool Taken { get; set; }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Interval)
            {
                Space = InputManager.IsNewKey(Keys.Space);
                Taken = InputManager.IsNewKey(Keys.E) ? true : Taken;
                GearWheels[0].UpdateRotation(0.005f);
                GearWheels[1].UpdateRotation(-0.01f);
                for (int i = 0; i < NUM_GEAR_WHEELS; ++i)
                {
                    float? collision = Collision(new Ray(Camera.Position, (Camera as Camera2).Direction), i);
                    if (collision < 30 && collision != null && Takables[0].IsGrabbed)
                    {
                        PressSpaceLabel.Visible = true;
                        if (Space && Takables[i].IsGrabbed)
                        {
                            if (!Placed[i])
                            {
                                Takables[i].Visible = false;
                                Takables[i].Enabled = false;
                                Taken = false;
                                GearWheels[i].Visible = true;
                                Placed[i] = true;
                                Space = false;
                            }
                            else if (!Taken)
                            {
                                GearWheels[i].Visible = false;
                                Takables[i].Visible = true;
                                Takables[i].Enabled = true;
                                Taken = true;
                                Placed[i] = false;
                                Space = false;
                            }
                        }
                    }
                    else
                    {
                        PressSpaceLabel.Visible = false;
                    }
                }
                
                Timer = 0;
            }
            base.Update(gameTime);
        }

        public float? Collision(Ray ray, int i)
        {
            return BoundingSphere(i).Intersects(ray);
        }

        public BoundingSphere BoundingSphere(int i)
        {
            return new BoundingSphere(Positions[i], Radius);
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using XNAProject;
using System.IO;
using System.Linq;
using System;

namespace HyperV
{
    public class ButtonPuzzle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        float MINIMAL_DISTANCE = 10;

        bool FirstButton { get; set; }
        bool SecondButton { get; set; }
        bool ThirdButton { get; set; }
        bool FourthButton { get; set; }
        public bool IsCompleted { get; set; }
        int SaveIndex { get; set; }
        float alpha { get; set; }
        float UpdateTimeElapsed { get; set; }
        int[] ButtonOrder { get; set; }
        List<ModelCreator> ButtonList { get; set; }
        string ButtonPositions { get; set; }
        InputManager InputMgrs { get; set; }
        GamePadManager GameControllerMgr { get; set; }
        Camera2 Camera { get; set; }
        RessourcesManager<SoundEffect> SoundManager { get; set; }
        SoundEffect BellSuccess { get; set; }
        SoundEffect BellMissed { get; set; }
        SoundEffect PuzzleComleted { get; set; }

        public ButtonPuzzle(Game game, int[] buttonOrder, string buttonPositions, int saveIndex)
            : base(game)
        {
            ButtonOrder = buttonOrder;
            ButtonPositions = buttonPositions;
            SaveIndex = saveIndex;
        }

        protected override void LoadContent()
        {
            InputMgrs = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GameControllerMgr = Game.Services.GetService(typeof(GamePadManager)) as GamePadManager;
            Camera = Game.Services.GetService(typeof(Camera)) as Camera2;
            SoundManager = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
        }

        public override void Initialize()
        {
            base.Initialize();
            ButtonList = new List<ModelCreator>();
            StreamReader file = new StreamReader(ButtonPositions);
            StreamReader save = new StreamReader("../../../WPFINTERFACE/Launching Interface/Saves/SaveButtonPuzzle" + SaveIndex + ".txt");
            MustSave = true;

            string saveLine = save.ReadLine();
            file.ReadLine();
            while (!file.EndOfStream)
            {
                string lineRead = file.ReadLine();
                string[] lineSplit = lineRead.Split(';');
                ModelCreator x = new ModelCreator(Game, lineSplit[0], new Vector3(float.Parse(lineSplit[1]), float.Parse(lineSplit[2]), float.Parse(lineSplit[3])), int.Parse(lineSplit[4]), int.Parse(lineSplit[5]), "Rock");
                Game.Components.Add(new Displayer3D(Game));
                Game.Components.Add(x);
                ButtonList.Add(x);
            }
            BellSuccess = SoundManager.Find("Bell_Success");
            BellMissed = SoundManager.Find("Bell_Missed");
            PuzzleComleted = SoundManager.Find("ButtonPuzzleCompleted");
            alpha = 0;
            FirstButton = false;
            SecondButton = false;
            ThirdButton = false;
            FourthButton = false;
            IsCompleted = false;
            if (saveLine == "True")
            {
                IsCompleted = true;
            }
            file.Close();
            save.Close();
        }

        float? FindDistance(Ray otherObject, BoundingSphere CollisionSphere)
        {
            return CollisionSphere.Intersects(otherObject);
        }

        public override void Update(GameTime gameTime)
        {
            //Cheat to put in the title the order of buttons to press !
            if (InputMgrs.IsPressede(Microsoft.Xna.Framework.Input.Keys.B))
            {
                Game.Window.Title = ButtonOrder[0].ToString() + ButtonOrder[1].ToString() + ButtonOrder[2].ToString() + ButtonOrder[3].ToString();
            }


            if (InputMgrs.IsNewLeftClick() || InputMgrs.IsNewKey(Microsoft.Xna.Framework.Input.Keys.R) || GameControllerMgr.IsNewButton(Microsoft.Xna.Framework.Input.Buttons.A))
            {
                for (int i = 0; i < ButtonList.Capacity; ++i)
                {
                    if (IsWithinRightDistance(ButtonList[i]))
                    {
                        ButtonList[i].ButtonDisplacement = true;
                        VerifyOrder(i);
                    }
                }
            }
            foreach (ModelCreator button in ButtonList)
            {
                if (button.ButtonDisplacement)
                {
                    DisplaceButton(button, gameTime);
                }
            }
            if (FourthButton)
            {
                IsCompleted = true;
                Save();
            }
        }

        bool IsWithinRightDistance(ModelCreator model)
        {
            float? minDistance = float.MaxValue;
            BoundingSphere sphere = new BoundingSphere(model.GetPosition(), 2.14f);
            float? distance = FindDistance(new Ray(Camera.Position, Camera.Direction), sphere);
            if (minDistance > distance)
            {
                minDistance = distance;
            }
            return minDistance < MINIMAL_DISTANCE;
        }

        void VerifyOrder(int buttonActivated)
        {
            bool mustContinue = true;
            if (FirstButton && SecondButton && ThirdButton && !FourthButton)
            {
                FourthButton = TestFourthButton(buttonActivated);
                mustContinue = false;
            }

            if (FirstButton && SecondButton && !ThirdButton && mustContinue)
            {
                ThirdButton = TestThirdButton(buttonActivated);
                mustContinue = false;
            }
            if (FirstButton && !SecondButton && mustContinue)
            {
                SecondButton = TestSecondButton(buttonActivated);
                mustContinue = false;
            }
            if (!FirstButton && mustContinue)
            {
                FirstButton = TestFirstButton(buttonActivated);
            }
        }

        bool TestFirstButton(int buttonActivated)
        {
            bool isOk = false;
            if (buttonActivated == ButtonOrder[0]) //if button is good and not pressed correctly
            {
                BellSuccess.Play();
                isOk = true;
            }
            else
            {
                BellMissed.Play();
            }
            return isOk;
        }

        bool TestSecondButton(int buttonActivated)
        {
            bool isOk = false;
            if (buttonActivated == ButtonOrder[1]) //if button is good and not pressed correctly
            {
                BellSuccess.Play();
                isOk = true;
            }
            else
            {
                BellMissed.Play();
                FirstButton = false;
            }
            return isOk;
        }

        bool TestThirdButton(int buttonActivated)
        {
            bool isOk = false;
            if (buttonActivated == ButtonOrder[2]) //if button is good and not pressed correctly
            {
                BellSuccess.Play();
                isOk = true;
            }
            else
            {
                BellMissed.Play();
                FirstButton = false;
                SecondButton = false;
            }
            return isOk;
        }

        bool TestFourthButton(int buttonActivated)
        {
            bool isOk = false;
            if (buttonActivated == ButtonOrder[3]) //if button is good and not pressed correctly
            {
                PuzzleComleted.Play();
                isOk = true;
            }
            else
            {
                BellMissed.Play();
                FirstButton = false;
                SecondButton = false;
                ThirdButton = false;
            }
            return isOk;
        }

        void DisplaceButton(ModelCreator button, GameTime gameTime)
        {
            UpdateTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (UpdateTimeElapsed >= 1 / 60f)
            {
                if (button.ButtonDisplacement)
                {
                    button.DisplaceModel(0.03f * new Vector3(0, 0, -(float)(Math.Cos(MathHelper.ToRadians(alpha)))));
                    alpha += 10;
                    if (alpha > 180)
                    {
                        button.ButtonDisplacement = false;
                        alpha = 0;
                    }
                }
                UpdateTimeElapsed = 0;
            }
        }


        bool MustSave { get; set; }
        void Save()
        {
            if(MustSave)
            {
                StreamWriter writer = new StreamWriter("../../../WPFINTERFACE/Launching Interface/Saves/SaveButtonPuzzle" + SaveIndex.ToString() + ".txt");
                writer.WriteLine(true);
                writer.Close();
                MustSave = false;
            }
        }
    }
}

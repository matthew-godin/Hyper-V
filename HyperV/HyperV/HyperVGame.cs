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
using System.IO;
using XNAProject;


namespace HyperV
{
    public class HyperVGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Skybox Skybox { get; set; }
        const string FILE_PATH = "../../";
        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        Rectangle DisplayZone { get; set; }
        Camera GameCamera { get; set; }
        Song GameSong { get; set; }
        InputManager InputMgr { get; set; }
        List<string[]> modelList { get; set; } //liste de tous les models a placer dans un niveau (qui sont dans le file texte)

        public HyperVGame(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            base.Initialize();
            DisplayZone = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            GameCamera = Game.Services.GetService(typeof(Camera)) as Camera;
            RessourcesManager<Song> gestionnaireDeMusiques = Game.Services.GetService(typeof(RessourcesManager<Song>)) as RessourcesManager<Song>;
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
            RessourcesManager<SoundEffect> gestionnaireDeSons = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
            modelList = new List<string[]>();            
        }
        
        public override void Update(GameTime gameTime)
        {

        }

        private void InitialiserCamera()
        {
            Vector3 positionCamera = Vector3.One;
        }

        private void ReadLevelFile(string fileName)
        {
            StreamReader file = new StreamReader(FILE_PATH + fileName);
            while (file.EndOfStream)
            {
                string lineRead = file.ReadLine();
                modelList.Add(lineRead.Split(';'));  //1.model name, 2.nom texture model, 3.position x, 4.position y, 5.position z, 6.scale

            }
        }        
    }
}

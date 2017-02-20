using XNAProject;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace HyperV
{
    public class HyperVGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Skybox Skybox { get; set; }
        const string FILE_PATH = "../../../";
        const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
        Rectangle DisplayZone { get; set; }
        Camera GameCamera { get; set; }
        Song GameSong { get; set; }
        InputManager InputMgr { get; set; }
        List<string> modelList { get; set; } //liste de tous les models a placer dans un niveau (qui sont dans le file texte)

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
            modelList = new List<string>();
            ReadLevelFile("Hub.txt");         
        }
        
        public override void Update(GameTime gameTime)
        {

        }

        private void ReadLevelFile(string fileName)
        {
            StreamReader file = new StreamReader(FILE_PATH + fileName);
            while (!file.EndOfStream)
            {
                string[] lineRead = file.ReadLine().Split(';');
                foreach(string s in lineRead)
                {
                    modelList.Add(s);  //0.model name, 1.position x, 2.position y, 3.position z, 4.scale, 5.rotation
                }
                Niveau model = new Niveau(Game, modelList[0], new Vector3(float.Parse(modelList[1]), float.Parse(modelList[2]), float.Parse(modelList[3])));
                Game.Components.Add(model);
            }
        }        
    }
}

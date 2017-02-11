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
    public class Maze //: BasicPrimitive
    {
        Vector3 Position { get; set; }
        float UpdateInterval { get; set; }
        protected InputManager InputMgr { get; private set; }
        float TimeElapsedSinceUpdate { get; set; }

        const int NUM_TRIANGLES = 2;
        protected Vector3[,] VerticesPts { get; private set; }
        Vector3 Origin { get; set; }
        Vector2 Delta { get; set; }
        protected BasicEffect BscEffect { get; private set; }

        //VertexPositionColor[] Vertices { get; set; }
        RessourcesManager<Texture2D> TextureMgr;
        Texture2D TileTexture;
        VertexPositionTexture[] Vertices { get; set; }
        BlendState AlphaMgr { get; set; }

        Vector2[,] TexturePts { get; set; }
        string NomTileTexture { get; set; }
        string MazeImageName { get; set; }

        public Maze(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector2 span, string nomTileTexture, float updateInterval, string mazeImageName) //: base(game, initialScale, initialRotation, initialPosition)
        {
            UpdateInterval = updateInterval;
            NomTileTexture = nomTileTexture;
            MazeImageName = mazeImageName;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        //public override void Initialize()
        //{
        //    TextureMgr = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
        //    NumVertices = ;
        //    VerticesPts = new Vector3[2, 2];
        //    CreatePointArray();
        //    CreateVertexArray();
        //    Position = InitialPosition;
        //    InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
        //    TimeElapsedSinceUpdate = 0;
        //    base.Initialize();
        //}

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        //public override void Update(GameTime gameTime)
        //{
        //    // TODO: Add your update code here

        //    base.Update(gameTime);
        //}
    }
}

/*
Character.cs
------------

By Matthew Godin

Role : Used to create a non-playable
       character rendered with a .fbx 
       3d model that can talk to the
       player

Created : 2/28/17
*/
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
    public class Character : BaseObject
    {
        string TextFile { get; set; }
        string FaceImageName { get; set; }
        string ScriptRectangleName { get; set; }
        float Interval { get; set; }
        float Radius { get; set; }
        CharacterScript CharacterScript { get; set; }
        string FontName { get; set; }
        float LabelInterval { get; set; }

        public Character(Game game, string modelName, float startScale, Vector3 startRotation, Vector3 startPosition, string textFile, string faceImageName, string scriptRectangleName, string fontName, float labelInterval) : base(game, modelName, startScale, startRotation, startPosition)
        {
            TextFile = textFile;
            FaceImageName = faceImageName;
            ScriptRectangleName = scriptRectangleName;
            FontName = fontName;
            LabelInterval = labelInterval;
            Radius = 9;
        }

        public void UpdateLanguage()
        {
            CharacterScript.UpdateLanguage();
        }

        public void AddLabel()
        {
            CharacterScript = new CharacterScript(Game, this, FaceImageName, TextFile, ScriptRectangleName, FontName, LabelInterval);
            Game.Components.Add(CharacterScript);
        }

        public void RemoveLabel()
        {
            Game.Components.Remove(CharacterScript.PressSpaceLabel);
            Game.Components.Remove(CharacterScript);
        }

        public Vector3 GetPosition()
        {
            return new Vector3(Position.X, Position.Y, Position.Z);
        }

        ///// <summary>
        ///// Allows the game component to update itself.
        ///// </summary>
        ///// <param name="gameTime">Provides a snapshot of timing values.</param>
        //public override void Update(GameTime gameTime)
        //{
        //    // TODO: Add your update code here

        //    base.Update(gameTime);
        //}

        public float? Collision(Ray ray)
        {
            return BoundingSphere.Intersects(ray);
        }

        public BoundingSphere BoundingSphere { get { return new BoundingSphere(Position, Radius); } }
    }
}

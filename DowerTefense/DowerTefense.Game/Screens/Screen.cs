using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using LibrairieTropBien.GUI;
using LibrairieTropBien.Network;
using DowerTefense.Game.Managers;
using Microsoft.Xna.Framework.Input;

namespace DowerTefense.Game.Screens
{


    public abstract class Screen
    {
        public GraphicsDeviceManager Graphics;
        //public SpriteBatch spriteBatch;
        /// <summary>
        /// Liste des éléments graphiques de l'écran
        /// </summary>
        public List<GuiElement> UIElementsList;
        public int leftMargin;
        public int topMargin;
        //Stock les requetes serveur
        public List<Message> Messages;


        public Screen()
        {
            UIElementsList = new List<GuiElement>();
            Messages = new List<Message>();
            this.leftMargin = 5;
            this.topMargin = 5;
        }


        public virtual void LoadContent()
        {

        }
        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            // Affichage du curseur
            Vector2 lol = Microsoft.Xna.Framework.Input.Mouse.GetState().Position.ToVector2();
            Texture2D fap = CustomContentManager.Textures["bananaCursor"];
            _spriteBatch.Draw(fap, lol, Color.White);
        }
        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Initialize(GraphicsDeviceManager _graphics)
        {
            //this.mouseIcone = CustomContentManager.Textures["bananaCursor"];
        }
        public virtual void TreatMessages() { }
    }



}

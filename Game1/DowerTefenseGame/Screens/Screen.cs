using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using LibrairieTropBien.GUI;

namespace DowerTefenseGame.Screens
{


    abstract class Screen
    {
        public GraphicsDeviceManager Graphics;

        /// <summary>
        /// Liste des éléments graphiques de l'écran
        /// </summary>
        public List<GuiElement> UIElementsList;
        public Screen()
        {
            UIElementsList = new List<GuiElement>();
        }

        public virtual void LoadContent()
        {

        }
        public virtual void Draw(SpriteBatch spritebatch)
        {
        }
        public virtual void Draw(SpriteBatch spritebatch, Vector2 pos, Color col)
        {
        }

        public virtual void Draw(SpriteBatch spritebatch, Vector2 pos, Color col, string _what)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Initialize(GraphicsDeviceManager _graphics)
        {
        }
    }



}

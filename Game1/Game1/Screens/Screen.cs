using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DowerTefenseGame.Screens
{


    abstract class Screen
    {
        public Screen() { }
        public GraphicsDeviceManager Graphics;
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

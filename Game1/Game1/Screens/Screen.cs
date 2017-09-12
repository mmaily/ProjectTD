using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Screens
{
    

    class Screen
    {
        public Screen() { }

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
    }



}

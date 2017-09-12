using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.Managers;

namespace Game1.Screens
{
    class GameScreen : Screen
    {

        Texture2D textureToDraw;

        public GameScreen()
        {
        }


        public override void Draw (SpriteBatch spritebatch, Vector2 pos, Color col, string _what)
        {

            textureToDraw = CustomContentManager.GetInstance().Textures[_what];

            if (_what.Equals("cursor"))
            {
                spritebatch.Draw(textureToDraw, pos, col);

            }
            else
            {
                spritebatch.Draw(textureToDraw, pos - new Vector2(textureToDraw.Height / 2, textureToDraw.Width / 2), col);
                //spritebatch.DrawString(CustomContentManager.GetInstance().Fonts["defaultFont"], pos.ToString(), Vector2.Zero, Color.YellowGreen);
            }

        }
    }
}

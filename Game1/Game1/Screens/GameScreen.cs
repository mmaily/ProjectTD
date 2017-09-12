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

        Texture2D mobTexture;

        public GameScreen()
        {
        }


        public override void Draw (SpriteBatch spritebatch, Vector2 pos, Color col)
        {

            mobTexture = CustomContentManager.GetInstance().Textures["unit"];

            spritebatch.Draw(mobTexture, pos - new Vector2(mobTexture.Height / 2, mobTexture.Width / 2), col);
            //spritebatch.DrawString(CustomContentManager.GetInstance().Fonts["defaultFont"], pos.ToString(), Vector2.Zero, Color.YellowGreen);
        }
    }
}

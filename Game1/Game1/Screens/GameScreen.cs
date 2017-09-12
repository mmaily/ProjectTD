using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.Managers;
using Game1.GameElements;

namespace Game1.Screens
{
    class GameScreen : Screen
    {
        Texture2D textureToDraw;
        GraphicsDeviceManager graphics;
        public Map map;
        public GameScreen()
        {
        }

        public override void LoadContent()
        {
            // Chargement du manager de map
            MapManager mapManager = MapManager.GetInstance();
            //Récupération de la map choisie par le manager (une seule pour l'instant)
            map = mapManager.GetMap();
            //// Réglage de la taille de l'écran selon la carte
            graphics.PreferredBackBufferHeight = map.mapHeight * map.tileSize;
            graphics.PreferredBackBufferWidth = map.mapWidth * map.tileSize;
            graphics.ApplyChanges();
        }

        public override void Draw(SpriteBatch spritebatch, Vector2 pos, Color col, string _what)
        {

            // Affichage de la carte
            MapManager.GetInstance().GetMap().Draw(spritebatch);

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



            //Draw the unit on the screen using the method in the UnitsManager
            UnitsManager.GetInstance().Draw(spritebatch);

            

        }

        public override void Update(GameTime gameTime)
        {
            UnitsManager.GetInstance().Update(gameTime);

        }
    }
        

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.Managers;
using Game1.GameElements;
using Microsoft.Xna.Framework.Input;

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
        public override void Update(GameTime gameTime)
        {
            UnitsManager.GetInstance().Update(gameTime);
            //#region Sélection d'une tuile
            //// Récupération de l'état de la souris
            //MouseState mouseState = Mouse.GetState();
            //// Récupération de la position de la souris
            //Point mousePosition = mouseState.Position;
            //// Si la souris est dans l'écran
            //if (GetGraphicsDevice().Viewport.Bounds.Contains(mousePosition))
            //{
            //    // On récupère la tuile visée
            //    Tile selectedTile = map.Tiles[mousePosition.Y / map.tileSize, mousePosition.X / map.tileSize];
            //    // On marque la tuile comme sélectionnée
            //    selectedTile.selected = true;
            //}
            //#endregion
        }

        public override void Draw(SpriteBatch spritebatch)
        {

            // Affichage de la carte
            MapManager.GetInstance().Draw(spritebatch);
            //Draw the unit on the screen using the method in the UnitsManager
            UnitsManager.GetInstance().Draw(spritebatch);
            spritebatch.Draw(textureToDraw, Mouse.GetState().Position.ToVector2(), Color.White);


        }
        public override void Draw(SpriteBatch spritebatch, Vector2 pos, Color col, string _what)
        {

            textureToDraw = CustomContentManager.GetInstance().Textures[_what];




                //spritebatch.Draw(textureToDraw, pos - new Vector2(textureToDraw.Height / 2, textureToDraw.Width / 2), col);
            //spritebatch.DrawString(CustomContentManager.GetInstance().Fonts["defaultFont"], pos.ToString(), Vector2.Zero, Color.YellowGreen);

        }




    }
        

}

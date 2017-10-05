using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownerTefense.Game.Managers
{
    /// <summary>
    /// Gestionnaire du jeu
    /// </summary>
    public static class GameManager
    {

        public static void Initialize()
        {
            //Calcule le facteur d'échelle entre les texture (en général 64px) sur la taille des Tiles
            this.imageRatio = (float)CurrentMap.tileSize / (float)CustomContentManager.GetInstance().textureSize;
            marginOffset = new Vector2(ScreenManager.GetInstance().Screens["GameScreen"].leftMargin, ScreenManager.GetInstance().Screens["GameScreen"].topMargin);
        }

        /// <summary>
        /// Mise à jour des éléments de jeu
        /// </summary>
        /// <param name="_gameTime"></param>
        public static void Update(GameTime _gameTime)
        {

        }

        /// <summary>
        /// Affichage des éléments de jeu
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public static void Draw(SpriteBatch _spriteBatch)
        {
            #region === Affichage map ===


            // Pour chaque tuile de la carte
            foreach (Tile tile in CurrentMap.Tiles)
            {
                // On affiche la texture correspondant à la nature de la carte
                spriteBatch.Draw(contentManager.Textures[tile.TileType.ToString()], new Vector2(tile.line * CurrentMap.tileSize, tile.column * CurrentMap.tileSize) + marginOffset, null, null, null, 0f, Vector2.One * imageRatio, Color.White);
                // Si cette tuile est sélectionnée ou sous le curseur
                if (tile.selected || tile.overviewed)
                {
                    // On affiche la texture "sélectionnée" sur cette tuile
                    spriteBatch.Draw(contentManager.Textures["Mouseover"], new Vector2(tile.line * CurrentMap.tileSize, tile.column * CurrentMap.tileSize) + marginOffset, null, null, null, 0f, Vector2.One * imageRatio, Color.White);
                    // On reset le boolée "sous le curseur"
                    tile.overviewed = false;
                }

            }
            #endregion

            // Affichage des bâtiments
            //foreach (Building building in DefenseBuildingsList)
            //{
            //    _spriteBatch.Draw(CustomContentManager.GetInstance().Textures[building.name],
            //                    new Vector2(building.GetTile().line * map.tileSize, building.GetTile().column * map.tileSize) + MapEngine.GetInstance().marginOffset,
            //                    null, null, null, 0f, Vector2.One * imageRatio,
            //                    Color.White);
            //}


        }
    }
}

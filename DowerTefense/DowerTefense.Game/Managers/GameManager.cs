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

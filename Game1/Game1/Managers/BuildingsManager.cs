using DowerTefenseGame.GameElements;
using Game1.GameElements.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefenseGame.Managers
{

    /// <summary>
    /// Gestionnaire de bâtiments
    /// </summary>
    public class BuildingsManager
    {

        // Instance du gestionnaire de bâtiments
        private static BuildingsManager instance;

        /// <summary>
        /// Liste de tous les bâtiments
        /// </summary>
        public List<Building> BuildingsList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public BuildingsManager()
        {
            BuildingsList = new List<Building>();
        }

        /// <summary>
        /// Récupération de l'instance du gestionnaire d'interface
        /// </summary>
        /// <returns></returns>
        public static BuildingsManager GetInstance()
        {
            if (instance == null)
            {

                instance = new BuildingsManager();

            }
            return instance;
        }

        /// <summary>
        /// Mise à jour des bâtiments
        /// </summary>
        /// <param name="_gameTime"></param>
        public void Update(GameTime _gameTime)
        {

            

        }

        /// <summary>
        /// Affichage des bâtiments
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            Map map = MapManager.GetInstance().map;
            // Pour chaque bâtiment
            foreach (Building building in BuildingsList)
            {
                _spriteBatch.Draw(CustomContentManager.GetInstance().Textures[building.name], new Vector2(building.Tile.column * map.tileSize, building.Tile.line * map.tileSize), Color.White);
            }

        }

    }
}


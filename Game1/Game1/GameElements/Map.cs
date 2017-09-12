using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Screens;
using Game1.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1.GameElements
{
    public class Map
    {
        /// <summary>
        /// Nom de la carte
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Array en deux dimensions représentant la carte
        /// </summary>
        public Tile[,] Tiles { get; private set; }

        // Taille d'une tuile en pixels
        public byte tileSize = 32;
        // Taille de la carte
        public byte mapHeight = 8;
        public byte mapWidth = 8;

        /// <summary>
        /// Constructeur de la carte
        /// </summary>
        public Map()
        {
            // Intialisation du nom de la carte
            this.Name = "Carte démo";

            // Initialisation des tuiles
            this.Tiles = new Tile[mapHeight, mapWidth];

            // Génération de la carte (en dur pour le moment, TODO)
            for (int line = 0; line < mapHeight; line++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    // Création d'un chemin horizontal
                    if (line == 4)
                    {
                        Tiles[line, col] = new Tile(Tile.TileTypeEnum.Path);
                    }
                    else
                    {
                        Tiles[line, col] = new Tile(Tile.TileTypeEnum.Blocked);
                    }
                }
            }

        }

        /// <summary>
        /// Affichage de la carte
        /// </summary>
        /// <param name="_spriteBatch">Instance du ScreenManager</param>
        internal void Draw(SpriteBatch _spriteBatch)
        {

            for (int line = 0; line < mapWidth; line++)
            {
                for (int col = 0; col < mapHeight; col++)
                {
                    _spriteBatch.Draw(CustomContentManager.GetInstance().Textures[Tiles[line, col].TileType.ToString()], new Vector2(line * tileSize, col * tileSize), Color.White);
                }
            }

        }
    }
}

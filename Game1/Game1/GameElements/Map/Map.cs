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

        /// <summary>
        /// Liste des tuiles spawn
        /// </summary>
        public List<Tile> Spawns { get; private set; }
        /// <summary>
        /// Liste des tuiles bases
        /// </summary>
        public List<Tile> Bases { get; private set; }
        /// <summary>
        /// Liste des tuiles chemin
        /// </summary>
        public List<Tile> Paths { get; private set; }

        // Taille d'une tuile en pixels
        public byte tileSize = 32;
        // Taille de la carte
        public byte mapHeight = 8;
        public byte mapWidth = 16;



        /// <summary>
        /// Constructeur de la carte
        /// </summary>
        public Map()
        {
            // Intialisation du nom de la carte
            this.Name = "Carte démo";

            // Initialisation des listes de spawns et de bases
            Spawns = new List<Tile>();
            Bases = new List<Tile>();
            Paths = new List<Tile>();

            // Initialisation des tuiles
            this.Tiles = new Tile[mapHeight, mapWidth];

            // Génération de la carte (en dur pour le moment, TODO)
            for (int line = 0; line < mapHeight; line++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    Tile newTile = new Tile(Tile.TileTypeEnum.Blocked, line, col);
                    // Création d'un chemin horizontal
                    if (line == 4)
                    {
                        newTile.TileType = Tile.TileTypeEnum.Path;
                        Paths.Add(newTile);
                    }
                    Tiles[line, col] = newTile;
                }
            }

            // Chicane
            Tiles[4, 8].TileType = Tile.TileTypeEnum.Blocked;
            Tiles[3, 7].TileType = Tile.TileTypeEnum.Path;
            Tiles[3, 8].TileType = Tile.TileTypeEnum.Path;
            Tiles[3, 9].TileType = Tile.TileTypeEnum.Path;
            Paths.Add(Tiles[3, 7]);
            Paths.Add(Tiles[3, 8]);
            Paths.Add(Tiles[3, 9]);


            // Ajout d'un spawn
            Tiles[4, 0].TileType = Tile.TileTypeEnum.Spawn;
            Spawns.Add(Tiles[4, 0]);
            Tiles[4, mapWidth-1].TileType = Tile.TileTypeEnum.Base;
            Bases.Add(Tiles[4, mapWidth - 1]);


        }

        /// <summary>
        /// Affichage de la carte
        /// </summary>
        /// <param name="_spriteBatch">Instance du ScreenManager</param>
        internal void Draw(SpriteBatch _spriteBatch, CustomContentManager _contentManager)
        {

            for (int line = 0; line < mapHeight; line++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    _spriteBatch.Draw(_contentManager.Textures[Tiles[line, col].TileType.ToString()], new Vector2(col * tileSize, line * tileSize), Color.White);
                }
            }

        }

        /// <summary>
        /// Permet de calculer le chemin
        /// </summary>
        internal void ComputePath()
        {
            // Création de la liste des tuiles à traiter
            List<Tile> queue = new List<Tile>();

            // Remise à zéro de toutes les tuiles à traiter
            foreach (Tile tile in Paths)
            {
                tile.explorated = false;
            }

            // Ajout de ou des tuiles d'arrivée dans la liste à traiter
            queue.AddRange(Bases);

            // Traitement de toute la queue tant qu'elle n'est pas vide
            while(queue.Count != 0)
            {
                // Récupération du premier élément de la liste
                Tile thisTile = queue[0];

                // Ajout de tous les voisins
                List<Tile> neighbours = new List<Tile>();
                if(thisTile.line != 0) neighbours.Add(Tiles[thisTile.line - 1, thisTile.column]);
                if (thisTile.line != mapHeight - 1) neighbours.Add(Tiles[thisTile.line + 1, thisTile.column]);
                if (thisTile.column != 0) neighbours.Add(Tiles[thisTile.line, thisTile.column - 1]);
                if (thisTile.column != mapWidth - 1) neighbours.Add(Tiles[thisTile.line, thisTile.column + 1]);

                // Pour chaque voisin
                foreach (Tile neigh in neighbours)
                {
                    // Si c'est un chemin ou un spawn non exploré
                    if ( (neigh.TileType == Tile.TileTypeEnum.Path || neigh.TileType == Tile.TileTypeEnum.Spawn ) && neigh.explorated == false)
                    {
                        neigh.explorated = true;
                        queue.Add(neigh);
                        neigh.NextTile = thisTile;
                    }
                }

                // On enlève la tuile que l'on vient de traiter
                queue.Remove(thisTile);
            }


        }
    }
}

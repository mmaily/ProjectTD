using DowerTefenseGame.Managers;
using Game1.GameElements.Units.Buildings;
using System.Collections.Generic;

namespace DowerTefenseGame.GameElements
{
    /// <summary>
    /// Classe représentant la carte en cours
    /// </summary>
    public class Map
    {

        //TEMPORAIRE
        public Tile towerTile;
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
        public byte tileSize = 64;
        // Taille de la carte
        public byte mapHeight = 8;
        public byte mapWidth = 16;

        /// <summary>
        /// Constructeur de la carte
        /// </summary>
        public Map()
        {
            // Intialisation du nom de la carte
            this.Name = "Carte demo";

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

            // On définit une tuile comme disponible
            towerTile = Tiles[5, 7];
            towerTile.TileType = Tile.TileTypeEnum.Free;
            // Ajout d'une tour sur cette tuile
            BuildingsManager.GetInstance().BuildingsList.Add(new BasicTower(towerTile));

        }

    }
}

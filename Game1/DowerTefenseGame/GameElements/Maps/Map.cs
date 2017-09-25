using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        public byte mapHeight = 20;
        public byte mapWidth = 20;
        //Gestion de la génération d'une map à partir d'une sauvegarde
        private String path = Path.Combine("Content", "savedMaps");
        public String mapName = "Belle";

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

            Tiles = GenerateMap(openMap(mapName));
            findSpawnBase();

        }
        public void BaseMap()
        {

            // Génération de la carte (en dur pour le moment, TODO)
            for (int line = 0; line < mapHeight; line++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    Tile newTile = new Tile(Tile.TileTypeEnum.Free, line, col);
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
            Tiles[4, mapWidth - 1].TileType = Tile.TileTypeEnum.Base;
            Bases.Add(Tiles[4, mapWidth - 1]);
        }
        private XmlMap openMap(String name)
        {
            //Clear mp for further usage.
            XmlMap mapObject = null;
            //Open the file written above and read values from it.
            Stream stream = File.Open(Path.Combine(path, "Map_" + mapName + ".osl"), FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            mapObject = (XmlMap)bformatter.Deserialize(stream);
            stream.Close();
            return mapObject;
        }
        private Tile[,] GenerateMap(XmlMap TempMap)
        {
            Tile[,] GeneratedMap = new Tile[TempMap.width, TempMap.height];
            this.mapWidth = (byte)TempMap.width;
            this.mapHeight = (byte)TempMap.height;
            this.tileSize = (byte)TempMap.tileSize;

            for (int j = 0; j < TempMap.width; j++)
            {
                for (int k = 0; k < TempMap.height; k++)
                {
                    GeneratedMap[j, k] = TempMap.map[j, k];
                }
            }
            return GeneratedMap;
        }
        private void findSpawnBase()
        {
            foreach( Tile tile in Tiles)
            {
                if (tile.TileType == Tile.TileTypeEnum.Base) Bases.Add(tile);
                if (tile.TileType == Tile.TileTypeEnum.Spawn) Spawns.Add(tile);
            }
        }

    }
}

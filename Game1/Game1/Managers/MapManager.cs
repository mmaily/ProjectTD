using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.GameElements;

namespace Game1.Managers
{
    class MapManager
    {
        private static MapManager instance = null;//L'instance est privée pour empêcher d'autre classe de la modifier. Utiliser le getter GetInstance()
        public Map map;
        private MapManager()
        {
            map = new Map();
        }

        //Créé une seule instance du ScreenManager même si il est appelé plusieurs fois
        public static MapManager GetInstance()
        {
            if (instance == null)
            {

                instance = new MapManager();

            }
            return instance;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

                CustomContentManager contentManager = CustomContentManager.GetInstance();

                for (int line = 0; line < map.mapHeight; line++)
                {
                    for (int col = 0; col < map.mapWidth; col++)
                    {
                        spriteBatch.Draw(contentManager.Textures[map.Tiles[line, col].TileType.ToString()], new Vector2(col * map.tileSize, line * map.tileSize), Color.White);
                        if (map.Tiles[line, col].selected)
                        {
                           spriteBatch.Draw(contentManager.Textures["Mouseover"], new Vector2(col * map.tileSize, line * map.tileSize), Color.White);
                            map.Tiles[line, col].selected = false;
                        }
                    }
                }

            
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 pos, Color col, string _what)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
        internal void ComputePath()
        {
            // Création de la liste des tuiles à traiter
            List<Tile> queue = new List<Tile>();

            // Remise à zéro de toutes les tuiles à traiter
            foreach (Tile tile in map.Paths)
            {
                tile.explorated = false;
            }

            // Ajout de ou des tuiles d'arrivée dans la liste à traiter
            queue.AddRange(map.Bases);

            // Traitement de toute la queue tant qu'elle n'est pas vide
            while (queue.Count != 0)
            {
                // Récupération du premier élément de la liste
                Tile thisTile = queue[0];

                // Ajout de tous les voisins
                List<Tile> neighbours = new List<Tile>();
                if (thisTile.line != 0) neighbours.Add(map.Tiles[thisTile.line - 1, thisTile.column]);
                if (thisTile.line != map.mapHeight - 1) neighbours.Add(map.Tiles[thisTile.line + 1, thisTile.column]);
                if (thisTile.column != 0) neighbours.Add(map.Tiles[thisTile.line, thisTile.column - 1]);
                if (thisTile.column != map.mapWidth - 1) neighbours.Add(map.Tiles[thisTile.line, thisTile.column + 1]);

                // Pour chaque voisin
                foreach (Tile neigh in neighbours)
                {
                    // Si c'est un chemin ou un spawn non exploré
                    if ((neigh.TileType == Tile.TileTypeEnum.Path || neigh.TileType == Tile.TileTypeEnum.Spawn) && neigh.explorated == false)
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
        public Map GetMap()
        {
           return map;
        }
    }
}

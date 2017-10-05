using Microsoft.Xna.Framework;
using System.Collections.Generic;
using DowerTefense.Commons.GameElements;

namespace DowerTefense.Commons.Managers
{

    /// <summary>
    /// Gestionnaire de carte
    /// </summary>
    public static class MapEngine
    {

        /// <summary>
        /// Calcul du chemin
        /// </summary>
        public static void ComputePath(ref Map map)
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

        /// <summary>
        /// Zone de la carte
        /// </summary>
        /// <returns>Rectangle correspondant à la zone de la carte</returns>
        public static Rectangle GetMapZone(Map _map)
        {
            Rectangle rec = new Rectangle(0, 0, _map.mapWidth * _map.tileSize, _map.mapHeight * _map.tileSize);
            return rec;
        }

    }
}

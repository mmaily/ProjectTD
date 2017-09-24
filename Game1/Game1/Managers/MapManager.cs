using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using DowerTefenseGame.GameElements;

namespace DowerTefenseGame.Managers
{

    /// <summary>
    /// Gestionnaire de carte
    /// </summary>
    class MapManager
    {
        // Instance du gestionnaire
        private static MapManager instance = null;
        // Carte en cours
        public Map CurrentMap { get; set; }
        // Booléen de calcul du chemin
        private bool pathComputed = false;
        public float imageRatio;

        /// <summary>
        /// Constructeur du gestionnaire de carte
        /// </summary>
        private MapManager()
        {
            this.CurrentMap = new Map();
            //Calcule le facteur d'échelle entre les texture (en général 64px) sur la taille des Tiles
            this.imageRatio = (float)CurrentMap.tileSize/ (float)CustomContentManager.GetInstance().textureSize;
        }

        /// <summary>
        /// Récupération de l'instance du gestionnaire de carte
        /// </summary>
        /// <returns>Instance du gestionnaires</returns>
        public static MapManager GetInstance()
        {
            // Si l'instance n'est pas encore créée
            if (instance == null)
            {
                instance = new MapManager();
            }
            return instance;
        }

        /// <summary>
        /// Mise à jour de la carte
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public void Update(GameTime gameTime)
        {
            #region  === Calcul du chemin ===
            // Si le chemin a besoin d'être calculé
            if (!pathComputed)
            {
                this.ComputePath();
            }
            #endregion
        }

        /// <summary>
        /// Affichage des éléments de la carte
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Récupération de l'instance du gestionnaire de contenu
            CustomContentManager contentManager = CustomContentManager.GetInstance();

            // Pour chaque tuile de la carte
            foreach (Tile tile in CurrentMap.Tiles)
            {
                // On affiche la texture correspondant à la nature de la carte
                spriteBatch.Draw(contentManager.Textures[tile.TileType.ToString()], new Vector2(tile.line * CurrentMap.tileSize, tile.column * CurrentMap.tileSize), null, null, null, 0f, Vector2.One*imageRatio, Color.White);
                // Si cette tuile est sélectionnée ou sous le curseur
                if (tile.selected || tile.overviewed)
                {
                    // On affiche la texture "sélectionnée" sur cette tuile
                    spriteBatch.Draw(contentManager.Textures["Mouseover"], new Vector2(tile.line * CurrentMap.tileSize, tile.column * CurrentMap.tileSize),null,null,null,0f, Vector2.One*imageRatio, Color.White);
                    // On reset le boolée "sous le curseur"
                    tile.overviewed = false;
                }

            }
            
        }


        /// <summary>
        /// Calcul du chemin
        /// </summary>
        private void ComputePath()
        {
            // Création de la liste des tuiles à traiter
            List<Tile> queue = new List<Tile>();

            // Remise à zéro de toutes les tuiles à traiter
            foreach (Tile tile in CurrentMap.Paths)
            {
                tile.explorated = false;
            }

            // Ajout de ou des tuiles d'arrivée dans la liste à traiter
            queue.AddRange(CurrentMap.Bases);

            // Traitement de toute la queue tant qu'elle n'est pas vide
            while (queue.Count != 0)
            {
                // Récupération du premier élément de la liste
                Tile thisTile = queue[0];

                // Ajout de tous les voisins
                List<Tile> neighbours = new List<Tile>();
                if (thisTile.line != 0) neighbours.Add(CurrentMap.Tiles[thisTile.line - 1, thisTile.column]);
                if (thisTile.line != CurrentMap.mapHeight- 1) neighbours.Add(CurrentMap.Tiles[thisTile.line + 1, thisTile.column]);
                if (thisTile.column != 0) neighbours.Add(CurrentMap.Tiles[thisTile.line, thisTile.column - 1]);
                if (thisTile.column != CurrentMap.mapWidth - 1) neighbours.Add(CurrentMap.Tiles[thisTile.line, thisTile.column + 1]);

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
        public Rectangle GetMapZone()
        {
            Rectangle rec = new Rectangle(0,0,this.CurrentMap.mapWidth*CurrentMap.tileSize, CurrentMap.mapHeight * CurrentMap.tileSize);
                return rec;
        }

    }
}

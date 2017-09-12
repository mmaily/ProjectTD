﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameElements
{
    // Classe fille de tuile
    public class Tile
    {
        // Énumérateur des types de tuiles
        public enum TileTypeEnum
        {
            Path, // Chemin
            Free, // Terrain constructible
            Blocked, // Terrain bloqué
            Spawn, // Point d'apparition des ennemis
            Base, // Base à défendre
        }

        /// <summary>
        /// Prochaine tuile sur le chemin
        /// </summary>
        public Tile NextTile { get; set; }
        // Permet de ne pas passer deux fois sur la même tuile lors du calcul du chemin
        public bool explorated = false;

        public int line;
        public int column;

        // Type de la tuile
        public TileTypeEnum TileType { get; set; }

        /// <summary>
        /// Constructeur par défaut d'une tuile bloquée.
        /// </summary>
        public Tile()
        {
            this.TileType = TileTypeEnum.Blocked;
        }

        /// <summary>
        /// Constructeur spécifiant le type de tuile
        /// </summary>
        /// <param name="_tileType">Type de tuile</param>
        public Tile(TileTypeEnum _tileType)
        {
            this.TileType = _tileType;
        }

        /// <summary>
        /// Constructeur avec type de tuile et position sur la carte
        /// </summary>
        /// <param name="_tileType">Type de la tuile</param>
        /// <param name="_line">Ligne de la tuile</param>
        /// <param name="_y">Colonne de la tuile</param>
        public Tile(TileTypeEnum _tileType, int _line, int _column)
        {
            this.TileType = _tileType;
            this.line = _line;
            this.column = _column;
        }

        /// <summary>
        /// Méthode pour obtenir la position du centre de la tuile
        /// </summary>
        /// <returns>Le Vecteur2 (ligne, colonne). Attention à le multiplier par la taille des tuiles</returns>
        public Vector2 getTilePosition()
        {
            // Création du vecteur de retour
            Vector2 res = new Vector2(this.column + 0.5f, this.line + 0.5f);
            // Retour du vecteur
            return res;
        }

    }
}
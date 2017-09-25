using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Units;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;

namespace DowerTefenseGame.GameElements
{

    /// <summary>
    /// Classe représentant une tuile de la carte
    /// </summary>
    [Serializable()]
    public class Tile : ISerializable
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
        // Taille d'une tuile en pixels
        public static byte tileSize = 32;
        // Position de la tuile
        public int line;
        public int column;

        // Etat de la tuile, si survolée ou pas
        public bool overviewed = false;
        // Etat de la tuile, si sélectionnée ou pas
        public bool selected = false;
        // Type de la tuile
        public TileTypeEnum TileType { get; set; }

        public Building building;

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
            Vector2 res = new Vector2(this.line + 0.5f, this.column + 0.5f);
            // Retour du vecteur
            return res;
        }
        //Partie dédiée à la sérialization
        public Tile(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            TileType = (TileTypeEnum)info.GetValue("TileType", typeof(TileTypeEnum));
            line = (int)info.GetValue("line", typeof(int));
            column = (int)info.GetValue("column", typeof(int));

        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("line", line);
            info.AddValue("column", column);
            info.AddValue("TileType", TileType);
        }
    }

}

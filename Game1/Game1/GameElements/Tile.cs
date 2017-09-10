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
        }

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

    }
}

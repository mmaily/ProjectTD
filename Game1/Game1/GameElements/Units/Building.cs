using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;

namespace Game1.GameElements.Units
{
    /// <summary>
    /// Classe mère des bâtiments
    /// </summary>
    public abstract class Building : Unit
    {

        /// <summary>
        /// Tuile sur laquelle est positionné le bâtiment
        /// </summary>
        public Tile Tile { get; set; }

        public Building() : base()
        {
        }

    }
}

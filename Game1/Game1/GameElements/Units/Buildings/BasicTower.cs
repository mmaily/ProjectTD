using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using System.Collections.Generic;

namespace Game1.GameElements.Units.Buildings
{
    /// <summary>
    /// Tour de base
    /// </summary>
    public class BasicTower : Building
    {

        protected List<Unit> targetList;

        /// <summary>
        /// Constructeur
        /// </summary>
        public BasicTower() : base()
        {
            this.name = "BasicTower";
            this.AttackPower = 1;
            this.Range = 200;
            this.RateOfFire = 1;
            this.UnitType = UnitTypeEnum.Ground;
            this.TargetType = UnitTypeEnum.Ground;
            this.TargetNumber = 1;

            // Initialisation des cibles potentielles
            targetList = new List<Unit>();
        }

        public BasicTower(Tile _tile) : this()
        {
            // On sauvegarde la tuile sur laquelle on se positionne
            this.Tile = _tile;

            //On indique à la tuile que l'on a posé un bâtiment dessus
            _tile.building = this;
        }

    }
}

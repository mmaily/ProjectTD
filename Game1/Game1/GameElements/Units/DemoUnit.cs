using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameElements.Units
{
    public class DemoUnit : Unit
    {

        /// <summary>
        /// Prochaine tuile de destination de cette unitée
        /// </summary>
        public Tile DestinationTile { get; set; }

        /// <summary>
        /// Constructeur de l'unitée démo
        /// </summary>
        public DemoUnit()
        {
            base.Init();
            // Définition des propriétés différentes de la classe de base
            this.Speed = 1f;
            this.UnitType = UnitTypeEnum.Ground;
        }
    }
}

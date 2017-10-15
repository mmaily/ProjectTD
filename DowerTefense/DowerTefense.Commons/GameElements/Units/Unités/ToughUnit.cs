using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Commons.GameElements.Units.Unités
{
    [Serializable()]
    class ToughUnit : Unit
    {
        public ToughUnit() : base()
        {
            // Définition des propriétés différentes de la classe de base
            this.Name = "ToughUnit";
            this.Speed = 0.3f;
            this.UnitType = UnitTypeEnum.Ground;
            this.GoldValue = 10;
            this.AttackPower = 1;
            this.HealthPoints = this.MaxHealthPoints = 50;
        }
        public override Unit DeepCopy()
        {
            ToughUnit other = (ToughUnit)this.MemberwiseClone();
            return other;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Commons.GameElements.Units.Unités
{
    [Serializable()]
    class FastUnit : Unit, ISerializable
    {
        public FastUnit() : base()
        {
            // Définition des propriétés différentes de la classe de base
            this.name = "FastUnit";
            this.Speed = 3f;
            this.UnitType = UnitTypeEnum.Ground;
            this.GoldValue = 10;
            this.AttackPower = 1;
            this.HealthPoints = 5;
        }
        public override Unit DeepCopy()
        {
            FastUnit other = (FastUnit)this.MemberwiseClone();
            return other;
        }
    }
}

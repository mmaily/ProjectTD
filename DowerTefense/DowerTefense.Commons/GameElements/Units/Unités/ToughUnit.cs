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
            //Leveling
            this.BaseSpeed = this.Speed;
            this.SpeedCoeff = 0.1;
            this.SpeedPrice = 100;
            this.SpeedPriceCoeff = 0.5;

            this.BaseMaxHealthPoints = MaxHealthPoints;
            this.MaxHealthPointsCoeff = 0.2;
            this.MaxHealthPointsPrice = 200;
            this.MaxHealthPointsPriceCoeff = 0.4;
        }
        public override Unit DeepCopy()
        {
            ToughUnit other = (ToughUnit)this.MemberwiseClone();
            return other;
        }
    }
}

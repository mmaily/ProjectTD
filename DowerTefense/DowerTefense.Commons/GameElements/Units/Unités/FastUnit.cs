using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Commons.GameElements.Units.Unités
{
    [Serializable()]
    class FastUnit : Unit
    {
        public FastUnit() : base()
        {
            // Définition des propriétés différentes de la classe de base
            this.Name = "FastUnit";
            this.Speed = 3f;
            this.UnitType = UnitTypeEnum.Ground;
            this.GoldValue = 10;
            this.AttackPower = 1;
            this.HealthPoints = this.MaxHealthPoints = 5;
            //Leveling
            this.BaseSpeed = this.Speed;
            this.SpeedCoeff = 0.2;
            this.SpeedPrice = 150;
            this.SpeedPriceCoeff = 0.3;

            this.BaseMaxHealthPoints = MaxHealthPoints;
            this.MaxHealthPointsCoeff = 0.05;
            this.MaxHealthPointsPrice = 150;
            this.MaxHealthPointsPriceCoeff = 0.4;
        }
        public override Unit DeepCopy()
        {
            FastUnit other = (FastUnit)this.MemberwiseClone();
            return other;
        }
    }
}

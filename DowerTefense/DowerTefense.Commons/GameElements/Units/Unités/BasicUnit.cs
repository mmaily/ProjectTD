using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace DowerTefense.Commons.GameElements.Units.Unités
{
    [Serializable()]
    /// <summary>
    /// Unité de démonstration
    /// </summary>
    public class BasicUnit : Unit , ISerializable
    {

        /// <summary>
        /// Constructeur de l'unité démo
        /// </summary>
        public BasicUnit() : base()
        {
            // Définition des propriétés différentes de la classe de base
            this.Name = "unit";
            this.Speed = 1f;
            this.UnitType = UnitTypeEnum.Ground;
            this.GoldValue = 10;
            this.AttackPower = 1;
            this.HealthPoints = this.MaxHealthPoints = 10;
        }
        public override Unit DeepCopy()
        {
            BasicUnit other = (BasicUnit)this.MemberwiseClone();
            return other;
        }
    }
}

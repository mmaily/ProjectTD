using System;
using System.Runtime.Serialization;

namespace DowerTefenseGame.GameElements.Units
{
    [Serializable()]
    /// <summary>
    /// Unité de démonstration
    /// </summary>
    public class Unit : Entity , ISerializable
    {

        /// <summary>
        /// Constructeur de l'unité démo
        /// </summary>
        public Unit() : base()
        {
            // Définition des propriétés différentes de la classe de base
            this.name = "unit";
            this.Speed = 1f;
            this.UnitType = UnitTypeEnum.Ground;
            this.GoldValue = 10;
            this.AttackPower = 1;
            this.HealthPoints = 10;
        }
    }
}

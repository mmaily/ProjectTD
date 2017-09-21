namespace DowerTefenseGame.GameElements.Units
{
    /// <summary>
    /// Unité de démonstration
    /// </summary>
    public class Unit : Entity
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

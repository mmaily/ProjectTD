namespace DowerTefenseGame.GameElements.Units
{
    /// <summary>
    /// Unité de démonstration
    /// </summary>
    public class DemoUnit : Unit
    {

        /// <summary>
        /// Prochaine tuile de destination de cette unité
        /// </summary>
        public Tile DestinationTile { get; set; }

        /// <summary>
        /// Constructeur de l'unité démo
        /// </summary>
        public DemoUnit() : base()
        {
            // Définition des propriétés différentes de la classe de base
            this.name = "unit";
            this.Speed = 1f;
            this.UnitType = UnitTypeEnum.Ground;
            this.GoldValue = 10;
        }
    }
}

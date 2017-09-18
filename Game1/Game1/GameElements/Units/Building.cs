using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using Microsoft.Xna.Framework;
using DowerTefenseGame.Managers;


namespace DowerTefenseGame.Units
{
    /// <summary>
    /// Classe mère des bâtiments
    /// </summary>
    public abstract class Building : Unit
    {

        /// <summary>
        /// Tuile sur laquelle est positionné le bâtiment
        /// </summary>
        public Tile Tile { get; set; }

        public Building() : base()
        {
            CreateOnEventListener();
        }
        //Ecoute l'event 'OnUnitInRange" et add la cible à sa liste
        public void CreateOnEventListener()
        {
            //Event qui prévient la tour qu'une unité est dans la surface-union, dans ce cas on déclenche la 
            //méthode AddTarget(), l'argument de l'event c'est l'objet Unit concerné
            BuildingsManager bd = BuildingsManager.GetInstance();
            bd.BuildingDuty += new BuildingsManager.BuildingDutyHandler(OnDuty);
        }
        public virtual void OnDuty()
        {

        }

    }
}

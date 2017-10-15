using DowerTefense.Commons.GameElements;
using DowerTefense.Commons.GameElements.Units;
using Microsoft.Xna.Framework;
using DowerTefense.Commons.Managers;
using LibrairieTropBien.GUI;
using System;

namespace DowerTefense.Commons.Units
{
    /// <summary>
    /// Classe mère des bâtiments
    /// </summary>
    [Serializable]
    public abstract class Building : Entity
    {

        /// <summary>
        /// Tuile sur laquelle est positionné le bâtiment
        /// </summary>
        private Tile tile;

        public Building() : base()
        {
         //CreateOnEventListener();
        }
        #region===Partie du code avec les events : obsolète===
        //public void CreateOnEventListener()
        //{
        //    //Event qui prévient la tour qu'une unité est dans la surface-union, dans ce cas on déclenche la 
        //    //méthode AddTarget(), l'argument de l'event c'est l'objet Unit concerné
        //    BuildingEngine bd = BuildingEngine.GetInstance();
        //    bd.BuildingDuty += new BuildingEngine.BuildingDutyHandler(OnDuty);
        //}
        //public void DeleteOnEventListener()
        //{
        //    //Event qui prévient la tour qu'une unité est dans la surface-union, dans ce cas on déclenche la 
        //    //méthode AddTarget(), l'argument de l'event c'est l'objet Unit concerné
        //    BuildingEngine bd = BuildingEngine.GetInstance();
        //    bd.BuildingDuty -= new BuildingEngine.BuildingDutyHandler(OnDuty);
        //}
        //public virtual void OnDuty()
        //{

        //}
        #endregion
        public virtual void Update() { }

        /// <summary>
        /// Récupération de la tuile de position
        /// </summary>
        /// <returns>Tuile</returns>
        public Tile GetTile()
        { return tile; }

        /// <summary>
        /// Réglage de la tuile de position. Informe cette dernière qu'un bâtiment est posé dessus.
        /// </summary>
        /// <param name="_tile">Tuile</param>
        public virtual void SetTile(Tile _tile, Map map)
        {
            tile = _tile;
            // On informe la tuile qu'un bâtiment est dessus
            //tile.building = this;
            this.Position = tile.getTilePosition() * map.tileSize;
        }
        public virtual void SetInfoPopUp(InfoPopUp _info) { }
        public abstract Building DeepCopy();
    }
}

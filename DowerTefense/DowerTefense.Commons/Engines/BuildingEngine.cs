namespace DowerTefense.Commons.Managers
{

    /// <summary>
    /// Gestionnaire de bâtiments
    /// </summary>
    public static class BuildingEngine
    {
        #region === Events de portée : obsolète ===
        //// Event enter range
        //public static event UnitInRangeHandler UnitInRange;
        //public delegate void UnitInRangeHandler(UnitRangeEventArgs arg);
        ////Event leave range

        ////Argument commun pour enter/leave range qui donne une unité en param

        //public class UnitRangeEventArgs : EventArgs
        //{
        //    public UnitRangeEventArgs(Entity iUnit)
        //    { unit = iUnit; }
        //    public Entity unit { get; set; }
        //}
        //public static EventArgs e = null;
        ////Event qui appelle les tours à tirer 
        //public static event BuildingDutyHandler BuildingDuty;
        //public delegate void BuildingDutyHandler();
      
        #endregion

        // Ratio entre l'image de la tour et la taille des tiles
        public static float imageRatio;



    }

}


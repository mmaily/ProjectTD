using DowerTefense.Commons.GameElements.Units;
using DowerTefense.Commons.Units;
using DowerTefense.Commons.Units.Buildings;
using System;
using System.Collections.Generic;

namespace DowerTefense.Commons.Managers
{

    /// <summary>
    /// Gestionnaire de bâtiments
    /// </summary>
    public static class BuildingEngine
    {
        #region === Events de portée ===
        // Event enter range
        public static event UnitInRangeHandler UnitInRange;
        public delegate void UnitInRangeHandler(UnitRangeEventArgs arg);
        //Event leave range

        //Argument commun pour enter/leave range qui donne une unité en param

        public class UnitRangeEventArgs : EventArgs
        {
            public UnitRangeEventArgs(Entity iUnit)
            { unit = iUnit; }
            public Entity unit { get; set; }
        }
        public static EventArgs e = null;
        //Event qui appelle les tours à tirer 
        public static event BuildingDutyHandler BuildingDuty;
        public delegate void BuildingDutyHandler();
      
        #endregion

        // Ratio entre l'image de la tour et la taille des tiles
        public static float imageRatio;

        /// <summary>
        /// Retourne une liste contenant les spawners verrouillés pour la prochaine vague
        /// </summary>
        /// <param name="spawners">Liste de tous les spawners du joueur</param>
        /// <returns>Liste des spawners actifs</returns>
        public static List<SpawnerBuilding> LockSpawners(List<SpawnerBuilding> spawners)
        {
            // Init de la liste de retour
            List<SpawnerBuilding> lockedSpawners = new List<SpawnerBuilding>();

            // Pour tous les spawners de la liste en paramètre
            foreach (SpawnerBuilding sp in spawners.FindAll(sp => sp.powered))
            {
                lockedSpawners.Add((SpawnerBuilding)sp.DeepCopy());
            }

            // Retour
            return lockedSpawners;
        }

    }

}


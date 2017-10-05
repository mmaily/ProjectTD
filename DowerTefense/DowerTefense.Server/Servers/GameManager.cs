using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DowerTefense.Commons.Units.Buildings;
using DowerTefense.Commons.Units;

namespace DowerTefense.Server.Servers
{
    /// <summary>
    /// Jeu à proprement dit, côté serveur
    /// </summary>
    public class GameManager : Microsoft.Xna.Framework.Game
    {

        #region === Buildings ===

        /// <summary>
        /// Liste de tous les bâtiments "locked"
        /// </summary>
        public List<SpawnerBuilding> LockedBuildingsList { get; set; }
        /// <summary>
        /// Liste de tous les bâtiments libre
        /// </summary>
        public List<SpawnerBuilding> FreeBuildingsList { get; set; }
        /// <summary>
        /// Liste de tous les bâtiments défensifs
        /// </summary>
        public List<Building> DefenseBuildingsList { get; set; }
        /// <summary>
        /// Liste de construction en attente pour le prochain update
        /// </summary>
        public List<Building> WaitingForConstruction { get; set; }

        #endregion

        /// <summary>
        /// Initialisation du jeu
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Initialisation des listes de bâtiments
            LockedBuildingsList = new List<SpawnerBuilding>();
            FreeBuildingsList = new List<SpawnerBuilding>();
            DefenseBuildingsList = new List<Building>();
            WaitingForConstruction = new List<Building>();
        }

        /// <summary>
        /// Mise à jour du jeu
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Check si les unités pénétrent dans la surface-union
            foreach (Entity unit in UnitEngine.GetInstance().mobs)
            {
                UnitRangeEventArgs arg = new UnitRangeEventArgs(unit);
                UnitInRange?.Invoke(this, arg);
            }

            //Update le temps de jeu écoule
            this.gameTime = _gameTime;

            #region =====Construction des bâtiments en attente=====
            //Construire la liste des tours en attente
            foreach (Building bd in WaitingForConstruction)
            {
                // Retrait du coût du bâtiment
                if (bd.GetType() == typeof(Tower))
                {
                    bd.CreateOnEventListener();
                    InfoPopUp info = new InfoPopUp(new Rectangle((int)((bd.GetTile().getTilePosition().X - 0.5) * UIManager.GetInstance().currentMap.tileSize),
                                                        (int)((bd.GetTile().getTilePosition().Y - 0.5) * UIManager.GetInstance().currentMap.tileSize),
                                                        UIManager.GetInstance().currentMap.tileSize, UIManager.GetInstance().currentMap.tileSize))
                    {
                        Name = bd.GetType().ToString() + "Info",
                        Tag = "InfoPopUp",
                        font = CustomContentManager.GetInstance().Fonts["font"],
                        texture = CustomContentManager.GetInstance().Colors["pixel"],
                        Enabled = true
                    };
                    UIManager.GetInstance().UIElementsList.Add(info);
                    bd.SetInfoPopUp(info);
                    UIManager.GetInstance().defensePlayer.totalGold -= bd.Cost;
                }
                if (bd.GetType() == typeof(SpawnerBuilding))
                {
                    //On le cast en spawner pour appliquer les méthodes propres aux spawner
                    SpawnerBuilding spawner = (SpawnerBuilding)bd;
                    UIManager.GetInstance().attackPlayer.totalGold -= bd.Cost;
                    UIManager.GetInstance().UpdateBtnLists(spawner);
                    BuildingEngine.GetInstance().FreeBuildingsList.Add(spawner);
                    if (UIManager.GetInstance().attackPlayer.totalEnergy - UIManager.GetInstance().attackPlayer.usedEnergy >= (spawner.PowerNeeded))
                    {
                        spawner.powered = true;
                        UIManager.GetInstance().attackPlayer.usedEnergy += spawner.PowerNeeded;
                    }
                }

            }
            //Une fois traitée, on vide les éléments de la waiting List
            WaitingForConstruction.Clear();
            #endregion 
            //Apelle les bâtiments à faire leur actions respectives (si il y a des buildings)
            BuildingDuty?.Invoke();


        }








    }
}

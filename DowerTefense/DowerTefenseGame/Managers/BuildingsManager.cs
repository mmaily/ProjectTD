using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.GameElements.Units.Buildings.AttackBuildings;
using DowerTefenseGame.GameElements.Units.Buildings.DefenseBuildings;
using DowerTefenseGame.Units;
using DowerTefenseGame.Units.Buildings;
using LibrairieTropBien.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DowerTefenseGame.Managers
{

    /// <summary>
    /// Gestionnaire de bâtiments
    /// </summary>
    public class BuildingsManager
    {
        //Définiton des events  
        //"une unité pénétre(ou sors) dans la surface-union" (constituée de l'union de toutes les range)
        //"Tous les bâtiments agissent s'ils le peuvent
        #region
        // Event enter range
        public event UnitInRangeHandler UnitInRange;
        public delegate void UnitInRangeHandler(BuildingsManager bd, UnitRangeEventArgs arg);
        //Event leave range
        //Argument commun pour enter/leave range qui donne une unité en param
        public class UnitRangeEventArgs : EventArgs
        {
            public UnitRangeEventArgs(Entity iUnit)
            { unit = iUnit; }
            public Entity unit { get; set; }
        }
        public EventArgs e = null;
        //Event qui appelle les tours à tirer 
        public event BuildingDutyHandler BuildingDuty;
        public delegate void BuildingDutyHandler();
        //Récupère l'objet GameTime 
        public GameTime gameTime;

      
        #endregion
        // Instance du gestionnaire de bâtiments
        private static BuildingsManager instance;

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
        public object Spawner { get; }


        // Ratio entre l'image de la tour et la taille des tiles
        public float imageRatio;
        

        /// <summary>
        /// Constructeur
        /// </summary>
        public BuildingsManager()
        {
            LockedBuildingsList = new List<SpawnerBuilding>();
            FreeBuildingsList = new List<SpawnerBuilding>();
            DefenseBuildingsList = new List<Building>();
            imageRatio=MapManager.GetInstance().imageRatio;
            WaitingForConstruction = new List<Building>();

        }

        /// <summary>
        /// Récupération de l'instance du gestionnaire de bâtiments
        /// </summary>
        /// <returns></returns>
        public static BuildingsManager GetInstance()
        {
            if (instance == null)
            {

                instance = new BuildingsManager();

            }
            return instance;
        }

        /// <summary>
        /// Mise à jour des bâtiments
        /// </summary>
        /// <param name="_gameTime"></param>
        public void Update(GameTime _gameTime)
        {
            //Check si les unités pénétrent dans la surface-union
            foreach (Entity unit in UnitsManager.GetInstance().mobs)
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
                    BuildingsManager.GetInstance().FreeBuildingsList.Add(spawner);
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

        /// <summary>
        /// Affichage des bâtiments
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            Map map = MapManager.GetInstance().CurrentMap;
            // Pour chaque bâtiment
            foreach (Building building in DefenseBuildingsList)
            {
                _spriteBatch.Draw(CustomContentManager.GetInstance().Textures[building.name],
                                new Vector2(building.GetTile().line * map.tileSize, building.GetTile().column * map.tileSize) + MapManager.GetInstance().marginOffset,
                                null, null, null, 0f, Vector2.One * imageRatio,
                                Color.White);
            }

        }

        public void lockBuildings()
        {
            foreach(Building bd in LockedBuildingsList) { bd.DeleteOnEventListener(); }
            LockedBuildingsList.Clear();
            SpawnerBuilding Spawner;
            foreach (SpawnerBuilding sp in FreeBuildingsList.FindAll(sp => sp.powered))
            {

                Spawner = (SpawnerBuilding)sp.DeepCopy();
                Spawner.Lock();// Une fois lock, le bâtiment commence à spawn
                LockedBuildingsList.Add(Spawner);
            }

        }
    }

}


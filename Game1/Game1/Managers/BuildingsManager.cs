using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Units;
using DowerTefenseGame.Units.Buildings;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
            public UnitRangeEventArgs(Unit iUnit)
            { unit = iUnit; }
            public Unit unit { get; set; }
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
        public List<Building> LockedBuildingsList { get; set; }
        /// <summary>
        /// Liste de tous les bâtiments libre
        /// </summary>
        public List<Building> FreeBuildingsList { get; set; }
        /// <summary>
        /// Liste de tous les bâtiments libre
        /// </summary>
        public List<Building> DefenseBuildingsList { get; set; }
        /// <summary>
        /// Liste de construction en attente pour le prochain update
        /// </summary>
        public List<Building> WaitingForConstruction { get; set; }
        /// <summary>
        /// Dictionnaire des prix des bâtiments
        /// </summary>
        public Dictionary<String, int> Price;
        //public CombinedGeometry coveredArea;
        public GeometryGroup coveredArea;

        /// <summary>
        /// Constructeur
        /// </summary>
        public BuildingsManager()
        {
           
            LockedBuildingsList = new List<Building>();
            FreeBuildingsList = new List<Building>();
            DefenseBuildingsList = new List<Building>();
            coveredArea = new GeometryGroup();
            WaitingForConstruction = new List<Building>();
            Price = new Dictionary<string, int>();
            SetPrice();
            // On instancie l'objet GameTime
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
            foreach (Unit unit in UnitsManager.GetInstance().mobs)
            {
                    UnitRangeEventArgs arg = new UnitRangeEventArgs(unit);
                    UnitInRange?.Invoke(this, arg);
            }
            //Update le temps de jeu écoule
            this.gameTime = _gameTime;

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
                                new Vector2(building.GetTile().column * map.tileSize, building.GetTile().line * map.tileSize),
                                Microsoft.Xna.Framework.Color.White);
            }

        }
        /// <summary>
        /// Méthode pour ajouter les prix des bâtiments au dictionnaire
        /// </summary>
        public void SetPrice()
        {
            Price.Add("BasicTower", 100);
            Price.Add("SpawnerBuidling", 100);
        }
    }

}


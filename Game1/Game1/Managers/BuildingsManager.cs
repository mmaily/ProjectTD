using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using Game1.GameElements.Units;
using Game1.GameElements.Units.Buildings;
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
        public event UnitLeaveRangeHandler UnitLeaveRange;
        public delegate void UnitLeaveRangeHandler(BuildingsManager bd, UnitRangeEventArgs arg);
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
        /// Liste de tous les bâtiments
        /// </summary>
        public List<Building> BuildingsList { get; set; }
        /// <summary>
        /// Liste de construction en attente pour le prochain update
        /// </summary>
        public List<Building> WaitingForConstruction { get; set; }
        public Boolean BuildingsWaiting;
        //public CombinedGeometry coveredArea;
        public GeometryGroup coveredArea;

        /// <summary>
        /// Constructeur
        /// </summary>
        public BuildingsManager()
        {
           
            BuildingsList = new List<Building>();
            coveredArea = new GeometryGroup();
            WaitingForConstruction = new List<Building>();
            BuildingsWaiting = false;
            // On instancie l'objet GameTime
        }

        /// <summary>
        /// Récupération de l'instance du gestionnaire d'interface
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
            //Check si les unités pénétre dans la surface-union
            foreach (Unit unit in UnitsManager.GetInstance().mobs)
            {      
                if (coveredArea.FillContains(new System.Windows.Point((int)unit.Position.X, (int)unit.Position.Y)))
                    {
                    if (!unit.isInRange)
                    {
                        unit.isInRange = true;
                        UnitRangeEventArgs arg = new UnitRangeEventArgs(unit);
                        UnitInRange(this, arg);
                    }

                }
                else
                {
                    if (unit.isInRange)
                    {
                        unit.isInRange = false;
                        UnitRangeEventArgs arg = new UnitRangeEventArgs(unit);
                        UnitLeaveRange(this, arg);
                    }
                }
            }
            //Update le temps de jeu écoule
            this.gameTime = _gameTime;
            if (BuildingsWaiting)
            {
                //Ajoute les building en attente de construction à la liste des buildings construit
                UpdateBuildingList();
            }
            //Apelle les bâtiments à faire leur actions respectives (si il y a des buildings)
             BuildingDuty?.Invoke();
            
        }

        /// <summary>
        /// Affichage des bâtiments
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            Map map = MapManager.GetInstance().map;
            // Pour chaque bâtiment
            foreach (Building building in BuildingsList)
            {
                _spriteBatch.Draw(CustomContentManager.GetInstance().Textures[building.name], 
                                new Vector2(building.Tile.column * map.tileSize, building.Tile.line * map.tileSize),
                                Microsoft.Xna.Framework.Color.White);
            }

        }
        /// <summary>
        /// Ajoute une cercle géométrique pour les range des tours pour définir la surface-union
        /// </summary>
        /// <param name="NewCircle"></param>
        public void AddToArea(EllipseGeometry newCircle)
        {
            coveredArea.Children.Add(newCircle);
        }
        public void AddBuildingToQueue(Tile tile, String request)
        {   

            if (tile!=null && tile.TileType == Tile.TileTypeEnum.Free)
            {
                //Ajoute les buildings en attente d'être construit Si le terrain est libre
                
                switch(request){
                    case "BasicTower":
                        WaitingForConstruction.Add(new BasicTower(tile));
                        break;
                }
                tile.TileType = Tile.TileTypeEnum.Blocked;
                BuildingsWaiting = true;
            }
        }
        public void UpdateBuildingList()
        {
            BuildingsList.AddRange(WaitingForConstruction);
            BuildingsWaiting = false;
        }
    }
}


using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using Game1.GameElements.Units;
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
        //Définiton de l'event "une unité pénétre dans la surface-union" (constituée de l'union de toutes les range)
        #region
        public event UnitInrangeHandler UnitInRange;
        public delegate void UnitInrangeHandler(BuildingsManager bd, UnitInRangeEventArgs arg);
        public EventArgs e = null;
        public class UnitInRangeEventArgs : EventArgs
        {
            public UnitInRangeEventArgs(Unit iUnit)
            { unit = iUnit; }
            public Unit unit { get; set; }
        }
        #endregion
        // Instance du gestionnaire de bâtiments
        private static BuildingsManager instance;

        /// <summary>
        /// Liste de tous les bâtiments
        /// </summary>
        public List<Building> BuildingsList { get; set; }
        //public CombinedGeometry coveredArea;
        public GeometryGroup coveredArea;

        /// <summary>
        /// Constructeur
        /// </summary>
        public BuildingsManager()
        {
           
            BuildingsList = new List<Building>();
            coveredArea = new GeometryGroup();
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
                        UnitInRangeEventArgs arg = new UnitInRangeEventArgs(unit);
                    UnitInRange(this, arg);
                }
            }
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

    }
}


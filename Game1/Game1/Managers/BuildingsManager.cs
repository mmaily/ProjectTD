using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using Game1.GameElements.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefenseGame.Managers
{

    /// <summary>
    /// Gestionnaire de bâtiments
    /// </summary>
    public class BuildingsManager
    {
        //Event si une unité est detectée "in range"
        //public delegate void dgEventRaiser();
        //public event EventHandler UnitInRange;
        //public virtual void OnUnitInRange(EventArgs e)
        //{
        //    UnitInRange?.Invoke(this, );
        //}

        public event UnitInrangeHandler UnitInRange;
        public delegate void UnitInrangeHandler(BuildingsManager bd, UnitInRangeEventArgs arg);
        public EventArgs e = null;
        public class UnitInRangeEventArgs : EventArgs
        {
            public UnitInRangeEventArgs(Unit iUnit)
            { unit = iUnit; }
            public Unit unit { get; set; }
        }
        // Instance du gestionnaire de bâtiments
        private static BuildingsManager instance;

        /// <summary>
        /// Liste de tous les bâtiments
        /// </summary>
        public List<Building> BuildingsList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public BuildingsManager()
        {
           
            BuildingsList = new List<Building>();
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

            // Temporaire dégueu
            // Pour toutes les tours
            //foreach (Building building in BuildingsList)
            //{
            //    // Pour toutes les unités
            //    foreach (Unit unit in UnitsManager.GetInstance().mobs)
            //    {
            //        // Si l'unité est proche
            //        if(Vector2.Distance(unit.Position, building.Position) < building.Range)
            //        {
            //            unit.Damage(building.AttackPower);
            //            break;
            //        }
            //    }
            //}
            foreach (Unit unit in UnitsManager.GetInstance().mobs)
            {
                //Détection si l'unité est "in range" COMPARAISON TEMPORAIRE
                //if(Vector2.Distance(unit.Position, MapManager.GetInstance().map.towerTile.getTilePosition())<200)
                //{
                if (Vector2.Distance(unit.Position, BuildingsList[0].Position)< 200)
                    {
                        UnitInRangeEventArgs arg = new UnitInRangeEventArgs(unit);
                    //UnitInRange?.Invoke(this,arg);
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
                _spriteBatch.Draw(CustomContentManager.GetInstance().Textures[building.name], new Vector2(building.Tile.column * map.tileSize, building.Tile.line * map.tileSize), Color.White);
            }

        }

    }
}


using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System.Collections.Generic;
using System;

namespace Game1.GameElements.Units.Buildings
{
    /// <summary>
    /// Tour de base
    /// </summary>
    public class BasicTower : Building
    {

        protected List<Unit> targetList;
        protected Unit Target;

        /// <summary>
        /// Constructeur
        /// </summary>
        public BasicTower() : base()
        {
            this.name = "BasicTower";
            this.AttackPower = 1;
            this.Range = 200;
            this.RateOfFire = 1;
            this.UnitType = UnitTypeEnum.Ground;
            this.TargetType = UnitTypeEnum.Ground;
            this.TargetNumber = 1;

            // Initialisation des cibles potentielles
            targetList = new List<Unit>();
            CreateOnRangeEventListener();
            
        }

        public BasicTower(Tile _tile) : this()
        {
            // On sauvegarde la tuile sur laquelle on se positionne
            this.Tile = _tile;

            // On sauvegarde la position
            // TODO : Attention appel récursif si on remplace le 64 par MapManager.GetInstance().map, puisque c'est de GetInstance que l'on vient ici...
            // Sûr ??? Baic tower est créée à la génération de MAP, on génére pas la map avec MapManager.GetInstance().map
            this.Position = _tile.getTilePosition() * 64 ;

            //On indique à la tuile que l'on a posé un bâtiment dessus
            _tile.building = this;
        }

        //Ecoute l'event 'OnUnitInRange" et add la cible à sa liste
        public  void CreateOnRangeEventListener()
        {
            BuildingsManager bd = BuildingsManager.GetInstance();
            bd.UnitInRange += new BuildingsManager.UnitInrangeHandler(AddTarget);
        }
        public void AddTarget(object sender, BuildingsManager.UnitInRangeEventArgs args)
        {
              
            targetList.Add(args.unit);
            ChooseTarget();
        }
        public int k;
        public void ChooseTarget()
        {
            k++;

            Target = targetList[0];
            Fire();
            UpdateTargetList();
        }
        public void Fire()
        {
            Target.Damage(AttackPower);
        }
        public void UpdateTargetList()
        {
            //foreach (Unit unit in targetList)
            //{

            //        targetList.RemoveAll(Unit => unit.Dead);
            //}
            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                if (targetList[i].Dead==true)
                    targetList.RemoveAt(i);
            }
        }

    }
}

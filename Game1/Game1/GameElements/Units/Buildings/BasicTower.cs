using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Game1.GameElements.Units.Buildings
{
    /// <summary>
    /// Tour de base
    /// </summary>
    public class BasicTower : Building
    {

        protected List<Unit> targetList;
        protected Unit Target;
        protected EllipseGeometry AreatoAdd;
        protected System.Windows.Point point;
        //public event RangeUpdatedHandler RangeUpdate;
        //public event EventHandler RangeUpdate;
        public delegate void EventHandler();
        public EventArgs e = null;
        //public class RangeUpdatedEventArgs : EventArgs
        //{
        //    public RangeUpdatedEventArgs(int newRangeRequired)
        //    { NewRange = newRangeRequired; }
        //    public int NewRange { get; set; }
        //}

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
            

            
        }

        public BasicTower(Tile _tile) : this()
        {
            // On sauvegarde la tuile sur laquelle on se positionne
            this.Tile = _tile;

            // On sauvegarde la position
            // TODO : Attention appel récursif si on remplace le 64 par MapManager.GetInstance().map, puisque c'est de GetInstance que l'on vient ici...
            // Sûr ??? Baic tower est créée à la génération de MAP, on génére pas la map avec MapManager.GetInstance().map
            this.Position = _tile.getTilePosition() * 64 ;
            point = new System.Windows.Point(this.Position.X, this.Position.Y);
            AreatoAdd = new EllipseGeometry(point, this.Range, this.Range);
            BuildingsManager.GetInstance().AddToArea(this.AreatoAdd);
            //On indique à la tuile que l'on a posé un bâtiment dessus
            _tile.building = this;

            //Créé le listener pour les events
            CreateOnRangeEventListener();
        }

        //Ecoute l'event 'OnUnitInRange" et add la cible à sa liste
        public  void CreateOnRangeEventListener()
        {
            BuildingsManager bd = BuildingsManager.GetInstance();
            bd.UnitInRange += new BuildingsManager.UnitInrangeHandler(AddTarget);
            AreatoAdd.Changed += new System.EventHandler(UpdateRangeCircle);
        }
        public void AddTarget(object sender, BuildingsManager.UnitInRangeEventArgs args)
        {
            //Get the point of the current position (we could move this part in the update Unit)
            System.Windows.Point unitPosAsPoint = new System.Windows.Point(args.unit.Position.X, args.unit.Position.Y);
            
            if (this.AreatoAdd.FillContains(unitPosAsPoint))
            {
                targetList.Add(args.unit);
                ChooseTarget();
                AreatoAdd.RadiusY = 201;
            }
        }
        public void ChooseTarget()
        {
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
        public EllipseGeometry getRangeCircle()
        {
            return this.AreatoAdd;
        }
        public void UpdateRangeCircle(object sender, EventArgs arg)
        {
        
        }
    }
}

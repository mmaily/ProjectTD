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

        protected List<Unit> targetList; //Liste des cibles
        protected Unit Target;//Cible actuelle
        protected EllipseGeometry AreatoAdd; //Cercle qu'on associe la range de la tour
        protected int AreaId; //Index pour retrouver le cercle-range dans la iste totale de la surface-union
        //Event à écouter ou faire proc
        public delegate void EventHandler();
        public EventArgs e = null;
        protected System.Windows.Point point;//Stock la position d'une unité en type Point
        private int idRemoval=-1; // Quand une cible quitte la range, on récupère son index pour actualiser la targetList

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
            this.Position = _tile.getTilePosition() * 64 ;
            //Il faut transformer le vecteur position en type Point pour définir le cercle associé à la range
            point = new System.Windows.Point(this.Position.X, this.Position.Y);
            //Ici on définit un cercle autour de la tour, qui a pour rayon la range
            AreatoAdd = new EllipseGeometry(point, this.Range, this.Range);
            //On ajoute ce cercle à l'union de la "surface-union" constitué de toutes les range
            BuildingsManager.GetInstance().AddToArea(this.AreatoAdd);
            //On récupère l'index de ce cercle dans la liste totale de la surface-union
            AreaId = BuildingsManager.GetInstance().coveredArea.Children.IndexOf(this.AreatoAdd);
            //On indique à la tuile que l'on a posé un bâtiment dessus
            _tile.building = this;

            //Créé le listener pour les events
            CreateOnRangeEventListener();
        }

        //Ecoute l'event 'OnUnitInRange" et add la cible à sa liste
        public  void CreateOnRangeEventListener()
        {
            //Event qui prévient la tour qu'une unité est dans la surface-union, dans ce cas on déclenche la 
            //méthode AddTarget(), l'argument de l'event c'est l'objet Unit concerné
            BuildingsManager bd = BuildingsManager.GetInstance();
            bd.UnitInRange += new BuildingsManager.UnitInRangeHandler(AddTarget);
            bd.UnitLeaveRange += new BuildingsManager.UnitLeaveRangeHandler(RemoveTarget);
            //Event pour actualiser le cercle associé à la range quand la range augmente/diminue
            AreatoAdd.Changed += new System.EventHandler(UpdateRangeCircle);
        }
        public void AddTarget(object sender, BuildingsManager.UnitRangeEventArgs args)
        {
            //Get the point of the current position (we could move this part in the update Unit)
            System.Windows.Point unitPosAsPoint = new System.Windows.Point(args.unit.Position.X, args.unit.Position.Y);
            
            //Quand une unité pénétre dans la surface-union, la tour check si c'est SA range qui est concernée
            if (this.AreatoAdd.FillContains(unitPosAsPoint))
            {
                //Si oui, elle aoute l'unité à sa liste de target
                targetList.Add(args.unit);
                if (targetList.Count > 10)
                {
                    Fire();
                }
                //Elle actualise sa cible actuelle
                ChooseTarget();
            }
        }
        public void RemoveTarget(object sender, BuildingsManager.UnitRangeEventArgs args)
        {
            System.Windows.Point unitPosAsPoint = new System.Windows.Point(args.unit.Position.X, args.unit.Position.Y);
            
            //Quand une unité quitte dans la surface-union, la tour check si c'est SA range qui est concernée
            if (!this.AreatoAdd.FillContains(unitPosAsPoint))
            {
                //Si oui, elle flag l'unité pour la remove de sa liste à la prochaine update
                idRemoval = targetList.IndexOf(args.unit);
                //Elle actualise sa cible actuelle
                ChooseTarget();
            }
        }
        public void ChooseTarget()
        {
            //Elle update sa liste pour virer les cible morte/hors-range
            UpdateTargetList();
            //Pour l'instant, la cible actuelle est la première de la liste de cible
            Target = targetList[0];
            //On tire sur l'unité (méthode à modifier pour inclure le cooldwon de tir etc
            Fire();

        }
        public void Fire()
        {
            //Elle inflige des dégats à sa cible actuelle
            Target.Damage(AttackPower);
        }
        //Méthode pour actualiser la liste des cibles (Remove les cibles morte/hors-range)
        public void UpdateTargetList()
        {
            
            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                //Si aucune cible n'es sortie de la range, on enlève juste les cible morte pour cet update
                if (idRemoval != -1) 
                {
                    if (targetList[i].Dead == true)
                    {
                        targetList.RemoveAt(i);
                    }
                }
                //Si une cible a été detectée en dehors de la range, on cherche la retire aussi
                else
                {
                    if (targetList[i].Dead == true|| i==idRemoval)
                    {
                        targetList.RemoveAt(i);
                        idRemoval = -1;
                    }
                }

                
                    
            }

        }
        //Récupère le cercle géométrique associé 
        public EllipseGeometry getRangeCircle()
        {
            return this.AreatoAdd;
        }
        //Méthode déclenché quand le cercle géomtrique associé à la range est modifié, actualise la surface-union
        public void UpdateRangeCircle(object sender, EventArgs arg)
        {
            BuildingsManager.GetInstance().coveredArea.Children[AreaId] = (this.AreatoAdd);
            AreaId = BuildingsManager.GetInstance().coveredArea.Children.IndexOf(this.AreatoAdd);
        }
    }
}

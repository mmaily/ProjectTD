using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using DowerTefenseGame.GameElements.Projectiles;

namespace Game1.GameElements.Units.Buildings
{
    /// <summary>
    /// Tour de base
    /// </summary>
    public class BasicTower : Building
    {

        protected List<Unit> targetList; //Liste des cibles
        protected List<Projectile> projectileList;//  Liste de ses munitions en vol
        protected Unit Target;//Cible actuelle
        protected EllipseGeometry AreatoAdd; //Cercle qu'on associe la range de la tour
        protected int AreaId; //Index pour retrouver le cercle-range dans la iste totale de la surface-union
        //Event à écouter ou faire proc
        public delegate void EventHandler();
        public EventArgs e = null;
        protected System.Windows.Point point;//Stock la position d'une unité en type Point
        private int idTargetRemoval; // Quand une cible quitte la range, on récupère son index pour actualiser la targetList
        private int idBulletRemoval; // Quand un proj touche, on récupère son index pour actualiser la BulletList


        /// <summary>
        /// Constructeur
        /// </summary>
        public BasicTower() : base()
        {
            this.name = "BasicTower";
            this.AttackPower = 1;
            this.Range = 200;
            this.RateOfFire = 0.0008; //En tir/milliseconde
            this.UnitType = UnitTypeEnum.Ground;
            this.TargetType = UnitTypeEnum.Ground;
            this.TargetNumber = 1;
            this.BulletSpeed = 5*64;

            // Initialisation des cibles potentielles
            targetList = new List<Unit>();
            //Initialisation de la liste des projectile
            projectileList = new List<Projectile>();
            
        }

        public BasicTower(Tile _tile) : this()
        {
            // On sauvegarde la tuile sur laquelle on se positionne
            this.Tile = _tile;
            //On initialise l'index pour remove les unité en dehors de la range à -1 (désactivé au début)
            this.idTargetRemoval = -1;
            this.idBulletRemoval = -1;
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
            //On récupère l'objet GameTime
            //gameTime = BuildingsManager.GetInstance().gameTime;
            //Créé le listener pour les events
            CreateOnEventListener();
        }

        //Ecoute l'event 'OnUnitInRange" et add la cible à sa liste
        public void CreateOnEventListener()
        {
            //Event qui prévient la tour qu'une unité est dans la surface-union, dans ce cas on déclenche la 
            //méthode AddTarget(), l'argument de l'event c'est l'objet Unit concerné
            BuildingsManager bd = BuildingsManager.GetInstance();
            bd.UnitInRange += new BuildingsManager.UnitInRangeHandler(AddTarget);
            bd.BuildingDuty += new BuildingsManager.BuildingDutyHandler(OnDuty);
            //Event pour actualiser le cercle associé à la range quand la range augmente/diminue
            AreatoAdd.Changed += new System.EventHandler(UpdateRangeCircle);
        }
        public void OnDuty()
        {
            //Update sa liste de target, choisi sa cible principale et tire
            ChooseTarget();
            //Update la liste des projectiles
            UpdateProjectileList();
            //Update ses projectile pour checker les collisions
            foreach (Projectile projectile in projectileList)
            {
                projectile.Update();
            }
        }
        private void CreateHitListener(Projectile projectile)
        {
            projectile.OnHit += new Projectile.HitHandler(RemoveBulletOnImpact);
        }
        public void AddTarget(object sender, BuildingsManager.UnitRangeEventArgs args)
        {
            //Get the point of the current position (we could move this part in the update Unit)
            System.Windows.Point unitPosAsPoint = new System.Windows.Point(args.unit.Position.X, args.unit.Position.Y);

            //Quand une unité pénétre dans la surface-union, la tour check si c'est SA range qui est concernée
            //Est-ce que une unité que je connais?
            if (targetList.Contains(args.unit))//Oui
            {
                //Est-ce qu'elle est sortie de ma range ?
                if (!this.AreatoAdd.FillContains(unitPosAsPoint))
                {
                    //Si oui, elle flag l'unité pour la remove de sa liste à la prochaine update
                    idTargetRemoval = targetList.IndexOf(args.unit);
                }
            }

            else//Je ne connais pas cette unité
            {
                //Est-ce qu'elle est dans ma range ?
                if (this.AreatoAdd.FillContains(unitPosAsPoint))
                {
                    //Si oui, elle aoute l'unité à sa liste de target
                    targetList.Add(args.unit);
                }
            }

        }
        private void RemoveBulletOnImpact(object sender, Projectile.OnHitEventArgs args)
        {
            idBulletRemoval = projectileList.IndexOf(args.proj);
            args.proj.OnHit -= new Projectile.HitHandler(RemoveBulletOnImpact);

            args.proj = null;
        }
        //Méthode qui appelle les tours à tier SI la liste est non-vide et SI le cooldown est ok
        public void ChooseTarget()
        {
            //Update sa liste pour voir si une cible est dispo
            //Elle update sa liste pour virer les cible morte/hors-range
            UpdateTargetList();
            if (targetList.Count > 0)
            {
                //Pour l'instant, la cible actuelle est la première de la liste de cible
                Target = targetList[0];
                //On tire sur l'unité (méthode à modifier pour inclure le cooldwon de tir etc
                Fire();
            }
        }
        public Boolean CanFire()
        {
            return BuildingsManager.GetInstance().gameTime.TotalGameTime.TotalMilliseconds > this.LastShot + 1/this.RateOfFire;
        }
        public void Fire()
        {
            //Si le cooldown est bon, on autorise le tir
            if (CanFire())
            {
                //Enregistre le temps du dernier tir en ms
                LastShot = BuildingsManager.GetInstance().gameTime.TotalGameTime.TotalMilliseconds;
                //Elle tire sur sa cible

                Projectile _proj = new SingleTargetProjectile(Target, this.AttackPower, BulletSpeed, this.Position, "BasicShot");
                projectileList.Add(_proj);
                CreateHitListener(_proj);

            }

        }
        //Méthode pour actualiser la liste des cibles (Remove les cibles morte/hors-range)
        public void UpdateTargetList()
        {
            
            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                //Si aucune cible n'es sortie de la range, on enlève juste les cible morte pour cet update
                if (idTargetRemoval == -1) 
                {
                    if (targetList[i].Dead == true)
                    {
                        targetList.RemoveAt(i);
                    }
                }
                //Si une cible a été detectée en dehors de la range, on cherche la retire aussi
                else
                {
                    if (i==idTargetRemoval)
                    {
                        targetList.RemoveAt(i);
                        idTargetRemoval = -1;
                    }
                }

                
                    
            }

        }
        public void UpdateProjectileList()
        {
            for (int i = projectileList.Count - 1; i >= 0; i--)
            {
                //Si aucune cible n'es sortie de la range, on enlève juste les cible morte pour cet update
 
                    if (i == idBulletRemoval)
                    {
                        projectileList.RemoveAt(i);
                        idBulletRemoval = -1;
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
        public List<Projectile> GetProjectileList()
        {
            return this.projectileList;
        }
    }
}

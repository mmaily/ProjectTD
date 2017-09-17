﻿using DowerTefenseGame.GameElements;
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

        protected List<Projectile> projectileList;//  Liste de ses munitions en vol
        protected Unit Target;//Cible actuelle
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
            bd.BuildingDuty += new BuildingsManager.BuildingDutyHandler(OnDuty);
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
       
        private void RemoveBulletOnImpact(object sender, Projectile.OnHitEventArgs args)
        {
            idBulletRemoval = projectileList.IndexOf(args.proj);
            args.proj.OnHit -= new Projectile.HitHandler(RemoveBulletOnImpact);

            args.proj = null;
        }
        //Méthode qui appelle les tours à tier SI la liste est non-vide et SI le cooldown est ok
        public Unit ChooseTarget()
        {
            if (Target == null || Target.Dead || Vector2.Distance(this.Position, Target.Position)>this.Range){
                return UnitsManager.GetInstance().GetFirstUnit();
            }
            else
            {
                return Target;
            }
           
        }
        public Boolean CanFire()
        {
            return BuildingsManager.GetInstance().gameTime.TotalGameTime.TotalMilliseconds > this.LastShot + 1/this.RateOfFire;
        }
        public void Fire()
        {
        if (CanFire())
        {
                //Si le cooldown est bon, elle s'active, sinon c'est déjà fini pour elle

                Target = ChooseTarget();
                Fire();

            //Enregistre le temps du dernier tir en ms
            LastShot = BuildingsManager.GetInstance().gameTime.TotalGameTime.TotalMilliseconds;
                //Elle tire sur sa cible

                Projectile _proj = new SingleTargetProjectile(Target, this.AttackPower, BulletSpeed, this.Position, "BasicShot");
                projectileList.Add(_proj);
                CreateHitListener(_proj);


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

        public List<Projectile> GetProjectileList()
        {
            return this.projectileList;
        }
    }
}

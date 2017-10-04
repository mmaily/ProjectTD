using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using DowerTefenseGame.GameElements.Projectiles;
using DowerTefenseGame.Units;
using LibrairieTropBien.GUI;
using DowerTefenseGame.Screens;
using System.Runtime.Serialization;

namespace DowerTefenseGame.GameElements.Units.Buildings.DefenseBuildings
{
    [Serializable()]
    public class Tower : Building, ISerializable
    {

        protected List<Projectile> projectileList;//  Liste de ses munitions en vol
        protected Entity target;//Cible actuelle
        private int idTargetRemoval; // Quand une cible quitte la range, on récupère son index pour actualiser la targetList
        private int idBulletRemoval; // Quand un proj touche, on récupère son index pour actualiser la BulletList
        protected String projectileName;
        public enum NameEnum
        {
            BasicTower, // Tour de base
            RapidFireTower,// Tour d'essaie (tir rapide, courte portée)
        }

        /// <summary>
        /// Constructeur, préciser AttackPower,Range,RateOfFire,UnitType,TargetType,BulletSpeed
        /// </summary>
        public Tower() : base()
        {
            this.AttackPower = 1;
            this.Range = 200;
            this.RateOfFire = 0.0008; //En tir/milliseconde
            this.UnitType = UnitTypeEnum.Ground;
            this.TargetType = UnitTypeEnum.Ground;
            this.TargetNumber = 1;
            this.BulletSpeed = 5 * 64;


            //Initialisation de la liste des projectile
            projectileList = new List<Projectile>();

            //On initialise l'index pour remove les unité en dehors de la range à -1 (désactivé au début)
            this.idTargetRemoval = -1;
            this.idBulletRemoval = -1;
        }
        

        /// <summary>
        /// Constructeur avec tuile de position
        /// </summary>
        /// <param name="_tile">Tuile de position</param>
        public Tower(Tile _tile) : this()
        {
            this.SetTile(_tile);

        }

        /// <summary>
        /// Réglage de la tuile de position
        /// </summary>
        /// <param name="_tile">Tuile</param>
        public override void SetTile(Tile _tile)
        {
            base.SetTile(_tile);
            // En plus de la méthode de base, on ajoute ce batiment à la liste des 
            BuildingsManager.GetInstance().DefenseBuildingsList.Add(this);
        }

        public override void OnDuty()
        {
            base.OnDuty();
            //Update sa liste de target, choisi sa cible principale et tire
            Fire();
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
        public Entity ChooseTarget()
        {

            if (this.target == null || this.target.Dead || Vector2.Distance(this.Position, this.target.Position) > this.Range)
            {
                target = null;
                foreach (Entity unit in UnitsManager.GetInstance().GetSortedUnitList())
                {
                    if (Vector2.Distance(this.Position, unit.Position) < this.Range)
                        target = unit;
                }
            }


            return target;
        }
        public Boolean CanFire()
        {
            return BuildingsManager.GetInstance().gameTime.TotalGameTime.TotalMilliseconds > this.LastShot + 1 / this.RateOfFire;
        }
        public void Fire()
        {
            if (CanFire())
            {
                //Si le cooldown est bon, elle s'active, sinon c'est déjà fini pour elle

                target = ChooseTarget();

                if (target != null)
                {


                    //Enregistre le temps du dernier tir en ms
                    LastShot = BuildingsManager.GetInstance().gameTime.TotalGameTime.TotalMilliseconds;
                    //Elle tire sur sa cible

                    Projectile _proj = new SingleTargetProjectile(target, this.AttackPower, BulletSpeed, this.Position, this.projectileName);
                    projectileList.Add(_proj);
                    CreateHitListener(_proj);
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

        public List<Projectile> GetProjectileList()
        {
            return this.projectileList;
        }
        public override void SetInfoPopUp(InfoPopUp info)
        {
            info.setText("Damage : " + AttackPower+ Environment.NewLine+"Range : " + this.Range);
        }
        public override Building DeepCopy()
        {
            Building other = (Tower)this.MemberwiseClone();
            return other;
        }

    }
}

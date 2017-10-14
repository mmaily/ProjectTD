using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DowerTefense.Commons.GameElements.Projectiles;
using DowerTefense.Commons.Units;
using System.Runtime.Serialization;
using LibrairieTropBien.GUI;
using DowerTefense.Commons.Managers;

namespace DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings
{
    [Serializable()]
    public class Tower : Building, ISerializable
    {

        public List<Projectile> projectileList;//  Liste de ses munitions en vol
        protected Entity target;//Cible actuelle
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
            this.idBulletRemoval = -1;
        }
        

        /// <summary>
        /// Constructeur avec tuile de position
        /// </summary>
        /// <param name="_tile">Tuile de position</param>
        public Tower(Tile _tile, Map map) : this()
        {
            this.SetTile(_tile, map);

        }

        /// <summary>
        /// Réglage de la tuile de position
        /// </summary>
        /// <param name="_tile">Tuile</param>
        public override void SetTile(Tile _tile, Map map)
        {
            base.SetTile(_tile, map);
        }
        #region ===Ancienne méthode OnDuty();===
        //public override void OnDuty()
        //{
        //    base.OnDuty();
        //    //Update sa liste de target, choisi sa cible principale et tire
        //    Fire();
        //    //Update la liste des projectiles
        //    UpdateProjectileList();
        //    //Update ses projectile pour checker les collisions
        //    foreach (Projectile projectile in projectileList)
        //    {
        //        projectile.Update();
        //    }
        //}
        #endregion
        public void Update(GameTime _gameTime, List<Unit> mobs)
        {
            this.gameTime = _gameTime;
            //Update sa liste de target, choisi sa cible principale et tire
            Fire(mobs);
            //Update la liste des projectiles
            UpdateProjectileList();
            //Update ses projectile pour checker les collisions

            //foreach (Projectile projectile in projectileList)
            //{
            //    projectile.Update(gameTime);
                
            //}
        }
        private void CreateHitListener(Projectile projectile)
        {
            //projectile.OnHit += new Projectile.HitHandler(RemoveBulletOnImpact);
        }
        private void RemoveBulletOnImpact(object sender, Projectile.OnHitEventArgs args)
        {
            //TODO : Prévenir les clients quand un projectile disparait (à voir)
            idBulletRemoval = projectileList.IndexOf(args.proj);
            args.proj.OnHit -= new Projectile.HitHandler(RemoveBulletOnImpact);
            args.proj = null;
        }
        //Méthode qui appelle les tours à tier SI la liste est non-vide et SI le cooldown est ok
        public Entity ChooseTarget(List<Unit> mobs)
        {

            if (this.target == null || this.target.Dead || Vector2.Distance(this.Position, this.target.Position) > this.Range)
            {
                target = null;
                //TODO : Faire une liste intermédiaire selon le typede focus
                foreach (Entity unit in UnitEngine.GetSortedUnitList(mobs))
                {
                    if (Vector2.Distance(this.Position, unit.Position) < this.Range)
                        target = unit;
                }
            }


            return target;
        }
        public Boolean CanFire()
        {
            return gameTime.TotalGameTime.TotalMilliseconds > this.LastShot + 1 / this.RateOfFire;
        }
        public void Fire(List<Unit> mobs)
        {
            if (CanFire())
            {
                //Si le cooldown est bon, elle s'active, sinon c'est déjà fini pour elle

                target = ChooseTarget(mobs);

                if (target != null)
                {


                    //Enregistre le temps du dernier tir en ms
                    LastShot = gameTime.TotalGameTime.TotalMilliseconds;
                    //Elle tire sur sa cible

                    Projectile _proj = new SingleTargetProjectile(target, this.AttackPower, BulletSpeed, this.Position, this.projectileName);
                    this.projectileList.Add(_proj);
                    CreateHitListener(_proj);
                }

            }
        }
        public void UpdateProjectileList()
        {
            for (int i = projectileList.Count - 1; i >= 0; i--)
            {
                projectileList[i].Update(gameTime);
                if (!projectileList[i].Exists)
                {
                    projectileList.RemoveAt(i);
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

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

        public List<Projectile> ProjectileList { get; protected set; }//  Liste de ses munitions en vol
        protected Entity target;//Cible actuelle
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
            ProjectileList = new List<Projectile>();

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

        /// <summary>
        /// Mise à jour de la tour et de tous ses projectiles
        /// </summary>
        /// <param name="_gameTime"></param>
        /// <param name="mobs"></param>
        public void Update(GameTime _gameTime, List<Unit> mobs)
        {
            this.gameTime = _gameTime;
            //Update sa liste de target, choisi sa cible principale et tire
            Fire(mobs);
            //Update la liste des projectiles
            // Mise à jour de chaque projectile
            this.ProjectileList.ForEach(proj => proj.Update(gameTime));
            // Suppression de tous les projectiles disparus
            this.ProjectileList.RemoveAll(deadProj => !deadProj.Exists);
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

        /// <summary>
        /// Vérification du cooldown de la tour
        /// </summary>
        /// <returns></returns>
        public Boolean CanFire()
        {
            return gameTime.TotalGameTime.TotalMilliseconds > this.LastShot + 1 / this.RateOfFire;
        }

        /// <summary>
        /// Lancement d'un projectile
        /// </summary>
        /// <param name="mobs"></param>
        public void Fire(List<Unit> mobs)
        {
            // Si le cooldown est bon, elle s'active, sinon c'est déjà fini pour elle
            if (CanFire())
            {
                // Récupération d'une cible
                target = ChooseTarget(mobs);

                if (target != null)
                {
                    // Enregistrement le temps du dernier tir en ms
                    LastShot = gameTime.TotalGameTime.TotalMilliseconds;

                    // Elle tire sur sa cible
                    Projectile _proj = new SingleTargetProjectile(target, this.AttackPower, BulletSpeed, this.Position, this.projectileName);
                    // Ajout à la liste des projectiles actuels
                    this.ProjectileList.Add(_proj);
                }
            }
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

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
    public class Tower : Building
    {

        [NonSerialized]
        protected Entity target;//Cible actuelle
        protected String projectileName;
        //Coefficient de leveling
        protected double rangeCoeff;
        protected double fireRateCoeff;
        protected double dmgCoeff;
        //Prix du leveling et coeff d'augmentation
        protected int rangePrice;
        protected double rangePriceCoeff;
        protected int fireRatePrice;
        protected double fireRatePriceCoeff;
        protected int dmgPrice;
        protected double dmgPriceCoeff;

        public List<Projectile> ProjectileList { get; protected set; }//  Liste de ses munitions en vol

        public enum NameEnum
        {
            BasicTower, // Tour de base
            RapidFireTower,// Tour d'essaie (tir rapide, courte portée)
        }
        public FocusEnum focus;
        public enum FocusEnum
        {
            Far,
            Close,
            Strong,
            Weak
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
            this.focus = FocusEnum.Far;

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
        /// <param name="_mobs"></param>
        public void Update(GameTime _gameTime, ref List<Unit> _mobs)
        {
            if (CanFire(_gameTime))
            {
                //Mise à jour de la liste de target, choisi la cible principale et tire
                Fire(ref _mobs, _gameTime);
            }
            //Update la liste des projectiles
            // Mise à jour de chaque projectile
            this.ProjectileList.ForEach(proj => proj.Update(_gameTime));
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
                foreach (Entity unit in UnitEngine.GetSortedUnitList(mobs,focus))
                {
                    if (Vector2.Distance(this.Position, unit.Position) < this.Range)
                        target = unit;
                }
            }


            return target;
        }

        /// <summary>
        /// Lancement d'un projectile
        /// </summary>
        /// <param name="_mobs"></param>
        public void Fire(ref List<Unit> _mobs, GameTime _gameTime)
        {
            // Si le cooldown est bon, elle s'active, sinon c'est déjà fini pour elle
            // Récupération d'une cible
            target = ChooseTarget(_mobs);

            if (target != null)
            {
                // Enregistrement le temps du dernier tir en ms
                LastShot = _gameTime.TotalGameTime.TotalMilliseconds;

                // Elle tire sur sa cible
                Projectile _proj = new SingleTargetProjectile(target, this.AttackPower, BulletSpeed, this.Position, this.projectileName);
                // Ajout à la liste des projectiles actuels
                this.ProjectileList.Add(_proj);
            }
        }

        /// <summary>
        /// Vérification du cooldown de la tour
        /// </summary>
        /// <returns></returns>
        public Boolean CanFire(GameTime _gameTime)
        {
            return _gameTime.TotalGameTime.TotalMilliseconds > this.LastShot + 1 / this.RateOfFire;
        }


        public override void SetInfoPopUp(InfoPopUp info)
        {
            info.setText("Damage : " + AttackPower + Environment.NewLine + "Range : " + this.Range);
        }

        public override Building DeepCopy()
        {
            Building other = (Tower)this.MemberwiseClone();
            return other;
        }
        public int FRLvlUp(int Gold)
        {
            int lostGold = 0;
            if (fireRatePrice <= Gold)
            {
                this.RateOfFire += (int)Math.Ceiling(this.BaseRateOfFire * fireRateCoeff);
                lostGold = fireRatePrice;
                //Calcule le nouveau coût du lvl up
                fireRatePrice *= (int)Math.Ceiling(1 + fireRatePriceCoeff);
            }
            //Si les gold retournés sont nuls, on sait que le joueur n'avait pas les sous pour construire
            //TODO : Message d'avertissement si pas assez de sous ? valeur -1 si max de lvl up atteint ?
            return lostGold;
        }
        public int DmgLvlUp(int Gold)
        {
            int lostGold = 0;
            if (dmgPrice <= Gold)
            {
                this.AttackPower += (int)Math.Ceiling(this.BaseAttackPower * dmgCoeff);
                lostGold = dmgPrice;
                //Calcule le nouveau coût du lvl up
                fireRatePrice *= (int)Math.Ceiling(1 + dmgPriceCoeff);
            }
            //Si les gold retournés sont nuls, on sait que le joueur n'avait pas les sous pour construire
            //TODO : Message d'avertissement si pas assez de sous ? valeur -1 si max de lvl up atteint ?
            return lostGold;
        }
        public int RangeLvlUp(int Gold)
        {
            int lostGold = 0;
            if(rangePrice<= Gold)
            {
                this.Range += (int)Math.Ceiling(this.BaseRange * rangeCoeff);
                lostGold = dmgPrice;
                //Calcule le nouveau coût du lvl up
                rangePrice *= (int)Math.Ceiling(1 + rangePriceCoeff);
            }
            //Si les gold retournés sont nuls, on sait que le joueur n'avait pas les sous pour construire
            //TODO : Message d'avertissement si pas assez de sous ? valeur -1 si max de lvl up atteint ?
            return lostGold;
        }



    }
}

using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using DowerTefenseGame.GameElements.Projectiles;
using DowerTefenseGame.Units;

namespace DowerTefenseGame.GameElements.Units.Buildings.DefenseBuildings
{
    class RapidFireTower :Tower
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public RapidFireTower() : base()
        {
            this.name = "RapidFireTower";
            this.AttackPower = 3;
            this.Range = 100;
            this.RateOfFire = 0.0016; //En tir/milliseconde
            this.UnitType = UnitTypeEnum.Ground;
            this.TargetType = UnitTypeEnum.Ground;
            this.TargetNumber = 1;
            this.BulletSpeed = 5 * 64;
            this.projectileName = "BasicShot";
            this.Cost = 20;
        }
    }
}

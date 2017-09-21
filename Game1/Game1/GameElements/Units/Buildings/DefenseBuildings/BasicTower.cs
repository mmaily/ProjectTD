using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using DowerTefenseGame.GameElements.Projectiles;

namespace DowerTefenseGame.GameElements.Units.Buildings.DefenseBuildings
{
    /// <summary>
    /// Tour de base
    /// </summary>
    public class BasicTower : Towers
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public BasicTower() : base()
        {
            this.name = "BasicTower";
            this.AttackPower = 5;
            this.Range = 200;
            this.RateOfFire = 0.0008; //En tir/milliseconde
            this.UnitType = UnitTypeEnum.Ground;
            this.TargetType = UnitTypeEnum.Ground;
            this.TargetNumber = 1;
            this.BulletSpeed = 5*64;
            this.projectileName = "BasicShot";
            this.Cost = BuildingsManager.GetInstance().Price[this.name];
        }

       
    }
}

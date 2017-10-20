using DowerTefense.Commons.GameElements;
using DowerTefense.Commons.GameElements.Units;
using DowerTefense.Commons.Managers;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DowerTefense.Commons.GameElements.Projectiles;
using DowerTefense.Commons.Units;
using System.Runtime.Serialization;

namespace DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings
{
    [Serializable()]
    class RapidFireTower :Tower
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public RapidFireTower() : base()
        {
            this.Name = "RapidFireTower";
            this.AttackPower = 3;
            this.BaseAttackPower = this.AttackPower;
            this.Range = 100;
            this.BaseRange = this.Range;
            this.RateOfFire = 0.0016; //En tir/milliseconde
            this.BaseRateOfFire = this.RateOfFire;
            this.UnitType = UnitTypeEnum.Ground;
            this.TargetType = UnitTypeEnum.Ground;
            this.TargetNumber = 1;
            this.BulletSpeed = 5 * 64;
            this.projectileName = "BasicShot";
            this.Cost = 70;
            //Leveling
            this.rangeCoeff = 0.08; 
            this.rangePrice = 70;//Premiere upgrade 
            this.rangePriceCoeff = 0.80;

            this.fireRateCoeff = 0.2;
            this.fireRatePrice = 90;
            this.fireRatePriceCoeff = 0.55;

            this.dmgCoeff = 0.10;
            this.dmgPrice = 80;
            this.dmgPriceCoeff = 0.6;
        }
    }
}

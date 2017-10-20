using DowerTefense.Commons.GameElements;
using DowerTefense.Commons.GameElements.Units;
using DowerTefense.Commons.Managers;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DowerTefense.Commons.GameElements.Projectiles;
using LibrairieTropBien.SerializableObjects;

namespace DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings
{
    [Serializable()]
    /// <summary>
    /// Tour de base
    /// </summary>
    public class BasicTower : Tower
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public BasicTower() : base()
        {
            this.Name = "BasicTower";
            this.AttackPower = 5;
            this.BaseAttackPower = this.AttackPower;
            this.Range = 200;
            this.BaseRange = this.Range;
            this.RateOfFire = 0.0008; //En tir/milliseconde
            this.BaseRateOfFire = RateOfFire;
            this.UnitType = UnitTypeEnum.Ground;
            this.TargetType = UnitTypeEnum.Ground;
            this.TargetNumber = 1;
            this.BulletSpeed = 5 * 64;
            this.projectileName = "BasicShot";
            this.Cost = 50;
            //Leveling
            this.rangeCoeff = 0.10; //=10%
            this.rangePrice = 90;//Premiere upgrade à 90g
            this.rangePriceCoeff = 0.50;//Chaque lvling coute 10% de plus

            this.fireRateCoeff = 0.10;
            this.fireRatePrice = 90;
            this.fireRatePriceCoeff = 0.50;

            this.dmgCoeff = 0.10;
            this.dmgPrice = 90;
            this.dmgPriceCoeff = 0.5;
        }

    }
}

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
    class RapidFireTower :Tower,ISerializable
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

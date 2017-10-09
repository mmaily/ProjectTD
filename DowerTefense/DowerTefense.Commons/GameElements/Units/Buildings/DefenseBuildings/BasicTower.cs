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
    public class BasicTower : Tower, ISerializable
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
            this.Cost = 10;
        }

       
    }
}

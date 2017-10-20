﻿using DowerTefense.Commons.GameElements.Units.Unités;
using System;
using System.Runtime.Serialization;

namespace DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings
{
    [Serializable()]
    class ToughSpawner : SpawnerBuilding
    {
        public ToughSpawner()
        {

        }
        public override void setName()
        {
            this.Name = "ToughSpawner";
            this.UnitName = "ToughUnit";
            this.Unit = new ToughUnit();
            //leveling SpawnRate
            this.SpawnRateCoeff = 0.12;
            this.SpawnRatePrice = 250;
            this.SpawnRatePriceCoeff = 0.1;
            //Levling du nombre de spawn Instant
            this.BaseNumberSpawn = this.NumberSpawn;
            this.NumberSpawnPrice = 1000;
            this.NumberSpawnCoeff = 2;
            this.NumberSpawnPriceCoeff = 2;
            //Leveling de la vitesse des unités
            this.BaseUnitSpeed = this.Unit.Speed;
            this.UnitSpeedPrice = 50;
            this.UnitSpeedCoeff = 0.33;
            this.UnitSpeedPriceCoeff = 2;
            //Leveling de la vie de base des unités
            this.BaseUnitHealth = this.Unit.BaseMaxHealthPoints;
            this.UnitHealthPrice = 50;
            this.UnitHealthCoeff = 1.7;
            this.UnitHealthPriceCoeff = 1.8;
        }
    }
}

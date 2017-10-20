using DowerTefense.Commons.GameElements.Units.Unités;
using System;
using System.Runtime.Serialization;

namespace DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings
{
    [Serializable()]
    class BasicSpawner : SpawnerBuilding
    {
        public BasicSpawner()
        {

        }
        public override void setName()
        {
            this.Name = "BasicSpawner";
            this.UnitName = "BasicUnit";
            this.Unit = new BasicUnit();
            this.SpawnRate = 0.4;
            this.NumberSpawn = 1;
            //leveling
            this.BaseSpawnRate = this.SpawnRate;
            this.SpawnRateCoeff = 0.1;
            this.SpawnRatePrice = 200;
            this.SpawnRatePriceCoeff = 2;
            //Levling du nombre de spawn Instant
            this.BaseNumberSpawn = this.NumberSpawn;
            this.NumberSpawnPrice = 1000;
            this.NumberSpawnCoeff = 2;
            this.NumberSpawnPriceCoeff = 2;
            //Leveling de la vitesse des unités
            this.BaseUnitSpeed = this.Unit.Speed;
            this.UnitSpeedPrice = 50;
            this.UnitSpeedCoeff = 0.4;
            this.UnitSpeedPriceCoeff = 1.5;
            //Leveling de la vie de base des unités
            this.BaseUnitHealth = this.Unit.BaseMaxHealthPoints;
            this.UnitHealthPrice = 50;
            this.UnitHealthCoeff = 1.7;
            this.UnitHealthPriceCoeff = 1.5;
        }
    }
}

using DowerTefense.Commons.GameElements.Units.Unités;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings
{
    [Serializable()]
    class FastSpawner : SpawnerBuilding
    {
        public FastSpawner()
        {

        }
        public override void setName()
        {
            this.Name = "FastSpawner";
            this.UnitName = "FastUnit";
            this.Unit = new FastUnit();
            this.NumberSpawn = 1;
            //Leveling du Spawnrate
            this.BaseSpawnRate = this.SpawnRate;
            this.SpawnRateCoeff = 0.55;
            this.SpawnRatePrice = 150;
            this.SpawnRatePriceCoeff =1.3;
            //Levling du nombre de spawn Instant
            this.BaseNumberSpawn = this.NumberSpawn;
            this.NumberSpawnPrice = 1000;
            this.NumberSpawnCoeff = 2;
            this.NumberSpawnPriceCoeff = 2;
            //Leveling de la vitesse des unités
            this.BaseUnitSpeed = this.Unit.Speed;
            this.UnitSpeedPrice = 45;
            this.UnitSpeedCoeff = 0.5;
            this.UnitSpeedPriceCoeff = 2;
            //Leveling de la vie de base des unités
            this.BaseUnitHealth = this.Unit.BaseMaxHealthPoints;
            this.UnitHealthPrice = 50;
            this.UnitHealthCoeff = 1.2;
            this.UnitHealthPriceCoeff = 1.5;
        }
    }
}

using DowerTefense.Commons.GameElements.Units.Unités;
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
            this.NbreOfInstantSpawnPrice = 1000;
            //leveling
            this.SpawnRateCoeff = 0.12;
            this.SpawnRatePrice = 250;
            this.SpawnRatePriceCoeff = 0.1;
        }
    }
}

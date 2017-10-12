using DowerTefense.Commons.GameElements.Units.Unités;
using System;
using System.Runtime.Serialization;

namespace DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings
{
    [Serializable()]
    class ToughSpawner : SpawnerBuilding, ISerializable
    {
        public ToughSpawner()
        {

        }
        public override void setName()
        {
            this.name = "ToughSpawner";
            this.UnitName = "ToughUnit";
            this.Unit = new ToughUnit();
        }
    }
}

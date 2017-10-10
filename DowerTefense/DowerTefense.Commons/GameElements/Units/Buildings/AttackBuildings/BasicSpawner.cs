using System;
using System.Runtime.Serialization;

namespace DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings
{
    [Serializable()]
    class BasicSpawner : SpawnerBuilding, ISerializable
    {
        public BasicSpawner()
        {

        }
        public override void setName()
        {
            this.name = "BasicSpawner";
            this.UnitName = "Basic Unit";
            this.Unit = new Unit();
        }
    }
}

using DowerTefense.Commons.GameElements.Units.Unités;
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
            this.Name = "BasicSpawner";
            this.UnitName = "BasicUnit";
            this.Unit = new BasicUnit();
        }
    }
}

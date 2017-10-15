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
        }
    }
}

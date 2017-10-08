using DowerTefense.Commons.Units.Buildings;
using DowerTefenseCommons.Managers;
using DowerTefenseCommons.Units.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefenseCommons.GameElements.Units.Buildings.AttackBuildings
{
    class BasicSpawner : SpawnerBuilding
    {
        public BasicSpawner()
        {

        }
        public override void setName()
        {
            this.name = "BasicSpawner";
            this.UnitName = "Basic Unit";
        }
    }
}

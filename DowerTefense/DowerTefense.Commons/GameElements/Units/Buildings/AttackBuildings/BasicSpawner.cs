namespace DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings
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
            this.Unit = new Unit();
        }
    }
}

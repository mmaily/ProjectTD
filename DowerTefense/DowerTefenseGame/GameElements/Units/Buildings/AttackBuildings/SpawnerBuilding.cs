using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using DowerTefenseGame.GameElements.Projectiles;
using LibrairieTropBien.GUI;
using System.Runtime.Serialization;

namespace DowerTefenseGame.Units.Buildings
{
    [Serializable()]
    public class SpawnerBuilding : Building, ISerializable
    {
        protected double SpawnRate; //Number of mobs/second
        protected double lastSpawn; // Time of the last spawned mob
        public Boolean locked=false; // Le batiment ne spawn qui s'il est lock
        public Boolean powered; // Le bâtiment ne spawn que s'il est powered
        public int PowerNeeded { get; protected set; } // Energie requise par le bâtiment
        public int NbreOfInstantSpawn;//Nombre de Spawn simultané d'un batiment, peut être amélioré
        protected Unit Unit;// Type d'unité qu'il spawn
        protected String UnitName; 
        public MapManager mapManager = MapManager.GetInstance();
        public enum NameEnum
        {
            BasicSpawner, // Spawner d'unité de base
        }


        public SpawnerBuilding() : base()
        {
            this.SpawnRate = 0.2;
            this.PowerNeeded = 1;
            this.Cost = 100;
            setName();
            this.NbreOfInstantSpawn = 1;
        }
        public override void OnDuty()
        {
            base.OnDuty();
            if (CanSpawn())
            {
                SpawnUnit();
            }
        }

        public Boolean CanSpawn()
        {
            Boolean canSpawn = false;
            if(this.locked && this.powered && BuildingsManager.GetInstance().gameTime.TotalGameTime.TotalMilliseconds>lastSpawn+(1/SpawnRate)*1000)
            {
                canSpawn = true;
            }
            return canSpawn;
        }
        public void SpawnUnit()
        {
            for(int i = 0; i < NbreOfInstantSpawn; i++)
            {
                this.Unit = (Unit)Activator.CreateInstance(Type.GetType("DowerTefenseGame.GameElements.Units." + UnitsManager.GetInstance().UnitSpawned[this.name]));
                // On définit sa position comme étant celle du spawn
                Unit.UpdatePosition(mapManager.CurrentMap.Spawns[0].getTilePosition() * mapManager.CurrentMap.tileSize);
                // On définit sa destination comme étant la tuile suivante
                Unit.DestinationTile = mapManager.CurrentMap.Spawns[0].NextTile;
                // On l'ajoute à la liste des mobs
                UnitsManager.GetInstance().mobs.Add(Unit);
                lastSpawn = (int)Math.Floor(BuildingsManager.GetInstance().gameTime.TotalGameTime.TotalMilliseconds);
            }

        }
        public void TurnPower()
        {
            powered = !powered;
        }
        public virtual void setName()
        {

        }
        public void Lock()
        {
            this.locked = true;
            this.CreateOnEventListener();
        }
        public override Building DeepCopy()
        {
            Building other = (SpawnerBuilding)this.MemberwiseClone();
            return other;
        }
        public override void SetInfoPopUp(InfoPopUp _info)
        {
            _info.setText( "Unit Spawned : " + UnitName + Environment.NewLine + "Spawn Rate : " + SpawnRate+ Environment.NewLine+ "Number spawned " + NbreOfInstantSpawn);
        }
        public SpawnerBuilding(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            name = (String)info.GetValue("name", typeof(String));
            Position = (Vector2)info.GetValue("Position", typeof(Vector2));
            UnitType = (UnitTypeEnum)info.GetValue("UnitType", typeof(UnitTypeEnum));
            HealthPoints = (int)info.GetValue("HealthPoints", typeof(int));
            GoldValue = (int)info.GetValue("GoldValue", typeof(int));
            Cost = (int)info.GetValue("Cost", typeof(int));
            Level = (byte)info.GetValue("Level", typeof(byte));
            Speed = (float)info.GetValue("Speed", typeof(float));
            AttackPower = (int)info.GetValue("AttackPower", typeof(int));
            RateOfFire = (double)info.GetValue("RateOfFire", typeof(double));
            Range = (float)info.GetValue("Range", typeof(float));
            BulletSpeed = (float)info.GetValue("BulletSpeed", typeof(float));
            TargetNumber = (int)info.GetValue("TargetNumber", typeof(int));
            //On ajoute les spécificité des Spawner
            SpawnRate = (double)info.GetValue("SpawnRate", typeof(double));
            PowerNeeded = (int)info.GetValue("PowerNeeded", typeof(int));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("name", name);
            info.AddValue("Position", Position);
            info.AddValue("UnitType", UnitType);
            info.AddValue("HealthPoints", HealthPoints);
            info.AddValue("GoldValue", GoldValue);
            info.AddValue("Cost", Cost);
            info.AddValue("Level", Level);
            info.AddValue("Speed", Speed);
            info.AddValue("AttackPower", AttackPower);
            info.AddValue("RateOfFire", RateOfFire);
            info.AddValue("Range", Range);
            info.AddValue("BulletSpeed", BulletSpeed);
            info.AddValue("TargetNumber", TargetNumber);
            //On ajoute les spécificité des Spawner
            info.AddValue("SpawnRate", SpawnRate);
            info.AddValue("PowerNeeded", PowerNeeded);

        }
    }
}

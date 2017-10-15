using System;
using Microsoft.Xna.Framework;
using LibrairieTropBien.GUI;
using System.Runtime.Serialization;
using System.Collections.Generic;
using DowerTefense.Commons.Units;
using DowerTefense.Commons.GameElements.Units.Unités;

namespace DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings
{
    [Serializable()]
    public class SpawnerBuilding : Building
    {
        protected double SpawnRate; //Number of mobs/second
        protected double lastSpawn; // Time of the last spawned mob
        public Boolean locked=false; // Le batiment ne spawn qui s'il est lock
        public Boolean powered; // Le bâtiment ne spawn que s'il est powered
        public int PowerNeeded { get; protected set; } // Energie requise par le bâtiment
        public int NbreOfInstantSpawn;//Nombre de Spawn simultané d'un batiment, peut être amélioré
        protected Unit Unit;// Type d'unité qu'il spawn
        protected String UnitName;
        [NonSerialized]
        private Map map;
        private bool blankShot;

        public enum NameEnum
        {
            BasicSpawner, // Spawner d'unité de base
            ToughSpawner,//Spawner d'unité tanky
            FastSpawner //Spawner d'unité rapide

        }


        public SpawnerBuilding() : base()
        {
            this.SpawnRate = 0.2;
            this.PowerNeeded = 1;
            this.Cost = 100;
            setName();
            this.NbreOfInstantSpawn = 1;
        }
        #region ===Méthode OnDuty() obsolète===
        //public override void OnDuty()
        //{
        //    base.OnDuty();
        //    if (CanSpawn())
        //    {
        //        SpawnUnit();
        //    }
        //}
        #endregion
        public void Update(GameTime _gameTime, Map _map,ref List<Unit> mobs)
        {
            this.map = _map;
            if (CanSpawn(_gameTime))
            {
                SpawnUnit(ref mobs, _gameTime);
            }
        }
        
        /// <summary>
        /// Retourne si le spawner peut faire apparaître ou non
        /// </summary>
        /// <returns></returns>
        public Boolean CanSpawn(GameTime _gameTime)
        {
            Boolean canSpawn = false;
            //Premier tir à blanc pour éviter le chevauchement des unités
            if (this.locked && this.powered && _gameTime.TotalGameTime.TotalMilliseconds > lastSpawn + (1 / SpawnRate) * 1000 && !blankShot)
            {
                
                blankShot = true;
                lastSpawn = (int)Math.Floor(_gameTime.TotalGameTime.TotalMilliseconds);
            }
            if (this.locked && this.powered && _gameTime.TotalGameTime.TotalMilliseconds>lastSpawn+(1/SpawnRate)*1000&&blankShot)
            {
                canSpawn = true;
            }
            return canSpawn;
        }
        public void SpawnUnit(ref List<Unit> mobs, GameTime _gameTime)
        {
            for(int i = 0; i < NbreOfInstantSpawn; i++)
            {
                Unit unit= this.Unit.DeepCopy();
                // On définit sa position comme étant celle du spawn
                unit.UpdatePosition(map.Spawns[0].getTilePosition() * map.tileSize);
                // On définit sa destination comme étant la tuile suivante
                unit.DestinationTile = map.Spawns[0].NextTile;
                // On l'ajoute à la liste des mobs
                lock (mobs)
                {
                    mobs.Add(unit);
                }
                lastSpawn = (int)Math.Floor(_gameTime.TotalGameTime.TotalMilliseconds);
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
            //this.CreateOnEventListener();
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
            Name = (String)info.GetValue("name", typeof(String));
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
            info.AddValue("name", Name);
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

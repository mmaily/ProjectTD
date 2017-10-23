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
        [NonSerialized]
        private Map map;
        private bool blankShot;
        public int NumberSpawn;//Nombre de Spawn simultané d'un batiment, peut être amélioré
        public double SpawnRate; //Number of mobs/second
        public Unit Unit;// Type d'unité qu'il spawn
        public String UnitName;
        #region ===Variable liée au levelling ===
        //LvlUp Spawn Rate
        public double BaseSpawnRate;//Nombre de mobs/seconde lvl 1
        public double SpawnRateCoeff;//Coefficient d'évolution du SpawnRate pour lvlUp
        public int SpawnRatePrice;//Prix de base de l'évolution du SpawnRate
        public double SpawnRatePriceCoeff;//Evolution du prix du lvl de SpawnRate
        //LvlUp Number of instant Spawn
        public double BaseNumberSpawn;//Nombre de Spawn à la création
        public double NumberSpawnCoeff;//Coefficient d'évolution du SpawnRate pour lvlUp
        public int NumberSpawnPrice;//Prix de base de l'évolution du SpawnRate
        public double NumberSpawnPriceCoeff;//Evolution du prix du lvl de SpawnRate
        //LvlUp UnitSPeed
        public double BaseUnitSpeed; //Vitesse initiale de l'unité
        public double UnitSpeedCoeff;//Coefficient d'évolution de la vitesse de déplacement de l'unité
        public int UnitSpeedPrice;//Prix de base de l'évolution de la vitesse de déplacement de l'unité
        public double UnitSpeedPriceCoeff;//Evolution du prix du lvl de la vitesse de déplacement de l'unité
        //LvlUp UnitHealth
        public double BaseUnitHealth;//Vie initiale de l'unité en question
        public double UnitHealthCoeff;//Coefficient d'évolution de la santé max de l'unité
        public int UnitHealthPrice;//Prix de base de l'évolution de la santé max de l'unité
        public double UnitHealthPriceCoeff;//Evolution du prix du lvl de la santé max de l'unité
        #endregion
        protected double lastSpawn; // Time of the last spawned mob
        public Boolean locked=false; // Le batiment ne spawn que s'il est lock
        public Boolean powered; // Le bâtiment ne spawn que s'il est powered
        public int PowerNeeded { get; protected set; } // Energie requise par le bâtiment


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
            this.NumberSpawn = 1;
        }
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
            for(int i = 0; i < NumberSpawn; i++)
            {
                Unit unit= this.Unit.DeepCopy();
                // On définit sa position comme étant celle du spawn
                unit.UpdatePosition(map.Spawns[0].GetTilePosition() * map.tileSize);
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
        public int SwitchPower(int playerEnergy)
        {
            int consumedEnergy = 0;
            if (this.powered)
            {
                //De l'energie sera rendue au joueur et le bat est désactivé
                powered = false;
                consumedEnergy = -this.PowerNeeded;
            }
            else
            {
                if (playerEnergy >= this.PowerNeeded)
                {
                    //De l'energie sera soustraite au joueur et le bat est activé
                    powered = true;
                    consumedEnergy = this.PowerNeeded;
                }
            }
            
            return consumedEnergy;
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
            _info.setText( "Unit Spawned : " + UnitName + Environment.NewLine + "Spawn Rate : " + SpawnRate+ Environment.NewLine+ "Number spawned " + NumberSpawn);
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
        #region === leveling ===
        public int NbreOfInstantSpawnLvlUp(int gold)
        {
            int lostGold = 0;
            if (NumberSpawnPrice <= gold)
            {
                lostGold = this.NumberSpawnPrice;
                this.NumberSpawn += (int)Math.Ceiling(this.BaseNumberSpawn * NumberSpawnCoeff);
                this.NumberSpawnPrice = (int)Math.Ceiling(this.NumberSpawnPrice * this.NumberSpawnPriceCoeff);
            }
            return lostGold;
        }
        public int SpawnRateLvlUp(int gold)
        {
            {
                int lostGold = 0;
                if (SpawnRatePrice <= gold)
                {
                    lostGold = this.SpawnRatePrice;
                    this.SpawnRate += (this.BaseSpawnRate * SpawnRateCoeff);
                    this.SpawnRatePrice = (int)Math.Ceiling(this.SpawnRatePrice * this.SpawnRatePriceCoeff);
                }
                return lostGold;
            }
        }
        public int UnitSpeedLvlUp(int gold)
        {
            int lostGold = 0;
            if (UnitSpeedPrice <= gold)
            {
                lostGold = this.UnitSpeedPrice;
                this.Unit.Speed += (float)(this.BaseUnitSpeed * UnitSpeedCoeff);
                this.UnitSpeedPrice = (int)Math.Ceiling(this.UnitSpeedPrice * this.UnitSpeedPriceCoeff);
            }
            return lostGold;
        }
        public int UnitHealthLvlUp(int gold)
        {
            int lostGold = 0;
            if (UnitHealthPrice <= gold)
            {
                lostGold = this.UnitHealthPrice;
                this.Unit.MaxHealthPoints += (int)Math.Ceiling(this.BaseUnitHealth * UnitHealthCoeff);
                this.Unit.UpdateHp(MaxHealthPoints);
                this.UnitHealthPrice = (int)Math.Ceiling(this.UnitHealthPrice * this.UnitHealthPriceCoeff);
            }
            return lostGold;
        }
        #endregion
    }
}

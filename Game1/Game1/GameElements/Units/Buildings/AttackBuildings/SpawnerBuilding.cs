﻿using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using DowerTefenseGame.GameElements.Projectiles;

namespace DowerTefenseGame.Units.Buildings
{
    class SpawnerBuilding : Building
    {
        protected double SpawnRate; //Number of mobs/second
        protected double lastSpawn; // Time of the last spawned mob
        protected Boolean locked=true; // Le batiment ne spawn qui s'il est lock
        public Boolean powered = true; // Le bâtiment ne spawn que s'il est powered
        public int PowerNeeded { get; protected set; } // Energie requise par le bâtiment
        public int NbreOfInstantSpawn;//Nombre de Spawn simultané d'un batiment, peut être amélioré
        protected Unit Unit;// Type d'unité qu'il spawn
        protected MapManager mapManager = MapManager.GetInstance();
        public enum NameEnum
        {
            BasicSpawner, // Spawner d'unité de base
        }


        public SpawnerBuilding() : base()
        {
            this.SpawnRate = 0.2;
            this.PowerNeeded = 1;
            BuildingsManager.GetInstance().FreeBuildingsList.Add(this);
            this.Cost = 100;
            setName();
        }
        public override void OnDuty()
        {
            base.OnDuty();
            if (CanSpawn())
            {
                SpawnUnit();
                if(BuildingsManager.GetInstance().gameTime.TotalGameTime.Milliseconds > lastSpawn + (1 / SpawnRate) * 1000)
                {
                    TurnPower();
                }
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
    }
}

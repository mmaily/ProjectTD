using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using LibrairieTropBien.SerializableObjects;

namespace DowerTefense.Commons.GameElements.Units
{
    [Serializable()]
    
    /// <summary>
    /// Classe de base de toutes les unités, défense ou attaque
    /// </summary>
    public abstract class Entity : ISerializable
    {
        protected GameTime gameTime;
        /// <summary>
        /// Prochaine tuile de destination de cette unité
        /// </summary>
        public Tile DestinationTile { get; set; }

        /// <summary>
        /// Nom de l'unité
        public string Name { get; set; }
        /// Position de l'unité
        /// </summary>
        public SVector2 Position { get; protected set; }
        /// <summary>
        /// Type de l'unité (défaut : None)
        /// </summary>
        public UnitTypeEnum UnitType { get; protected set; }
        /// <summary>
        /// Points de vie de base de l'unité
        /// </summary>
        public int MaxHealthPoints { get; protected set; }
        /// <summary>
        /// Nombre de points de vie de l'unité (défaut : 1)
        /// </summary>
        private int healthPoints;
        public int HealthPoints
        {
            get {
                // Avec verrouillage pour les accès concurentiels
                int currentHP;
                lock (lockHealth)
                {
                    currentHP = healthPoints;
                }
                    return currentHP; }
            protected set { healthPoints = value; }
        }

        /// <summary>
        /// Gain à la destruction de l'unité (défaut : 0)
        /// </summary>
        public int GoldValue { get; protected set; }
        /// <summary>
        /// Coût de construction l'unité (défaut : 0)
        /// </summary>
        public int Cost { get; protected set; }
        /// <summary>
        /// Niveau de l'unité (défaut : 1)
        /// </summary>
        public byte Level { get; protected set; }
        /// <summary>
        /// Vitesse de déplacement en tuiles par seconde (défaut : 0)
        /// </summary>
        public float Speed { get; protected set; }
        /// <summary>
        /// Distance totale parcourue par le mob
        /// </summary>
        public double DistanceTraveled { get; set; }
        /// <summary>
        /// Dégâts infligés par l'unité (défaut : 0)
        /// </summary>
        public int AttackPower { get; protected set; }
        /// <summary>
        /// Portée des attaques de l'unité (défaut : 0)
        /// </summary>
        public float Range { get; protected set; }
        /// <summary>
        /// vitesse de déplacement du projectile tiré
        /// </summary>
        public float BulletSpeed { get; protected set; }
        /// <summary>
        /// Vitesse d'attaque de l'unité en tirs/MILLIseconde  (défaut : 1)
        /// </summary>
        public double RateOfFire { get; protected set; }
        /// <summary>
        /// Sauvegarde le temps de jeu du dernier tir
        /// </summary>
        public double LastShot { get; protected set; }
        /// <summary>
        /// Nombre de cibles visées en un seul tir (défaut : 0)
        /// </summary>
        public float TargetNumber { get; protected set; }
        /// <summary>
        /// Type de cibles visées par cette unité  (défaut : None)
        /// </summary>
        public UnitTypeEnum TargetType { get; protected set; }

        /// <summary>
        /// Drapeau permettant de marquer l'unité comme détruite
        /// </summary>
        public bool Dead { get; set; }




        /// <summary>
        /// Verrou pour sécuriser les accès à la vie de l'unité
        /// </summary>
        private Object lockHealth = new Object();


        /// <summary>
        /// Tente d'infliger des dégâtes à l'unité
        /// </summary>
        /// <param name="_damage">Dégâts à infliger</param>
        /// <returns>Vrai si les poins de vie ont été enlevés, false si elle est morte</returns>
        public bool TryDamage(float _damage)
        {
            // L'unité est vivante
            bool alive = true;
            // Verrouillage
            lock (lockHealth)
            {
                // Retrait des points de vie (arrondis)
                this.HealthPoints -= (int)_damage;
                // Si cela a tué l'unité
                if(HealthPoints <= 0)
                {
                    alive = false;
                }
            }
            //On renvoie l'état de l'unité
            return alive;
        }

        // TODO
        //Taille, Effet spécial;

        /// <summary>
        /// Contructeur de base d'une unité.
        /// </summary>
        public Entity()
        {
            Position = new Vector2();
            UnitType = UnitTypeEnum.None;
            MaxHealthPoints = 1;
            HealthPoints = 1;
            Cost = 0;
            Level = 1;
            Speed = 0;
            AttackPower = 0;
            Range = 0;
            RateOfFire = 1;
            TargetNumber = 0;
            TargetType = UnitTypeEnum.None;
            Dead = false;
        }


        /// <summary>
        /// Mise à jour de la position de l'unité
        /// </summary>
        /// <param name="_newPosition">Nouvelle position de l'unité</param>
        public void UpdatePosition(Vector2 _newPosition)
        {
            Position = _newPosition;
        }
        //Get the values from info and assign them to the appropriate properties
        public Entity(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            Name = (String)info.GetValue("name", typeof(String));
            Position =(Vector2)info.GetValue("Position", typeof(Vector2));
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
        }


    }



    /// <summary>
    /// Types d'unités
    /// </summary>
    public enum UnitTypeEnum
    {
        None,
        Ground,
        Air,
        AirGround,
    }

}

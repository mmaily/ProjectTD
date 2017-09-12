using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameElements.Units
{

    /// <summary>
    /// Classe de base de toutes les unitées, défense ou attaque
    /// </summary>
    public abstract class Unit
    {

        /// <summary>
        /// Position de l'unitée
        /// </summary>
        public Vector2 Position { get; protected set; }
        /// <summary>
        /// Type de l'unitée (défaut : None)
        /// </summary>
        public UnitTypeEnum UnitType { get; protected set; }
        /// <summary>
        /// Nombre de points de vie de l'unitée (défaut : 1)
        /// </summary>
        public int HealthPoints { get; protected set; }
        /// <summary>
        /// Coût de construction l'unitée (défaut : 0)
        /// </summary>
        public int Cost { get; protected set; }
        /// <summary>
        /// Niveau de l'unitée (défaut : 1)
        /// </summary>
        public byte Level { get; protected set; }
        /// <summary>
        /// Vitesse de déplacement en tuiles par seconde (défaut : 0)
        /// </summary>
        public float Speed { get; protected set; }
        /// <summary>
        /// Dégâts infligés par l'unitée (défaut : 0)
        /// </summary>
        public float AttackPower { get; protected set; }
        /// <summary>
        /// Portée des attaques de l'unitée (défaut : 0)
        /// </summary>
        public float Range { get; protected set; }
        /// <summary>
        /// Vitesse d'attaque de l'unitée en tirs/seconde (défaut : 1)
        /// </summary>
        public float RateOfFire { get; protected set; }
        /// <summary>
        /// Nombre de cibles visées en un seul tir (défaut : 0)
        /// </summary>
        public float TargetNumber { get; protected set; }
        /// <summary>
        /// Type de cibles visées par cette unitée  (défaut : None)
        /// </summary>
        public UnitTypeEnum TargetType { get; protected set; }
        /// <summary>
        /// Drapeau permettant de marquer l'unitée comme détruite
        /// </summary>
        public bool Dead { get; set; }


        // TODO
        //Taille, Effet spécial;

        /// <summary>
        /// Contructeur de base d'une unitée.
        /// </summary>
        public void Init()
        {
            Position = new Vector2();
            UnitType = UnitTypeEnum.None;
            HealthPoints = 1;
            Cost = 0;
            Level = 1;
            Speed = 0;
            AttackPower = 0;
            Range = 0;
            RateOfFire = 1;
            TargetNumber = 0;
            TargetType = UnitTypeEnum.None;
        }

        public void UpdatePosition(Vector2 _newPosition)
        {
            Position = _newPosition;
        }

    }

    public enum UnitTypeEnum
    {
        None,
        Ground,
        Air,
        AirGround,
    }
}

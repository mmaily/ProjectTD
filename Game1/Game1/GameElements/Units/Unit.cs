using Microsoft.Xna.Framework;

namespace DowerTefenseGame.GameElements.Units
{

    /// <summary>
    /// Classe de base de toutes les unités, défense ou attaque
    /// </summary>
    public abstract class Unit
    {

        /// <summary>
        /// Nom de l'unité
        public string name{ get; set; }
        /// Position de l'unité
        /// </summary>
        public Vector2 Position { get; protected set; }
        /// <summary>
        /// Type de l'unité (défaut : None)
        /// </summary>
        public UnitTypeEnum UnitType { get; protected set; }
        /// <summary>
        /// Nombre de points de vie de l'unité (défaut : 1)
        /// </summary>
        public int HealthPoints { get; protected set; }
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
        /// Dégâts infligés par l'unité (défaut : 0)
        /// </summary>
        public float AttackPower { get; protected set; }
        /// <summary>
        /// Portée des attaques de l'unité (défaut : 0)
        /// </summary>
        public float Range { get; protected set; }
        /// <summary>
        /// Vitesse d'attaque de l'unité en tirs/seconde (défaut : 1)
        /// </summary>
        public float RateOfFire { get; protected set; }
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


        // TODO
        //Taille, Effet spécial;

        /// <summary>
        /// Contructeur de base d'une unité.
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


        /// <summary>
        /// Mise à jour de la position de l'unité
        /// </summary>
        /// <param name="_newPosition">Nouvelle position de l'unité</param>
        public void UpdatePosition(Vector2 _newPosition)
        {
            Position = _newPosition;
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

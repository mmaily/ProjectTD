using DowerTefense.Commons.GameElements;
using DowerTefense.Commons.GameElements.Units;
using DowerTefense.Commons.Managers;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DowerTefense.Commons.GameElements.Projectiles
{
    public class Projectile
    {
        public String name;
        public Entity target; // Suit cette unité
        public int damage; //Fais ce nombre de dégats à l'impact
        public float speed; // vitesse du projectile
        public Vector2 position;
        public Vector2 direction;//Direction normalisée
        public double tol = 5; //Tolérence pour le detection de collision
        public Boolean Exists = true;
        //Use to load all the texture
        public enum NameEnum
        {
            BasicShot, // ShotFrom BasicTower
        };

        public Projectile()
        {

        }
        /// <summary>
        /// Mise à jour du projectile
        /// </summary>
        /// <param name="_gameTime"></param>
        public void Update(GameTime _gameTime)
        {
            // Mise à jour de la direction
            UpdateDirection();
            // Mise à jour de la position
            UpdatePosition(_gameTime);
            // Vérification de la collision
            CheckCollision();
        }

        /// <summary>
        /// Mise à jour de la direction
        /// </summary>
        public void UpdateDirection()
        {
            // Récupération du vecteur direction
            direction = -(position - target.Position);
            // Normalisation
            direction.Normalize();
        }
       
        /// <summary>
        /// Mise à jour de la position
        /// </summary>
        /// <param name="_gameTime"></param>
        public void UpdatePosition(GameTime _gameTime)
        {
            position += direction * speed * _gameTime.ElapsedGameTime.Milliseconds/1000;
        }

        /// <summary>
        /// Vérification de la position
        /// </summary>
        public void CheckCollision()
        {
            //Si la distance entre le projectile est inférieur à la tolérence après le mouvement
            //On applique l'effet à l'impact défini dans chaque sous-classe projectile
            //On raise l'event OnHit() pour signifier à sa tour que son projectile a touché et doit être détruit
            if (Vector2.Distance(this.position, target.Position) < tol)
            {
                ApplyEffectOnImpact();
                // Le projectile n'existe plus
                Exists = false;
            }
        }

        /// <summary>
        /// Application des effets à l'impact
        /// </summary>
        public virtual void ApplyEffectOnImpact() { }
    }
}

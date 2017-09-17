using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace DowerTefenseGame.GameElements.Projectiles
{
    public class Projectile
    {
        public String name;
        public Unit target; // Suit cette unité
        public int damage; //Fais ce nombre de dégats à l'impact
        public float speed; // vitesse du projectile
        public Vector2 position;
        public Vector2 direction;//Direction normalisée
        public double tol = 5; //Tolérence pour le detection de collision
        //Use to load all the texture
        public enum NameEnum
        {
            BasicShot, // ShotFrom BasicTower
        }
        #region ===Event quand un projectile touche sa cible ===
        public event HitHandler OnHit;
        public delegate void HitHandler(Projectile proj, OnHitEventArgs arg);
        //Argument commun pour enter/leave range qui donne une unité en param
        public class OnHitEventArgs : EventArgs
        {
            public OnHitEventArgs(Projectile iProj)
            { proj = iProj; }
            public Projectile proj { get; set; }
        }
        #endregion

        public Projectile()
        {

        }
        public void Update()
        {
            UpdateDirection();
            UpdatePosition();
            CheckCollision();
        }

        public void UpdateDirection()
        {
            direction = -(position - target.Position);
            direction.Normalize();
        }
        public void UpdatePosition()
        {
            position += direction * speed*BuildingsManager.GetInstance().gameTime.ElapsedGameTime.Milliseconds/1000;
        }
        public void CheckCollision()
        {
            //Si la distance entre le projectile est inférieur à la tolérence après le mouvement
            //On applique l'effet à l'impact défini dans chaque sous-classe projectile
            //On raise l'event OnHit() pour signifier à sa tour que son projectile a touché et doit être détruit
            if (Vector2.Distance(this.position, target.Position) < tol)
            {
                ApplyEffectOnImpact();
                    OnHitEventArgs arg = new OnHitEventArgs(this);
                OnHit?.Invoke(this, arg);


            }
        }
        public virtual void ApplyEffectOnImpact() { }
    }
}

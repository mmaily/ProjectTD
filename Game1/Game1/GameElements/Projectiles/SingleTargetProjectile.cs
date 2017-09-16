using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace Game1.GameElements.Projectiles
{
    class SingleTargetProjectile
    {
        Unit target; // Suit cette unité
        int damage; //Fais ce nombre de dégats à l'impact
        float speed; // vitesse du projectile
        Vector2 position;
        Vector2 direction;//Direction normalisée
        double tol = 1; //Tolérence pour le detection de collision
        public SingleTargetProjectile(Unit _target, int _damage, float _speed, Vector2 _pos)
        {
            this.target = _target;
            this.damage = _damage;
            this.speed = _speed;
            this.position = _pos;
        }
        public void GeneralUpdate()
        {
         
        }

        public void UpdateDirection()
        {
            direction= (target.Position - position);
            direction.Normalize();
        }
        public void UpdatePosition()
        {
            position += direction * speed;
        }
        public void CheckCollision()
        {
            if (Vector2.Distance(this.position, target.Position) < tol)
            {
                target.Damage(damage);
            }
        }
        
    }
}

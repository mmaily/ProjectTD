using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace DowerTefenseGame.GameElements.Projectiles
{
    class SingleTargetProjectile : Projectile
    {

        public SingleTargetProjectile(Entity _target, int _damage, float _speed, Vector2 _pos, String _name) : base()
        {
            this.name = _name;
            this.target = _target;
            this.damage = _damage;
            this.speed = _speed;
            this.position = _pos;
        }

        public override void ApplyEffectOnImpact() => target.TryDamage(damage);

    }
}

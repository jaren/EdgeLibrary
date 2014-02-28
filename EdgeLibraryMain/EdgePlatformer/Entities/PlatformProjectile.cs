using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EdgeLibrary;

namespace EdgeLibrary.Platform
{
    //Bullets, arrows, etc.
    public class PlatformProjectile : PlatformSprite
    {
        public Vector2 Target { get { return _target; } set { _target = value; reloadMovement(); } }
        private Vector2 _target;
        public float Speed { get { return _speed; } set { _speed = value; reloadMovement(); } }
        private float _speed;
        public bool removeOnHit;

        public PlatformProjectile(string eTextureName, PlatformCharacter character, Vector2 targetPos, float speed) : base(MathTools.RandomID(character.ID + "_projectile"), eTextureName,  Vector2.Zero)
        {
            CollisionLayers = character.CollisionLayers;
            removeOnHit = true;
            Target = targetPos;
            Speed = speed;

            Collision += new CollisionEvent(PlatformProjectile_Collision);
        }

        public void reloadMovement()
        {
            Movement.MoveTo(_target, Speed);
        }

        private void PlatformProjectile_Collision(PlatformSprite sprite1, PlatformSprite sprite2, GameTime gameTime)
        {
            MarkedForPlatformRemoval = true;
        }
    }
}

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
        public bool RemoveFromCharacter;
        public string characterID;

        public new event CollisionEvent Collision;

        public PlatformProjectile(string eTextureName, PlatformCharacter character, Vector2 targetPos, float speed) : base(MathTools.RandomID(character.ID + "_projectile"), eTextureName,  Vector2.Zero)
        {
            CollisionBody.CollisionLayers = character.CollisionBody.CollisionLayers;
            Position = character.Position;
            removeOnHit = true;
            _speed = speed;
            _target = targetPos;
            characterID = character.ID;
            reloadMovement();

            RemoveFromCharacter = false;
        }

        public void reloadMovement()
        {
            Vector2 force = _target - Position;
            force.Normalize();
            Movement.MoveBy(force, _speed);
        }

        protected override void UpdateCollision(GameTime gameTime)
        {
            foreach (Element element in EdgeGame.SelectedScene.elements)
            {
                if (element is PlatformSprite)
                {
                    PlatformSprite sprite = (PlatformSprite)element;
                    if (sprite != this && sprite.ID != characterID)
                    {
                        if (CollisionBody.CheckForCollide(sprite.CollisionBody) && !(sprite is PlatformProjectile && ((PlatformProjectile)sprite).characterID == characterID))
                        {
                            if (Collision != null)
                            {
                                Collision(this, sprite, gameTime);
                            }

                            if (removeOnHit)
                            {
                                RemoveFromCharacter = true;
                            }
                        }
                    }
                }
            }
        }
    }
}

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
    //Any "character" such as players, enemies, bosses, etc.
    public class PlatformCharacter : PlatformSprite
    {
        public List<PlatformProjectile> Projectiles;
        public string ProjectileTexture;
        public Vector2 ProjectileScale;
        public SpriteStyle ProjectileStyle;
        public float ProjectileRotationAdd;
        public float ProjectileSpeed;
        public float ShootDelay;
        private float TimeSinceLastShoot;

        public new event CollisionEvent Collision;

        public PlatformCharacter(string eTextureName, Vector2 ePosition) : this(MathTools.RandomID(typeof(PlatformCharacter)), eTextureName, ePosition) { }

        public PlatformCharacter(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition) 
        {
            Projectiles = new List<PlatformProjectile>();
            ShootDelay = 0;
            TimeSinceLastShoot = ShootDelay;

            ProjectileTexture = "";
            ProjectileScale = Vector2.One;
            ProjectileStyle = new SpriteStyle(SpriteEffects.None, 0, Color.White);
            ProjectileSpeed = 1;
        }

        protected override void updateElement(GameTime gameTime)
        {
            base.updateElement(gameTime);

            TimeSinceLastShoot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Update(gameTime);

                if (Projectiles[i].RemoveFromCharacter)
                {
                    Projectiles.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void drawElement(GameTime gameTime)
        {
            base.drawElement(gameTime);

            foreach (PlatformProjectile projectile in Projectiles)
            {
                projectile.Draw(gameTime);
            }
        }

        protected override void UpdateCollision(GameTime gameTime)
        {
            collidingXGreater = false;
            collidingXLower = false;
            collidingYGreater = false;
            collidingYLower = false;

            foreach (Element element in EdgeGame.SelectedScene.elements)
            {
                if (element is PlatformSprite)
                {
                    PlatformSprite sprite = (PlatformSprite)element;
                    if (CollisionBody != null && sprite.CollisionBody != null && sprite != this)
                    {
                        if (CollisionBody.CheckForCollide(sprite.CollisionBody) && !Projectiles.Contains(sprite))
                        {
                            if (Collision != null)
                            {
                                Collision(this, sprite, gameTime);
                            }

                            Rectangle collision = CollisionBody.Intersect(CollisionBody, sprite.CollisionBody);

                            //If it's collided in horizontally more than vertical
                            if (Math.Abs(collision.Width) > Math.Abs(collision.Height))
                            {
                                //When collided into something, acceleration is reset
                                fallSpeed = 0;

                                if (Position.Y > sprite.Position.Y)
                                {
                                    Position = new Vector2(Position.X, Position.Y + collision.Height);
                                    collidingYGreater = true;
                                }
                                else
                                {
                                    Position = new Vector2(Position.X, Position.Y - collision.Height);
                                    collidingYLower = true;
                                }
                            }
                            else
                            {
                                if (Position.X > sprite.Position.X)
                                {
                                    Position = new Vector2(Position.X + collision.Width, Position.Y);
                                    collidingXGreater = true;
                                }
                                else
                                {
                                    Position = new Vector2(Position.X - collision.Width, Position.Y);
                                    collidingXLower = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Shoot(Vector2 targetPos, float speed)
        {
            if (TimeSinceLastShoot >= ShootDelay)
            {
                TimeSinceLastShoot = 0;
                PlatformProjectile projectile = new PlatformProjectile("", this, targetPos, speed);
                projectile.MarkedForRemoval = true;
                projectile.Speed = ProjectileSpeed;
                projectile.Texture = ResourceManager.getTexture(ProjectileTexture);
                projectile.Style = ProjectileStyle;
                projectile.Scale = ProjectileScale;
                projectile.StyleChanger.RotateSpriteTowards(projectile, targetPos, ProjectileRotationAdd);
                Projectiles.Add(projectile);
            }
        }
    }
}

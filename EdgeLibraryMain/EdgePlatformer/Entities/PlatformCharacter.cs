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
        public PlatformProjectile ProjectileToCopy;
        public float ShootDelay;
        private float TimeSinceLastShoot;

        public PlatformCharacter(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition) 
        {
            Projectiles = new List<PlatformProjectile>();
            ShootDelay = 0;
            TimeSinceLastShoot = ShootDelay;

            ProjectileToCopy = new PlatformProjectile("Pixel", this, Vector2.Zero, 0);
        }

        public override void UpdatePlatform(GameTime gameTime, Vector2 Gravity, List<PlatformSprite> sprites)
        {
            base.UpdatePlatform(gameTime, Gravity, sprites);

            TimeSinceLastShoot = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].UpdatePlatform(gameTime, Gravity, sprites);

                if (Projectiles[i].MarkedForPlatformRemoval)
                {
                    Projectiles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Shoot(Vector2 targetPos, float speed)
        {
            if (TimeSinceLastShoot >= ShootDelay)
            {
                TimeSinceLastShoot = 0;
                PlatformProjectile projectile = new PlatformProjectile("", this, targetPos, speed);
                projectile.Texture = ProjectileToCopy.Texture;
                projectile.Style = ProjectileToCopy.Style;
                projectile.Scale = ProjectileToCopy.Scale;
                Projectiles.Add(projectile);
            }
        }
    }
}

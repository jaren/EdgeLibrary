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
using EdgeLibrary.Basic;
using EdgeLibrary.Menu;
using EdgeLibrary.Effects;

namespace EdgeLibrary_Test
{
    public class PlayerShip : ESprite
    {
        int Speed;

        public PlayerShip(): base("enemyShip", new Vector2(400, EdgeGame.WindowSize.Y - 100))
        {
            Speed = 10;

            AddCollision(ECollisionBody.BodyWithSpriteAndIDs(EShapeTypes.rectangle, this, "player", new List<string>(){"asteroid", "laser"}));
            CollisionStart += new SpriteCollisionEvent(Collide);
        }

        void Collide(ESpriteCollisionArgs e)
        {
        }

        public override void FillTexture()
        {
            base.FillTexture();

            EParticleEmitter FireEmitter = new EParticleEmitter("fire", new Vector2(400, 400));
            FireEmitter.ShouldEmit = true;
            FireEmitter.DrawLayer = 3;
            FireEmitter.EmitPositionVariance = new ERangeArray(new ERange(0), new ERange(0));
            FireEmitter.ColorVariance = new ERangeArray(new ERange(0), new ERange(30, 40), new ERange(60, 80), new ERange(255));
            FireEmitter.VelocityVariance = new ERangeArray(ERange.RangeWithDiffer(0,3), ERange.RangeWithDiffer(6,2));
            FireEmitter.SizeVariance = new ERangeArray(ERange.RangeWithDiffer(50, 2), ERange.RangeWithDiffer(50, 2));
            FireEmitter.GrowSpeed = -0.2f;
            FireEmitter.StartRotationVariance = ERange.RangeWithDiffer(0, 0);
            FireEmitter.RotationSpeedVariance = ERange.RangeWithDiffer(0, 0);
            FireEmitter.LifeVariance = new ERange(225);
            FireEmitter.EmitWait = 0;
            FireEmitter.clampToAt(this, new Vector2(0, 15));
            EdgeGame.GetLayerFromObject(this).addElement(FireEmitter);
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            base.updateElement(updateArgs);

            if (updateArgs.keyboardState.IsKeyDown(Keys.Left) && Position.X > 0)
            {
                Position = new Vector2(Position.X - Speed, Position.Y);
            }
            if (updateArgs.keyboardState.IsKeyDown(Keys.Right) && Position.Y < EdgeGame.WindowSize.X)
            {
                Position = new Vector2(Position.X + Speed, Position.Y);
            }
        }
    }
}

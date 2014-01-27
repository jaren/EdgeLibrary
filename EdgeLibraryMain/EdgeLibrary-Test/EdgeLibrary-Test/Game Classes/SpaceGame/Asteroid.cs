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
    public class Asteroid : ESprite
    {
        int Speed;
        int Size;

        EScene GameScene;

        public Asteroid(EScene gameScene, int speed, int size, int X) : base("meteorBig", new Vector2(X, 0), size, size)
        {
            GameScene = gameScene;
            Speed = speed;
            Size = size;

            //The "collides with IDs" is blank because the player is colliding with the asteroids, not the other way around
            AddCollision(ECollisionBody.BodyWithSpriteAndIDs(EShapeTypes.circle, this, "asteroid", new List<string>()));

            EActionMove fall = new EActionMove(new Vector2(Position.X, EdgeGame.WindowSize.Y + Height), speed);
            runAction(fall);
        }

        public override void FillTexture()
        {
            base.FillTexture();

            EParticleEmitter FireEmitter = new EParticleEmitter("fire", new Vector2(400, 400));
            FireEmitter.ShouldEmit = true;
            FireEmitter.DrawLayer = 3;
            FireEmitter.EmitPositionVariance = new ERangeArray(new ERange(0), new ERange(0));
            FireEmitter.ColorVariance = new ERangeArray(new ERange(0), new ERange(60, 80), new ERange(0, 10), new ERange(255));
            FireEmitter.VelocityVariance = new ERangeArray(ERange.RangeWithDiffer(0, 5), ERange.RangeWithDiffer(0, 5));
            FireEmitter.SizeVariance = new ERangeArray(ERange.RangeWithDiffer(Size/1.5f, 2), ERange.RangeWithDiffer(Size/1.5f, 2));
            FireEmitter.GrowSpeed = -0.2f;
            FireEmitter.StartRotationVariance = ERange.RangeWithDiffer(0, 0);
            FireEmitter.RotationSpeedVariance = ERange.RangeWithDiffer(0, 0);
            FireEmitter.LifeVariance = new ERange(225);
            FireEmitter.EmitWait = 0;
            FireEmitter.clampToAt(this, new Vector2(0, 0));
            GameScene.addElement(FireEmitter);
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            base.updateElement(updateArgs);

            if (Position.Y >= EdgeGame.WindowSize.Y + Height / 2)
            {
                EdgeGame.RemoveElement(this);
               // Texture = null;
                CollisionBody = null;
                Actions.Clear();
            }
        }
    }
}

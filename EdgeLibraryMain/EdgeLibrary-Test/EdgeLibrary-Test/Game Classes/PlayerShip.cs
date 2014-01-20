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
        public PlayerShip(string eTextureName, Vector2 ePosition): base(eTextureName, ePosition)
        {
        }

        public override void FillTexture()
        {
            EParticleEmitter FireEmitter = new EParticleEmitter("fire", new Vector2(400, 400));
            FireEmitter.ShouldEmit = true;
            FireEmitter.DrawLayer = 3;
            FireEmitter.EmitPositionVariance = new ERangeArray(new ERange(0), new ERange(0));
            FireEmitter.ColorVariance = new ERangeArray(new ERange(60, 80), new ERange(30, 40), new ERange(0), new ERange(255));
            FireEmitter.VelocityVariance = new ERangeArray(ERange.RangeWithDiffer(0, 4), ERange.RangeWithDiffer(0, 4));
            FireEmitter.SizeVariance = new ERangeArray(ERange.RangeWithDiffer(100, 25), ERange.RangeWithDiffer(100, 25));
            FireEmitter.GrowSpeed = 1f;
            FireEmitter.StartRotationVariance = ERange.RangeWithDiffer(0, 0);
            FireEmitter.RotationSpeedVariance = ERange.RangeWithDiffer(0, 0);
            FireEmitter.LifeVariance = new ERange(500);
            FireEmitter.EmitWait = 0;
            FireEmitter.clampTo(this);
            EdgeGame.GetLayerFromObject(this).addElement(FireEmitter);
        }
    }
}

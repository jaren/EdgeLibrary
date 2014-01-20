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
            EParticleEmitter mouseEmitter = new EParticleEmitter("fire", new Vector2(400, 400));
            mouseEmitter.ShouldEmit = true;
            mouseEmitter.DrawLayer = 3;
            mouseEmitter.EmitPositionVariance = new ERangeArray(new ERange(0), new ERange(0));
            mouseEmitter.ColorVariance = new ERangeArray(new ERange(60, 80), new ERange(30, 40), new ERange(0), new ERange(255));
            mouseEmitter.VelocityVariance = new ERangeArray(ERange.RangeWithDiffer(0, 4), ERange.RangeWithDiffer(0, 4));
            mouseEmitter.SizeVariance = new ERangeArray(ERange.RangeWithDiffer(100, 25), ERange.RangeWithDiffer(100, 25));
            mouseEmitter.GrowSpeed = 1f;
            mouseEmitter.StartRotationVariance = ERange.RangeWithDiffer(0, 0);
            mouseEmitter.RotationSpeedVariance = ERange.RangeWithDiffer(0, 0);
            mouseEmitter.LifeVariance = new ERange(500);
            mouseEmitter.EmitWait = 0;
            mouseEmitter.ClampToMouse();
            EdgeGame.GetLayerFromObject(this).addElement(mouseEmitter);
        }
    }
}

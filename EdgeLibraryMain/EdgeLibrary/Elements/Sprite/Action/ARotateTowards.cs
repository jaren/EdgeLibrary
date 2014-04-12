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

namespace EdgeLibrary
{
    //Rotates a sprite towards another one
    public class ARotateTowards : Action
    {
        public Sprite Target;
        //The higher this is, the faster the sprite will rotate
        public float Speed;

        //The number of radians to add to the sprite's rotation after rotating towards the target sprite
        public float AdditionalAngle;

        public ARotateTowards(Sprite target) : this(target, 0) { }
        public ARotateTowards(Sprite target, float additionalAngle) : this(target, 360, additionalAngle) {}
        public ARotateTowards(Sprite target, float speed, float additionalAngle) : base()
        {
            Target = target;
            Speed = speed;
            AdditionalAngle = additionalAngle;
        }
        
        public ARotateTowards(string ID, Sprite target) : this(ID, target, 0) { }
        public ARotateTowards(string ID, Sprite target, float additionalAngle) : this(ID, target, 360, additionalAngle) {}
        public ARotateTowards(string ID, Sprite target, float speed, float additionalAngle) : base(ID)
        {
            Target = target;
            Speed = speed;
            AdditionalAngle = additionalAngle;
        }

        //Calculates rotation
        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            float targetRotation = (float)Math.Atan2(Target.Position.Y - sprite.Position.Y, Target.Position.X - sprite.Position.X) + AdditionalAngle;

            //Adds the speed to the sprite's rotation if it won't be more/less than the target rotation
            if (sprite.Rotation < targetRotation)
            {
                if (sprite.Rotation + Math.Abs(Speed) > targetRotation)
                {
                    sprite.Rotation = targetRotation;
                }
                else
                {
                    sprite.Rotation += Math.Abs(Speed);
                }
            }
            else if (sprite.Rotation > targetRotation)
            {
                if (sprite.Rotation - Math.Abs(Speed) < targetRotation)
                {
                    sprite.Rotation = targetRotation;
                }
                else
                {
                    sprite.Rotation -= Math.Abs(Speed);
                }
            }
        }

        public override Action Clone()
        {
            return new ARotateTowards(ID, Target, Speed, AdditionalAngle);
        }
    }
}

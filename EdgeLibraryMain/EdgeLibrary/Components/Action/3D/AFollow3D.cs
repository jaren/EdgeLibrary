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
    //Moves a sprite to a certain sprite at a certian speed
    public class AFollow3D : Action3D
    {
        public Sprite3D Target;
        public float Speed;

        public AFollow3D(Sprite3D target, float speed)
        {
            Target = target;
            Speed = speed;
        }

        //Moves the sprite by the speed towards the target, checks if it should end and automatically removes it
        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            //Calculates what the sprite should move by
            Vector3 moveVector = Target.Position - sprite.Position;

            //If the moveVector is Vector2.Zero, then normalizing it will result in NaN, and checkIfEnd will return false
            //To fix this, if moveVector is Vector2.Zero, then toRemove will be set to true (Sprite's position is the Target Position)
            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();
                moveVector *= Speed * EdgeGame.GameSpeed;

                if (!checkIfEnd(moveVector, Target.Position, sprite.Position)) { sprite.Position += moveVector; }
            }
        }

        //Checks if movement should stop
        //Move vector - the vector which the sprite has moved by
        //Target - the end position
        //Position - the sprite's current position
        private bool checkIfEnd(Vector3 moveVector, Vector3 target, Vector3 position)
        {
            position += moveVector;

            if ((moveVector.X < 0) && (position.X < target.X)) { return true; }
            else if ((moveVector.X > 0) && (position.X > target.X)) { return true; }

            if ((moveVector.Y < 0) && (position.Y < target.Y)) { return true; }
            else if ((moveVector.Y > 0) && (position.Y > target.Y)) { return true; }

            return false;
        }

        public override Action3D Clone()
        {
            return new AFollow3D(Target, Speed);
        }
    }
}

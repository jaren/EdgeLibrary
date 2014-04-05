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
    //Moves a sprite to a certain position at a certian speed
    public class AMove : AAction
    {
        public Vector2 TargetPosition;
        public float Speed;

        public AMove(Vector2 targetPosition, float speed)
        {
            TargetPosition = targetPosition;
            Speed = speed;
        }

        //Moves the sprite by the speed towards the target position, checks if it should end and automatically removes it
        public override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            //Calculates what the sprite should move by
            Vector2 moveVector = TargetPosition - sprite.Position;
            moveVector.Normalize();
            moveVector *= Speed;

            if (checkIfEnd(moveVector, TargetPosition, sprite.Position)) { Stop(); }
            sprite.Position += moveVector;
        }

        //Checks if movement should stop
        //Move vector - the vector which the sprite has moved by
        //Target - the end position
        //Position - the sprite's current position
        private bool checkIfEnd(Vector2 moveVector, Vector2 target, Vector2 position)
        {
            position += moveVector;

            if ((moveVector.X < 0) && (position.X < target.X)) { return true; }
            else if ((moveVector.X > 0) && (position.X > target.X)) { return true; }

            if ((moveVector.Y < 0) && (position.Y < target.Y)) { return true; }
            else if ((moveVector.Y > 0) && (position.Y > target.Y)) { return true; }

            return false;
        }


        //Returns an AMove copy
        public override AAction Copy()
        {
            return new AMove(TargetPosition, Speed);
        }
    }
}

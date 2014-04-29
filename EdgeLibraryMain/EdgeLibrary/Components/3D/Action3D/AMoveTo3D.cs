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
    public class AMoveTo3D : Action3D
    {
        public Vector3 TargetPosition;
        public float Speed;

        public AMoveTo3D(Vector3 targetPosition, float speed)
        {
            TargetPosition = targetPosition;
            Speed = speed;
        }

        //Creates a waypoint movement path
        public static ASequence CreateMoveSequence(float speed, params Vector3[] targetPositions)
        {
            List<Vector3> vectors = new List<Vector3>(targetPositions);
            return CreateMoveSequence(speed, vectors);
        }
        public static ASequence CreateMoveSequence(float speed, List<Vector3> targetPositions)
        {
            List<Action> moves = new List<Action>();

            foreach (Vector3 vector in targetPositions)
            {
                moves.Add(new AMoveTo3D(vector, speed));
            }

            return new ASequence(moves);
        }

        //Moves the sprite by the speed towards the target position, checks if it should end and automatically removes it
        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            //Calculates what the sprite should move by
            Vector3 moveVector = TargetPosition - sprite.Position;

            //If the moveVector is Vector2.Zero, then normalizing it will result in NaN, and checkIfEnd will return false
            //To fix this, if moveVector is Vector2.Zero, then toRemove will be set to true (Sprite's position is the Target Position)
            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();
                moveVector *= Speed;

                if (checkIfEnd(moveVector, TargetPosition, sprite.Position)) { Stop(gameTime, sprite); }
                sprite.Position += moveVector;
            }
            else
            {
                Stop(gameTime, sprite);
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


        //Returns an AMoveTo Clone
        public override Action3D Clone()
        {
            return new AMoveTo3D(TargetPosition, Speed);
        }
    }
}

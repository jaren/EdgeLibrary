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
    /// <summary>
    /// The base for a modifier - a class which modifies sprite properties such as position, color, rotation, scale, etc.
    /// </summary>
    public class Modifier
    {
        //Each modifier type should have a different ID
        public string ID;

        public Modifier(string id) { ID = id; }

        //Each modifier type should override Update
        public virtual void Update(Sprite sprite, GameTime gameTime) {}
    }

    //Changes how the MovementModifier executes
    public enum MovementType
    {
        Move,
        MoveVelocity,
        Follow,
        PointRotation,
        Clamp,
        None
    }


    //Changes the sprite's position
    public class MovementModifier : Modifier
    {
        protected Vector2 TargetVector;
        protected Vector2 MoveVelocity;
        protected Element TargetElement;
        protected Vector2 MoveDifference;
        protected MovementType MovementType;
        public float Speed;

        //An event which is called when MoveTo is finished
        public delegate void MoveEvent(MovementModifier modifier, Sprite sprite, GameTime gameTime);
        public event MoveEvent OnFinishMove = delegate { };

        public MovementModifier() : base("Movement") { MovementType = MovementType.None; }

        //Updates with a sprite
        public override void Update(Sprite sprite, GameTime gameTime)
        {
            Vector2 moveVector = Vector2.Zero;
            switch (MovementType)
            {
                    //Clamps to the target position with move difference
                case MovementType.Clamp:
                    sprite.Position = TargetElement.Position + MoveDifference;
                    break;
                    //Follows the given element at a certain speed
                case MovementType.Follow:
                    moveVector = new Vector2(TargetElement.Position.X - sprite.Position.X, TargetElement.Position.Y - sprite.Position.Y);
                    moveVector.Normalize();
                    if (!checkIfEnd(moveVector, TargetElement.Position, sprite.Position + moveVector * Speed))
                    {
                        sprite.Position += moveVector * Speed;
                    }
                    break;
                    //Moves to a point at a certain speed
                case MovementType.Move:
                    moveVector = new Vector2(TargetVector.X - sprite.Position.X, TargetVector.Y - sprite.Position.Y);
                    if (moveVector != Vector2.Zero)
                    {
                        moveVector.Normalize();
                    }
                    if (!checkIfEnd(moveVector, TargetVector, sprite.Position + moveVector * Speed))
                    {
                        sprite.Position += moveVector * Speed;
                    }
                    else
                    {
                        MovementType = MovementType.None;
                        OnFinishMove(this, sprite, gameTime);
                    }
                    break;
                    //Keeps moving at a constant velocity
                case MovementType.MoveVelocity:
                    sprite.Position += MoveVelocity * Speed;
                    break;
                    //Rotates around a point
                case MovementType.PointRotation:
                    float dist = Vector2.Distance(sprite.Position, TargetVector);
                    float angleMeasure = (float)Math.Atan2(sprite.Position.Y - TargetVector.Y, sprite.Position.X - TargetVector.X);
                    angleMeasure += Speed / 40;
                    float newDiffX = (float)Math.Cos(angleMeasure) * dist;
                    float newDiffY = (float)Math.Sin(angleMeasure) * dist;
                    sprite.Position = new Vector2(TargetVector.X + newDiffX, TargetVector.Y + newDiffY);
                    break;
            }
        }

        //Checks if movement should stop
        private bool checkIfEnd(Vector2 moveVector, Vector2 target, Vector2 position)
        {
            if ((moveVector.X < 0) && (position.X < target.X)) { return true; }
            else if ((moveVector.X > 0) && (position.X > target.X)) { return true; }

            if ((moveVector.Y < 0) && (position.Y < target.Y)) { return true; }
            else if ((moveVector.Y > 0) && (position.Y > target.Y)) { return true; }

            return false;
        }

        //Sets the movement type to none, stopping motion
        public void StopMoving()
        {
            MovementType = MovementType.None;
        }

        //Moves to a position
        public void MoveTo(Vector2 target, float speed)
        {
            TargetVector = target;
            Speed = speed;
            MovementType = MovementType.Move;
        }

        //Moves at a constant velocity
        public void MoveBy(Vector2 target, float speed)
        {
            MoveVelocity = target;
            Speed = speed;
            MovementType = MovementType.MoveVelocity;
        }

        //Follows an element at a speed
        public void FollowElement(Element target, float speed)
        {
            TargetElement = target;
            Speed = speed;
            MovementType = MovementType.Follow;
        }

        //Clamps to an element
        public void ClampTo(Element target)
        {
            TargetElement = target;
            MoveDifference = Vector2.Zero;
            MovementType = MovementType.Clamp;
        }
        public void ClampTo(Element target, Vector2 difference)
        {
            TargetElement = target;
            MoveDifference = difference;
            MovementType = MovementType.Clamp;
        }

        //Rotates around a point
        public void RotateAround(Vector2 target, float speed)
        {
            TargetVector = target;
            Speed = speed;
            MovementType = MovementType.PointRotation;
        }
    }

    /// <summary>
    /// Modifies the sprite's color and rotation
    /// </summary>
    public class AppearanceModifier : Modifier
    {
        private bool colorChanging;
        private bool usingColorIndex;
        private bool rotating;

        private Color color1;
        private Color color2;
        private ColorChangeIndex colorIndex;
        private float colorChangeTime;
        private float elapsedColorChangeTime;

        private float angleAdd;
        private bool rotateToPoint;
        private Vector2 rotateTarget;
        private Element activeRotateTarget;

        //Called when the color is finished changing
        public delegate void StyleColorEvent(AppearanceModifier modifier, Color finishColor);
        public event StyleColorEvent OnFinishColorChange = delegate { };

        public AppearanceModifier() : base("Appearance")
        {
            colorChanging = false;
            rotating = false;

            color1 = Color.White;
            color2 = Color.White;
            colorChangeTime = 0;
            elapsedColorChangeTime = 0;

            angleAdd = 0;
            rotateToPoint = true;
            rotateTarget = Vector2.Zero;
            activeRotateTarget = null;
        }

        //Updates with a sprite
        public override void Update(Sprite sprite, GameTime gameTime)
        {
            if (colorChanging)
            {
                if (usingColorIndex)
                {
                    //Sets the sprite's color to be the color index
                    sprite.Color = colorIndex.Update(gameTime);
                }
                else
                {
                    //Sets the sprite's color to be a color between color1 and color2
                    elapsedColorChangeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (elapsedColorChangeTime > colorChangeTime)
                    {
                        colorChanging = false;

                        sprite.Color = Color.Lerp(color1, color2, 1);

                        OnFinishColorChange(this, color2);
                    }
                    else
                    {
                        sprite.Color = Color.Lerp(color1, color2, elapsedColorChangeTime / colorChangeTime);
                    }
                }
            }
            if (rotating)
            {
                if (rotateToPoint)
                {
                    //Rotates the sprite towards a point
                    sprite.Rotation = MathHelper.ToDegrees((float)Math.Atan2(sprite.Position.Y - rotateTarget.Y, sprite.Position.X - rotateTarget.X)) + angleAdd;
                }
                else
                {
                    //Rotates the sprite towards an element
                    sprite.Rotation = MathHelper.ToDegrees((float)Math.Atan2(sprite.Position.Y - activeRotateTarget.Position.Y, sprite.Position.X - activeRotateTarget.Position.X)) + angleAdd;
                }
            }
        }

        //Changes the color
        public void ColorChange(Color color, Color nextColor, float time)
        {
            color1 = color;
            color2 = nextColor;
            colorChangeTime = time;
            elapsedColorChangeTime = 0;
            usingColorIndex = false;
            colorChanging = true;
        }
        public void ColorChange(ColorChangeIndex index)
        {
            colorIndex = index;
            usingColorIndex = true;
            colorChanging = true;
        }

        //Rotates towards a point
        public void Rotate(Vector2 target, float addAngle)
        {
            angleAdd = addAngle;
            rotateTarget = target;
            rotateToPoint = true;
            rotating = true;
        }

        //Rotates a sprite towards a point - only sets rotation
        public static void RotateSpriteTowards(Sprite sprite, Vector2 target, float addAngle)
        {
            sprite.Rotation = MathHelper.ToDegrees((float)Math.Atan2(sprite.Position.Y - target.Y, sprite.Position.X - target.X)) + addAngle;
        }
        public static void RotateSpriteTowards(Sprite sprite, Element target, float addAngle)
        {
            sprite.Rotation = MathHelper.ToDegrees((float)Math.Atan2(sprite.Position.Y - target.Position.Y, sprite.Position.X - target.Position.X)) + addAngle;
        }

        //Rotates a sprite towards an element
        public void Rotate(Element target, float addAngle)
        {
            angleAdd = addAngle;
            activeRotateTarget = target;
            rotateToPoint = false;
            rotating = true;
        }

        //Stops rotating and color changing
        public void Stop()
        {
            rotating = false;
            colorChanging = false;
        }
        public void StopRotating()
        {
            rotating = false;
        }
        public void StopColorChanging()
        {
            colorChanging = false;
        }
    }
}

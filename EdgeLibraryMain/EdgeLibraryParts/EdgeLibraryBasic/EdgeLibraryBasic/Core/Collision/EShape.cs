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
using System.Xml;

namespace EdgeLibrary.Basic
{
    //Provides the base for the collision bodies, checks if other shapes are in collision and supports debug draw
    public enum EShapeTypes
    {
        circle,
        rectangle
    };

    public class EShape
    {
        public EShapeTypes ShapeType { get; private set; }
        public Vector2 CenterPosition { get; set; }

        public EShape(Vector2 position, EShapeTypes shapeType)
        {
            CenterPosition = position;
            ShapeType = shapeType;
        }

        //Check for collision here
        public virtual bool CollidesWith(EShape shape)
        {
            return false;
        }

        public virtual void DebugDraw(SpriteBatch spriteBatch, Color drawColor) { }
    }

    public class EShapeCircle : EShape
    {
        public float Radius {get; set;}

        public EShapeCircle(Vector2 position, float radius) : base(position, EShapeTypes.circle)
        {
            Radius = radius;
        }

        public override bool CollidesWith(EShape shape)
        {
            switch (shape.ShapeType)
            {
                case EShapeTypes.circle:
                    return (EMath.DistanceBetween(CenterPosition, shape.CenterPosition) <= (Radius + ((EShapeCircle)shape).Radius));
                    break;
                case EShapeTypes.rectangle:
                    return shape.CollidesWith(this);
                    break;
            }
            return false;
        }

        public override void DebugDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            List<Vector2> points = EMath.GetOuterCirclePoints(CenterPosition, Radius);
            foreach (Vector2 point in points)
            {
                EMath.DrawPixelAt(spriteBatch, point, drawColor);
            }
        }
    }

    public class EShapeRectangle : EShape
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public EShapeRectangle(Vector2 position, float width, float height) : base(position, EShapeTypes.rectangle)
        {
            Width = width;
            Height = height;
        }

        public override bool CollidesWith(EShape shape)
        {
            switch (shape.ShapeType)
            {
                case EShapeTypes.circle:
                    Rectangle rectangle = new Rectangle((int)CenterPosition.X + (int)Width / 2, (int)CenterPosition.Y - (int)Height / 2, (int)Width, (int)Height);
                    List<Vector2> circlePoints = EMath.GetCirclePoints(shape.CenterPosition, ((EShapeCircle)shape).Radius);
                    foreach(Vector2 point in circlePoints)
                    {
                        if (rectangle.Contains((int)point.X, (int)point.Y))
                        {
                            return true;
                        }
                    }
                    return false;
                    break;
                case EShapeTypes.rectangle:
                    return new Rectangle((int)CenterPosition.X + (int)Width / 2, (int)CenterPosition.Y - (int)Height / 2, (int)Width, (int)Height).Intersects(new Rectangle((int)shape.CenterPosition.X + (int)((EShapeRectangle)shape).Width / 2, (int)shape.CenterPosition.Y + (int)((EShapeRectangle)shape).Height / 2, (int)((EShapeRectangle)shape).Width, (int)((EShapeRectangle)shape).Height));
                    break;
            }
            return false;
        }

        public override void DebugDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Rectangle rectangle = new Rectangle((int)CenterPosition.X - (int)Width / 2, (int)CenterPosition.Y - (int)Height / 2, (int)Width, (int)Height);
            EMath.DrawRectangleAt(spriteBatch, new Vector2(rectangle.Left, rectangle.Top), 1, rectangle.Height, drawColor);
            EMath.DrawRectangleAt(spriteBatch, new Vector2(rectangle.Right, rectangle.Top), 1, rectangle.Height, drawColor);
            EMath.DrawRectangleAt(spriteBatch, new Vector2(rectangle.Left, rectangle.Top), rectangle.Width, 1, drawColor);
            EMath.DrawRectangleAt(spriteBatch, new Vector2(rectangle.Left, rectangle.Bottom), rectangle.Width, 1, drawColor);
        }
    }
}

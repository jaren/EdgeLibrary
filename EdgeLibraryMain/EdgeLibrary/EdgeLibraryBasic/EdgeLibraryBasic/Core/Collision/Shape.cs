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
using System.Drawing;
using System.Xml;

namespace EdgeLibrary
{
    //Provides the base for the collision bodies, checks if other shapes are in collision and supports debug draw
    public enum ShapeTypes
    {
        circle,
        rectangle
    };

    public class Shape
    {
        public ShapeTypes ShapeType { get; private set; }
        public Vector2 CenterPosition { get; set; }

        public Shape(Vector2 position, ShapeTypes shapeType)
        {
            CenterPosition = position;
            ShapeType = shapeType;
        }

        //Check for collision here
        public virtual bool CollidesWith(Shape shape)
        {
            return false;
        }

        public virtual void DebugDraw(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color drawColor) { }
    }

    public class ShapeCircle : Shape
    {
        public float Radius {get; set;}

        public ShapeCircle(Vector2 position, float radius) : base(position, ShapeTypes.circle)
        {
            Radius = radius;
        }

        public override bool CollidesWith(Shape shape)
        {
            switch (shape.ShapeType)
            {
                case ShapeTypes.circle:
                    return (MathGenerator.DistanceBetween(CenterPosition, shape.CenterPosition) <= (Radius + ((ShapeCircle)shape).Radius));
                    break;
                case ShapeTypes.rectangle:
                    return shape.CollidesWith(this);
                    break;
            }
            return false;
        }

        public override void DebugDraw(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color drawColor)
        {
            List<Vector2> points = MathGenerator.GetOuterCirclePoints(CenterPosition, Radius);
            foreach (Vector2 point in points)
            {
                TextureGenerator.DrawPixelAt(spriteBatch, point, drawColor);
            }
        }
    }

    public class ShapeRectangle : Shape
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public ShapeRectangle(Vector2 position, float width, float height) : base(position, ShapeTypes.rectangle)
        {
            Width = width;
            Height = height;
        }

        public override bool CollidesWith(Shape shape)
        {
            switch (shape.ShapeType)
            {
                case ShapeTypes.circle:
                    RectangleF RectangleF = new RectangleF(CenterPosition.X - Width / 2, CenterPosition.Y - Height / 2, Width, Height);
                    ShapeCircle realshape = ((ShapeCircle)shape);
                    if (RectangleF.IntersectsWith(new RectangleF(realshape.CenterPosition.X - realshape.Radius, CenterPosition.Y - realshape.Radius, realshape.Radius*2, realshape.Radius*2)))
                    {
                    List<Vector2> circlePoints = MathGenerator.GetCirclePoints(shape.CenterPosition, realshape.Radius);
                    foreach(Vector2 point in circlePoints)
                    {
                        if (RectangleF.Contains(point.X, point.Y))
                        {
                            return true;
                        }
                    }
                    }
                    return false;
                    break;
                case ShapeTypes.rectangle:
                    return new RectangleF(CenterPosition.X - Width / 2, CenterPosition.Y - Height / 2, Width, Height).IntersectsWith(new RectangleF(shape.CenterPosition.X - ((ShapeRectangle)shape).Width / 2, shape.CenterPosition.Y + ((ShapeRectangle)shape).Height / 2, ((ShapeRectangle)shape).Width, ((ShapeRectangle)shape).Height));
                    break;
            }
            return false;
        }

        public override void DebugDraw(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color drawColor)
        {
            RectangleF rectangle = new RectangleF(CenterPosition.X - Width / 2, CenterPosition.Y - Height / 2, Width, Height);
            TextureGenerator.DrawRectangleAt(spriteBatch, new Vector2(rectangle.Left, rectangle.Top), 1, rectangle.Height, drawColor);
            TextureGenerator.DrawRectangleAt(spriteBatch, new Vector2(rectangle.Right, rectangle.Top), 1, rectangle.Height, drawColor);
            TextureGenerator.DrawRectangleAt(spriteBatch, new Vector2(rectangle.Left, rectangle.Top), rectangle.Width, 1, drawColor);
            TextureGenerator.DrawRectangleAt(spriteBatch, new Vector2(rectangle.Left, rectangle.Bottom), rectangle.Width, 1, drawColor);
        }
    }
}

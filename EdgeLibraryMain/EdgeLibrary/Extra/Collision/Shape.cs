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

        public virtual void ScaleWith(Sprite sprite) { }

        public virtual void DebugDraw(Microsoft.Xna.Framework.Color drawColor) { }
    }

    public class ShapeCircle : Shape
    {
        public float Radius { get { return _radius; } set { _radius = value; } }
        private float _radius;

        public ShapeCircle(Vector2 position, float radius) : base(position, ShapeTypes.circle)
        {
            _radius = radius;
        }
        public override bool CollidesWith(Shape shape)
        {
            switch (shape.ShapeType)
            {
                case ShapeTypes.circle:
                    return (Vector2.Distance(CenterPosition, shape.CenterPosition) <= (_radius + ((ShapeCircle)shape).Radius));
                    break;
                case ShapeTypes.rectangle:
                    return shape.CollidesWith(this);
                    break;
            }
            return false;
        }

        public override void DebugDraw(Microsoft.Xna.Framework.Color drawColor)
        {
            foreach (Vector2 point in MathTools.GetOuterCirclePoints(CenterPosition, _radius))
            {
                TextureTools.DrawPixelAt(point, EdgeGame.DebugDrawColor);
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
                    Vector2 circleDistance;
                    circleDistance.X = Math.Abs(shape.CenterPosition.X - CenterPosition.X);
                    circleDistance.Y = Math.Abs(shape.CenterPosition.Y - CenterPosition.Y);

                    if (circleDistance.X > (Width/2 + ((ShapeCircle)shape).Radius)) { return false; }
                    if (circleDistance.Y > (Height / 2 + ((ShapeCircle)shape).Radius)) { return false; }

                    if (circleDistance.X <= (Width/2)) { return true; } 
                    if (circleDistance.Y <= (Height/2)) { return true; }

                    float cornerDistance_sq = ((circleDistance.X - Width / 2) * (circleDistance.X - Width / 2) + (circleDistance.Y - Height / 2) * (circleDistance.Y - Height / 2));

                    return (cornerDistance_sq <= (((ShapeCircle)shape).Radius * ((ShapeCircle)shape).Radius));
                    break;
                case ShapeTypes.rectangle:
                    return new RectangleF(CenterPosition.X - Width / 2, CenterPosition.Y - Height / 2, Width, Height).IntersectsWith(new RectangleF(shape.CenterPosition.X - ((ShapeRectangle)shape).Width / 2, shape.CenterPosition.Y + ((ShapeRectangle)shape).Height / 2, ((ShapeRectangle)shape).Width, ((ShapeRectangle)shape).Height));
                    break;
            }
            return false;
        }

        public override void DebugDraw(Microsoft.Xna.Framework.Color drawColor)
        {
            RectangleF rectangle = new RectangleF(CenterPosition.X - Width / 2, CenterPosition.Y - Height / 2, Width, Height);
            TextureTools.DrawRectangleAt(new Vector2(rectangle.Left, rectangle.Top), 1, rectangle.Height, drawColor);
            TextureTools.DrawRectangleAt(new Vector2(rectangle.Right, rectangle.Top), 1, rectangle.Height, drawColor);
            TextureTools.DrawRectangleAt(new Vector2(rectangle.Left, rectangle.Top), rectangle.Width, 1, drawColor);
            TextureTools.DrawRectangleAt(new Vector2(rectangle.Left, rectangle.Bottom), rectangle.Width, 1, drawColor);
        }
    }
}

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

        public virtual Rectangle AsRectangle()
        {
            return Rectangle.Empty;
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

        public override Rectangle AsRectangle()
        {
            return new Rectangle((int)CenterPosition.X - (int)_radius, (int)CenterPosition.Y - (int)_radius, (int)_radius * 2, (int)_radius * 2);
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

        public override void DebugDraw(Color drawColor)
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
                    return new Rectangle((int)CenterPosition.X - (int)Width / 2, (int)CenterPosition.Y - (int)Height / 2, (int)Width, (int)Height).Intersects(new Rectangle((int)shape.CenterPosition.X - (int)((ShapeRectangle)shape).Width / 2, (int)shape.CenterPosition.Y + (int)((ShapeRectangle)shape).Height / 2, (int)((ShapeRectangle)shape).Width, (int)((ShapeRectangle)shape).Height));
                    break;
            }
            return false;
        }

        public override Rectangle AsRectangle()
        {
            return new Rectangle((int)CenterPosition.X - (int)Width/2, (int)CenterPosition.Y - (int)Height/2, (int)Width, (int)Height);
        }

        public override void DebugDraw(Color drawColor)
        {
            Rectangle rectangle = new Rectangle((int)CenterPosition.X - (int)Width / 2, (int)CenterPosition.Y - (int)Height / 2, (int)Width, (int)Height);
            TextureTools.DrawHollowRectangleAt(rectangle, drawColor, 1);
        }
    }
}

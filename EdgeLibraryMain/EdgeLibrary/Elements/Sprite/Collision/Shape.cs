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
    public enum ShapeTypes
    {
        circle,
        rectangle
    };

    //Provides the base for the collision bodies, checks if other shapes are in collision and supports debug draw
    public class Shape
    {
        //The type of the shape
        public ShapeTypes ShapeType { get; private set; }
        
        //The center position of the shape
        public Vector2 CenterPosition { get; set; }

        public Shape(Vector2 position, ShapeTypes shapeType)
        {
            CenterPosition = position;
            ShapeType = shapeType;
        }

        //Converts the shape into a rectangle
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

        public virtual void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch, Color color) { }
    }

    /// <summary>
    /// A circle shape
    /// </summary>
    public class ShapeCircle : Shape
    {
        //The radius of the circle
        public float Radius { get { return _radius; } set { _radius = value; } }
        private float _radius;

        public ShapeCircle(Vector2 position, float radius) : base(position, ShapeTypes.circle)
        {
            _radius = radius;
        }

        //Converts the circle into a rectangle
        public override Rectangle AsRectangle()
        {
            return new Rectangle((int)CenterPosition.X - (int)_radius, (int)CenterPosition.Y - (int)_radius, (int)_radius * 2, (int)_radius * 2);
        }

        //Checks if it collides with another shape
        public override bool CollidesWith(Shape shape)
        {
            switch (shape.ShapeType)
            {
                case ShapeTypes.circle:
                    return (Vector2.Distance(CenterPosition, shape.CenterPosition) <= (_radius + ((ShapeCircle)shape).Radius));

                case ShapeTypes.rectangle:
                    return shape.CollidesWith(this);

            }
            return false;
        }

        //Draws a circle using generated points
        public override void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            foreach (Vector2 point in MathTools.GetOuterCirclePoints(CenterPosition, _radius))
            {
                spriteBatch.Draw(Resources.GetTexture("Pixel"), new Rectangle((int)point.X, (int)point.Y, 1, 1), color);
            }
        }
    }

    public class ShapeRectangle : Shape
    {
        //The width/height of the rectangle
        public float Width { get; set; }
        public float Height { get; set; }

        public ShapeRectangle(Vector2 position, float width, float height) : base(position, ShapeTypes.rectangle)
        {
            Width = width;
            Height = height;
        }

        //Checks if the shape collides with another shape
        public override bool CollidesWith(Shape shape)
        {
            switch (shape.ShapeType)
            {
                case ShapeTypes.circle:
                    Vector2 circleDistance;
                    circleDistance.X = Math.Abs(shape.CenterPosition.X - CenterPosition.X);
                    circleDistance.Y = Math.Abs(shape.CenterPosition.Y - CenterPosition.Y);

                    if (circleDistance.X > (Width / 2 + ((ShapeCircle)shape).Radius)) { return false; }
                    if (circleDistance.Y > (Height / 2 + ((ShapeCircle)shape).Radius)) { return false; }

                    if (circleDistance.X <= (Width / 2)) { return true; }
                    if (circleDistance.Y <= (Height / 2)) { return true; }

                    float cornerDistance_sq = ((circleDistance.X - Width / 2) * (circleDistance.X - Width / 2) + (circleDistance.Y - Height / 2) * (circleDistance.Y - Height / 2));

                    return (cornerDistance_sq <= (((ShapeCircle)shape).Radius * ((ShapeCircle)shape).Radius));

                case ShapeTypes.rectangle:
                    return AsRectangle().Intersects(shape.AsRectangle());
            }
            return false;
        }

        //Converts the shape into a rectangle
        public override Rectangle AsRectangle()
        {
            return new Rectangle((int)CenterPosition.X - (int)Width / 2, (int)CenterPosition.Y - (int)Height / 2, (int)Width, (int)Height);
        }

        //Draw the shape to a spritebatch
        public override void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            Rectangle rectangle = new Rectangle((int)CenterPosition.X - (int)Width / 2, (int)CenterPosition.Y - (int)Height / 2, (int)Width, (int)Height);
            //Draws a box around where the rectangle is
            spriteBatch.Draw(Resources.GetTexture("Pixel"), new Rectangle(rectangle.Top, rectangle.Left, 1, rectangle.Height), color);
            spriteBatch.Draw(Resources.GetTexture("Pixel"), new Rectangle(rectangle.Top, rectangle.Right, 1, rectangle.Height), color);
            spriteBatch.Draw(Resources.GetTexture("Pixel"), new Rectangle(rectangle.Top, rectangle.Left, rectangle.Width, 1), color);
            spriteBatch.Draw(Resources.GetTexture("Pixel"), new Rectangle(rectangle.Bottom, rectangle.Left, rectangle.Width, 1), color);
        }
    }
}

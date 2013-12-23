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
    public class EScene : EElement
    {
        protected List<EObject> eobjects;
        protected List<EElement> eelements;
        public EData edgeData;
        public EdgeGame mainGame;
        public EdgeGameDrawTypes DrawType;
        public Color DebugDrawColor;

        public EScene(string id) : base()
        {
            DrawType = EdgeGameDrawTypes.Normal;
            ID = id;
            eobjects = new List<EObject>();
            eelements = new List<EElement>();
        }

        public void setEData(EData data)
        {
            edgeData = data;
        }

        #region UPDATE
        public override void updateElement(EUpdateArgs updateArgs)
        {
            foreach (EElement element in eelements)
            {
                element.Update(updateArgs);
                if (element.IsActive && element.SupportsCollision)
                {
                    element.UpdateCollision(eelements);
                }
            }
        }

        public void addElement(EElement eElement)
        {
            try
            {
                eElement.Texture = edgeData.getTexture(eElement.Data);
                eElement.Font = edgeData.getFont(eElement.Data);
            }
            catch
            { }
            eElement.OnAddToScene(this);
            eelements.Add(eElement);
        }

        public bool RemoveObject(EObject eObject)
        {
            if (eobjects.Contains(eObject)) 
            {
                eobjects.Remove(eObject);
                return true;
            }
            return false;
        }
        public bool RemoveElement(EElement eElement)
        {
            if (eelements.Contains(eElement))
            {
                eelements.Remove(eElement);
                return true;
            }
            return false;
        }

        public void addObject(EObject eObject)
        {
            eobjects.Add(eObject);
        }
        #endregion

        #region DRAW
        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsVisible)
            {
                eelements = eelements.OrderBy(x => x.DrawLayer).ToList();

                foreach (EElement element in eelements)
                {
                    switch (DrawType)
                    {
                        case EdgeGameDrawTypes.Normal:
                            element.Draw(spriteBatch, gameTime);
                            break;
                        case EdgeGameDrawTypes.Debug:
                            //Debug Draw Here
                            if (element.SupportsCollision && element.CollisionBody != null)
                            {
                                switch(element.CollisionBody.Shape.ShapeType)
                                {
                                    case EShapeTypes.circle:
                                        List<Vector2> points = EMath.GetCirclePoints(element.CollisionBody.Shape.CenterPosition, ((EShapeCircle)element.CollisionBody.Shape).Radius);
                                        foreach (Vector2 point in points)
                                        {
                                            mainGame.DrawPixelAt(point, DebugDrawColor);
                                        }
                                        break;
                                    case EShapeTypes.rectangle:
                                        Rectangle rectangle = new Rectangle((int)((EShapeRectangle)element.CollisionBody.Shape).CenterPosition.X - (int)((EShapeRectangle)element.CollisionBody.Shape).Width / 2, (int)((EShapeRectangle)element.CollisionBody.Shape).CenterPosition.Y - (int)((EShapeRectangle)element.CollisionBody.Shape).Height / 2, (int)((EShapeRectangle)element.CollisionBody.Shape).Width, (int)((EShapeRectangle)element.CollisionBody.Shape).Height);
                                        mainGame.DrawLineAt(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Left, rectangle.Bottom), DebugDrawColor);
                                        mainGame.DrawLineAt(new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Right, rectangle.Bottom), DebugDrawColor);
                                        mainGame.DrawLineAt(new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom), DebugDrawColor);
                                        mainGame.DrawLineAt(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top), DebugDrawColor);
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
        }
        #endregion
    }
}

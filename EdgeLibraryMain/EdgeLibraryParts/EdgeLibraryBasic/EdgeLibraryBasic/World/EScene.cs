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
                eElement.FillTexture(edgeData);
            }
            catch
            { }
            eElement.OnAddToScene(this);
            eelements.Add(eElement);
        }

        public Texture2D GetTexture(string texture)
        {
            return edgeData.getTexture(texture);
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
                                element.CollisionBody.Shape.DebugDraw(spriteBatch, DebugDrawColor);
                            }
                            break;
                        case EdgeGameDrawTypes.Hybrid:
                            element.Draw(spriteBatch, gameTime);
                            if (element.SupportsCollision && element.CollisionBody != null)
                            {
                                element.CollisionBody.Shape.DebugDraw(spriteBatch, DebugDrawColor);
                            }
                            break;
                    }
                }
            }
        }
        #endregion
    }
}

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
    public class ELayer : EElement
    {
        protected List<EObject> eobjects;
        protected List<EElement> eelements;
        public EData edgeData;

        public ELayer(string id) : base()
        {
            ID = id;

            eobjects = new List<EObject>();
            eelements = new List<EElement>();
        }

        public void setEData(EData data)
        {
            edgeData = data;
        }

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

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsVisible)
            {
                eelements = eelements.OrderBy(x => x.DrawLayer).ToList();

                foreach (EElement element in eelements)
                {
                    switch (EMath.DrawType)
                    {
                        case EdgeGameDrawTypes.Normal:
                            element.Draw(spriteBatch, gameTime);
                            break;
                        case EdgeGameDrawTypes.Debug:
                            //Debug Draw Here
                            if (element.SupportsCollision && element.CollisionBody != null)
                            {
                                element.CollisionBody.Shape.DebugDraw(spriteBatch, EMath.DebugDrawColor);
                            }
                            break;
                        case EdgeGameDrawTypes.Hybrid:
                            element.Draw(spriteBatch, gameTime);
                            if (element.SupportsCollision && element.CollisionBody != null)
                            {
                                element.CollisionBody.Shape.DebugDraw(spriteBatch, EMath.DebugDrawColor);
                            }
                            break;
                    }
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
            eElement.OnAddToLayer(this);
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
    }
}

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
    //Like an EElement that contains other EElements in an EScene, optional
    public class ELayer : EElement
    {
        protected List<EObject> eobjects;
        protected List<EElement> eelements;

        public ELayer(string id) : base()
        {
            ID = id;

            eobjects = new List<EObject>();
            eelements = new List<EElement>();
        }
        public override void updateElement(EUpdateArgs updateArgs)
        {
            for (int i = 0; i < eelements.Count; i++ )
            {
                eelements[i].Update(updateArgs);
                if (eelements[i].IsActive && eelements[i].SupportsCollision)
                {
                    eelements[i].UpdateCollision(eelements);
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
                    switch (EdgeGame.DrawType)
                    {
                        case EdgeGameDrawTypes.Normal:
                            element.Draw(spriteBatch, gameTime);
                            break;
                        case EdgeGameDrawTypes.Debug:
                            //Debug Draw Here
                            if (element.SupportsCollision && element.CollisionBody != null)
                            {
                                element.CollisionBody.Shape.DebugDraw(spriteBatch, EdgeGame.DebugDrawColor);
                            }
                            break;
                        case EdgeGameDrawTypes.Hybrid:
                            element.Draw(spriteBatch, gameTime);
                            if (element.SupportsCollision && element.CollisionBody != null)
                            {
                                element.CollisionBody.Shape.DebugDraw(spriteBatch, EdgeGame.DebugDrawColor);
                            }
                            break;
                    }
                }
            }
        }

        public void addElement(EElement eElement)
        {
                eElement.LayerID = ID;
                eElement.SceneID = SceneID;
                eElement.FillTexture();
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
                eElement.Texture = null;
                eElement.CollisionBody = null;
                eElement.SupportsCollision = false;
                eElement.ID = "deletedElement";
                return true;
            }
            return false;
        }

        public void addObject(EObject eObject)
        {
            eObject.LayerID = ID;
            eObject.SceneID = SceneID;
            eobjects.Add(eObject);
        }
    }
}

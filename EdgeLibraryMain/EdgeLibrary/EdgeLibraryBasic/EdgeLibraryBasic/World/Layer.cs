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
    //Like an Element that contains other Elements in an Scene, optional
    public class Layer : Element
    {
        protected List<Object> Objects;
        protected List<Element> Elements;

        public Layer(string id) : base()
        {
            ID = id;

            Objects = new List<Object>();
            Elements = new List<Element>();
        }
        public override void updatElement(UpdateArgs updateArgs)
        {
            for (int i = 0; i < Elements.Count; i++ )
            {
                Elements[i].Update(updateArgs);
                if (Elements[i].IsActive && Elements[i].SupportsCollision)
                {
                    Elements[i].UpdateCollision(Elements);
                }
            }
        }

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsVisible)
            {
                Elements = Elements.OrderBy(x => x.DrawLayer).ToList();

                foreach (Element element in Elements)
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

        public void addElement(Element Element)
        {
           Element.LayerID = ID;
           Element.SceneID = SceneID;
           Element.FillTexture();
           Elements.Add(Element);
        }

        public bool RemovObject(Object Object)
        {
            if (Objects.Contains(Object))
            {
                Objects.Remove(Object);
                return true;
            }
            return false;
        }
        public bool RemovElement(Element Element)
        {
            if (Elements.Contains(Element))
            {
                Elements.Remove(Element);
                Element.Texture = null;
                Element.CollisionBody = null;
                Element.SupportsCollision = false;
                Element.ID = "deletedElement";
                return true;
            }
            return false;
        }

        public void addObject(Object Object)
        {
            Object.LayerID = ID;
            Object.SceneID = SceneID;
            Objects.Add(Object);
        }
    }
}

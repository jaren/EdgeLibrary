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

        public EScene(string id) : base()
        {
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
            try
            {
                foreach (EElement element in eelements)
                {
                    element.Update(updateArgs);
                }
            }
            catch { }
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

        public void RemoveObject(EObject eObject)
        {
            if (eobjects.Contains(eObject)) 
            {
                eobjects.Remove(eObject);
                eObject = null;
            }
        }
        public void RemoveElement(EElement eElement)
        {
            if (eelements.Contains(eElement))
            {
                eelements.Remove(eElement);
                eElement = null;
            }
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
                    element.Draw(spriteBatch, gameTime);
                }
            }
        }
        #endregion
    }
}

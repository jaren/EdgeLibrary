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
        public List<ELayer> layers;

        public EScene(string id) : base()
        {
            ID = id;

            layers = new List<ELayer>();
        }

        #region UPDATE
        public override void updateElement(EUpdateArgs updateArgs)
        {
            foreach (ELayer layer in layers)
            {
                layer.updateElement(updateArgs);
            }
        }

        public void AddLayer(ELayer layer)
        {
            layer.SceneID = ID;
            layer.LayerID = layer.ID;
            layers.Add(layer);
        }

        public ELayer getLayer(string layerName)
        {
            foreach (ELayer layer in layers)
            {
                if (layer.ID == layerName)
                {
                    return layer;
                }
            }
            return null;
        }

        public Texture2D GetTexture(string texture)
        {
            return EData.getTexture(texture);
        }

        public void RemoveElement(EElement eElement)
        {
            foreach (ELayer layer in layers)
            {
                layer.RemoveElement(eElement);
            }
        }

        public void RemoveObject(EObject eObject)
        {
            foreach (ELayer layer in layers)
            {
                layer.RemoveObject(eObject);
            }
        }
        #endregion

        #region DRAW
        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsVisible)
            {
                layers = layers.OrderBy(x => x.DrawLayer).ToList();
                foreach (ELayer layer in layers)
                {
                    layer.drawElement(spriteBatch, gameTime);
                }
            }
        }
        #endregion
    }
}

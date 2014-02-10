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
    public class Scene : Element
    {
        public List<Layer> layers;
        public Layer mainLayer;

        public Scene(string id) : base()
        {
            ID = id;

            mainLayer = new Layer("main");
            layers = new List<Layer>();
            layers.Add(mainLayer);
        }

        #region UPDATE
        public override void updatElement(UpdateArgs updateArgs)
        {
            foreach (Layer layer in layers)
            {
                layer.updatElement(updateArgs);
            }
        }

        public void AddLayer(Layer layer)
        {
            layer.SceneID = ID;
            layer.LayerID = layer.ID;
            layers.Add(layer);
        }

        public Layer getLayer(string layerName)
        {
            foreach (Layer layer in layers)
            {
                if (layer.ID == layerName)
                {
                    return layer;
                }
            }
            return null;
        }

        public void addElement(Element Element)
        {
            mainLayer.addElement(Element);
        }

        public void addObject(Object Object)
        {
            mainLayer.addObject(Object);
        }

        public Texture2D GetTexture(string texture)
        {
            return ResourceData.getTexture(texture);
        }

        public void RemovElement(Element Element)
        {
            foreach (Layer layer in layers)
            {
                layer.RemovElement(Element);
            }
        }

        public void RemovObject(Object Object)
        {
            foreach (Layer layer in layers)
            {
                layer.RemovObject(Object);
            }
        }
        #endregion

        #region DRAW
        public override void drawElement(GameTime gameTime)
        {
            if (IsVisible)
            {
                layers = layers.OrderBy(x => x.DrawLayer).ToList();
                foreach (Layer layer in layers)
                {
                    layer.drawElement(gameTime);
                }
            }
        }
        #endregion
    }
}

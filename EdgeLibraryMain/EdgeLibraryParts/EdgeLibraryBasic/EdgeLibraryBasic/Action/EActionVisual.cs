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

namespace EdgeLibrary.Basic
{
    [Flags]
    public enum EdgeActionVisualTypes : byte
    {
        Color = 1,
        Size = 2,
        Rotation = 4,
        All = 7
    }

    public class EActionVisual : EAction
    {
        public Color color;
        public Vector2 size;
        public float rotation;

        EdgeActionVisualTypes actionType;

        public EActionVisual(EActionVisual action)
        {
            color = action.color;
            size = action.size;
            rotation = action.rotation;
            actionType = action.actionType;
        }

        public EActionVisual(EdgeActionVisualTypes type)
        {
            actionType = type;
            initVars();
        }

        public EActionVisual(Color eColor, Vector2 eSize, float eRotation)
            : this(EdgeActionVisualTypes.All)
        {
            color = eColor;
            size = eSize;
            rotation = eRotation;
            initVars();
        }

        public EActionVisual(Color eColor)
            : this(EdgeActionVisualTypes.Color)
        {
            color = eColor;
            initVars();
        }

        public EActionVisual(Vector2 eSize)
            : this(EdgeActionVisualTypes.Size)
        {
            size = eSize;
            initVars();
        }

        public EActionVisual(float eRotation)
            : this(EdgeActionVisualTypes.Rotation)
        {
            rotation = eRotation;
            initVars();
        }

        protected void initVars()
        {
            requiresUpdate = false;
        }

        public override void initWithSprite(ESprite sprite)
        {
            if (actionType.HasFlag(EdgeActionVisualTypes.Rotation))
            {
                sprite.Rotation = rotation;
            }
            if (actionType.HasFlag(EdgeActionVisualTypes.Color))
            {
                sprite.Color = color;
            }
            if (actionType.HasFlag(EdgeActionVisualTypes.Size))
            {
                sprite.Scale = size;
            }

        }
    }
}

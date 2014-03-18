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

namespace EdgeLibrary
{
    public class StyleCapability : Capability
    {
        private bool colorChanging;
        private bool usingColorIndex;
        private bool rotating;

        private Color color1;
        private Color color2;
        private ColorChangeIndex colorIndex;
        private float colorChangeTime;
        private float elapsedColorChangeTime;

        private float angleAdd;
        private bool rotateToPoint;
        private Vector2 rotateTarget;
        private Element activeRotateTarget;

        public delegate void StyleColorEvent(StyleCapability capability, Color finishColor);
        public event StyleColorEvent FinishedColorChange;

        public StyleCapability() : base("Style")
        {
            colorChanging = false;
            rotating = false;

            color1 = Color.White;
            color2 = Color.White;
            colorChangeTime = 0;
            elapsedColorChangeTime = 0;

            angleAdd = 0;
            rotateToPoint = true;
            rotateTarget = Vector2.Zero;
            activeRotateTarget = null;
        }

        public override void updateCapability(GameTime gameTime, Element element)
        {
            if (element is Sprite)
            {
                Sprite sprite = (Sprite)element;

                if (colorChanging)
                {
                    if (usingColorIndex)
                    {
                        sprite.Style.Color = colorIndex.Update(gameTime);
                    }
                    else
                    {
                        elapsedColorChangeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (elapsedColorChangeTime > colorChangeTime)
                        {
                            colorChanging = false;

                            sprite.Style.Color = Color.Lerp(color1, color2, 1);

                            if (FinishedColorChange != null)
                            {
                                FinishedColorChange(this, color2);
                            }
                        }
                        else
                        {
                            sprite.Style.Color = Color.Lerp(color1, color2, elapsedColorChangeTime / colorChangeTime);
                        }
                    }
                }
                if (rotating)
                {
                    if (rotateToPoint)
                    {
                        sprite.Style.Rotation = MathHelper.ToDegrees((float)Math.Atan2(sprite.Position.Y - rotateTarget.Y, sprite.Position.X - rotateTarget.X)) + angleAdd;
                    }
                    else
                    {
                        sprite.Style.Rotation = MathHelper.ToDegrees((float)Math.Atan2(sprite.Position.Y - activeRotateTarget.Position.Y, sprite.Position.X - activeRotateTarget.Position.X)) + angleAdd;
                    }
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void ColorChange(Color color, Color nextColor, float time)
        {
            color1 = color;
            color2 = nextColor;
            colorChangeTime = time;
            elapsedColorChangeTime = 0;
            usingColorIndex = false;
            colorChanging = true;
        }

        public void ColorChange(ColorChangeIndex index)
        {
            colorIndex = index;
            usingColorIndex = true;
            colorChanging = true;
        }

        public void Rotate(Vector2 target, float addAngle)
        {
            angleAdd = addAngle;
            rotateTarget = target;
            rotateToPoint = true;
            rotating = true;
        }

        public void RotateSpriteTowards(Sprite sprite, Vector2 target, float addAngle)
        {
            sprite.Style.Rotation = MathHelper.ToDegrees((float)Math.Atan2(sprite.Position.Y - target.Y, sprite.Position.X - target.X)) + angleAdd;
        }

        public void RotateSpriteTowards(Sprite sprite, Element target, float addAngle)
        {
            sprite.Style.Rotation = MathHelper.ToDegrees((float)Math.Atan2(sprite.Position.Y - target.Position.Y, sprite.Position.X - target.Position.X)) + angleAdd;
        }

        public void Rotate(Element target, float addAngle)
        {
            angleAdd = addAngle;
            activeRotateTarget = target;
            rotateToPoint = false;
            rotating = true;
        }

        public void StopRotating()
        {
            rotating = false;
        }

        public void StopColorChanging()
        {
            colorChanging = false;
        }
    }
}

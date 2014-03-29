﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EdgeLibrary;

namespace EdgeLibrary.Platform
{
    //A platform sprite which WILL NOT MOVE unless the position is manually changed
    public class PlatformStatic : PlatformSprite
    {
        public new event CollisionEvent Collision;

        public PlatformStatic(string eTextureName, Vector2 ePosition) : this(MathTools.RandomID(typeof(PlatformStatic)), eTextureName, ePosition) { }
        public PlatformStatic(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition) { }

        //Doesn't move in collisions
        protected override void UpdateCollision(GameTime gameTime)
        {
            CollisionBody.ScaleWith(this);
            if (_centerAsOrigin)
            {
                CollisionBody.Position = new Vector2(Position.X, Position.Y);
            }
            else
            {
                CollisionBody.Position = new Vector2(Position.X + Width / 2, Position.Y + Height / 2);
            }

            foreach (Element element in EdgeGame.SelectedScene.elements)
            {
                if (element is PlatformSprite)
                {
                    PlatformSprite sprite = (PlatformSprite)element;
                    if (sprite != this && CollisionBody != null && sprite.CollisionBody != null && CollisionBody.CheckForCollide(sprite.CollisionBody))
                    {
                        if (Collision != null)
                        {
                            Collision(this, sprite, gameTime);
                        }
                        DebugLogger.LogEvent("Collision", "Sprite 1:" + ID, "Sprite 2:" + sprite.ID, "GameTime:" + gameTime.TotalGameTime.ToString());
                    }
                }
            }
        }

        //Isn't affected by any forces
        public override void UpdateForces() { }
    }
}
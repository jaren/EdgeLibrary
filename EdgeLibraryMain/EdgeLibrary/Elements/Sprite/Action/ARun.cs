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

namespace EdgeLibrary
{
    //Runs a function
    public class ARun : AAction
    {
        public delegate void ARunEvent(Sprite sprite, GameTime gameTime);
        public ARunEvent OnRun = delegate { };

        public ARun(ARunEvent runEvent)
        {
            OnRun += runEvent;
        }

        //Called once then is removed
        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            OnRun(sprite, gameTime);
            Stop(gameTime, sprite);
        }

        public override AAction Copy()
        {
            return new ARun(OnRun);
        }
    }
}
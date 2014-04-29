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
    //Calls an event when run
    public class ARun3D : Action3D
    {
        public delegate void ARunEvent(Sprite3D sprite, GameTime gameTime);
        public ARunEvent OnRun = delegate { };

        public ARun3D(ARunEvent runEvent)
            : base()
        {
            OnRun += runEvent;
        }

        //Called once then is removed
        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            OnRun(sprite, gameTime);
            Stop(gameTime, sprite);
        }

        public override Action3D Clone()
        {
            return new ARun3D(OnRun);
        }
    }
}

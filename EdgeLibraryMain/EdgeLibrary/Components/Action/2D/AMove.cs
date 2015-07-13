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
    //Adds a vector to a sprite's position
    public class AMove : Action
    {
        public Vector2 MoveVector;

        public AMove(Vector2 moveVector) : base()
        {
            MoveVector = moveVector;
        }

        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            sprite.Position += MoveVector * EdgeGame.GameSpeed;
        }

        public override Action SubClone()
        {
            return new AMove(MoveVector);
        }
    }
}

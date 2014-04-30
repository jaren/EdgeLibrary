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
    //Adds a vector to a sprite's position every frame
    public class AMove3D : Action3D
    {
        public Vector3 MoveVector;

        public AMove3D(Vector3 moveVector)
        {
            MoveVector = moveVector;
        }

        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            sprite.Position += MoveVector * EdgeGame.GameSpeed;
        }

        public override Action3D Clone()
        {
            return new AMove3D(MoveVector);
        }
    }
}

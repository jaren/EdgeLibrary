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
    //Clamps a sprite to another sprite's position, adding a Vector3 to it
    public class AClamp3D : Action3D
    {
        public Sprite3D Target;
        public Vector3 AddPosition;

        public AClamp3D(Sprite3D target)
        {
            Target = target;
            AddPosition = Vector3.Zero;
        }

        public AClamp3D(Sprite3D target, Vector3 addPosition)
        {
            Target = target;
            AddPosition = addPosition;
        }

        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            sprite.Position = Target.Position + AddPosition;
        }

        public override Action3D Clone()
        {
            return new AClamp3D(Target, AddPosition);
        }
    }
}

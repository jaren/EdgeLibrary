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
    //Clamps a sprite to an element
    public class AClamp : Action
    {
        public Sprite Target;
        public Vector2 AddPosition;

        public AClamp(Sprite target)
            : base()
        {
            Target = target;
            AddPosition = Vector2.Zero;
        }

        public AClamp(Sprite target, Vector2 addPosition)
            : base()
        {
            Target = target;
            AddPosition = addPosition;
        }
        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            sprite.Position = Target.Position + AddPosition;
        }

        public override Action SubClone()
        {
            return new AClamp(Target, AddPosition);
        }
    }
}

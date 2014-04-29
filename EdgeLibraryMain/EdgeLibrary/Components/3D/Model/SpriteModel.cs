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
    public class SpriteModel : ICloneable
    {
        public Matrix World
        {
            get { return Effect.World; }
            set
            { Effect.World = value; }
        }
        protected BasicEffect Effect;

        public SpriteModel()
        {
            Effect = new BasicEffect(EdgeGame.Game.GraphicsDevice);
            Effect.VertexColorEnabled = true;
            Effect.Projection = EdgeGame.Camera3D.Projection;
            Effect.View = EdgeGame.Camera3D.View;
        }

        public virtual void Update(GameTime gameTime) { Effect.View = EdgeGame.Camera3D.View; Effect.Projection = EdgeGame.Camera3D.Projection; }
        public virtual void Draw(GameTime gameTime) { }

        public virtual object Clone()
        {
            SpriteModel clone = (SpriteModel)MemberwiseClone();
            clone.Effect = new BasicEffect(EdgeGame.Game.GraphicsDevice);
            clone.Effect.Projection = EdgeGame.Camera3D.Projection;
            clone.Effect.View = EdgeGame.Camera3D.View;
            return clone;
        }
    }
}

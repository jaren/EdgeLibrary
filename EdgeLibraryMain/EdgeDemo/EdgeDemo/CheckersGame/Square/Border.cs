using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class Border : Sprite
    {
        float Size;
        float BorderWidth;
        Rectangle leftRectangle;
        Rectangle rightRectangle;
        Rectangle topRectangle;
        Rectangle bottomRectangle;

        public Border(string texture, Vector2 position, float borderWidth, float size, Color color) : base(texture, position)
        {
            Size = size;
            BorderWidth = borderWidth;
            Color = color;

            Vector2 topLeft = new Vector2(position.X - size/2, position.Y - size/2);
            Vector2 topRight = new Vector2(position.X + size / 2, position.Y - size / 2);
            Vector2 bottomLeft = new Vector2(position.X - size / 2, position.Y + size / 2);
            Vector2 bottomRight = new Vector2(position.X + size / 2, position.Y + size / 2);

            leftRectangle = new Rectangle((int)(position.X - size / 2 - borderWidth), (int)(position.Y - size / 2 - borderWidth), (int)borderWidth, (int)(size + borderWidth*2));
            rightRectangle = new Rectangle((int)(position.X + size / 2), (int)(position.Y - size / 2 - borderWidth), (int)borderWidth, (int)(size + borderWidth * 2));
            topRectangle = new Rectangle((int)(position.X - size / 2 - borderWidth), (int)(position.Y - size / 2 - borderWidth), (int)(size + borderWidth * 2), (int)borderWidth);
            bottomRectangle = new Rectangle((int)(position.X - size / 2 - borderWidth), (int)(position.Y + size / 2), (int)(size + borderWidth * 2), (int)borderWidth);
        }

        public override void Draw(GameTime gameTime)
        {
            EdgeGame.Game.SpriteBatch.Draw(Texture, leftRectangle, Color);
            EdgeGame.Game.SpriteBatch.Draw(Texture, rightRectangle, Color);
            EdgeGame.Game.SpriteBatch.Draw(Texture, topRectangle, Color);
            EdgeGame.Game.SpriteBatch.Draw(Texture, bottomRectangle, Color);
        }
    }
}

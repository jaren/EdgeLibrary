using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace CheckersGame
{
    public class Piece : Sprite
    {
        public bool King;
        public bool ShowX;
        public bool Player1;
        public bool Fake;
        public string BoardID;

        public float Size
        {
            set
            {
                Scale = new Vector2(value / Width, value / Height);
            }
        }

        public int X;
        public int Y;

        public Vector2 Location()
        {
            return new Vector2(X, Y);
        }

        public Piece(string textureName, Vector2 position, Color color, float size, bool topTeam)
            : base(textureName, position)
        {
            Color = color;
            Size = size;
            Player1 = topTeam;
            King = false;
            ShowX = false;
            Fake = false;
        }

        public override void DrawObject(GameTime gameTime)
        {
            base.DrawObject(gameTime);

            if (King)
            {
                RestartSpriteBatch();

                EdgeGame.Game.SpriteBatch.Draw(EdgeGame.GetTexture(Config.KingTexture), Position, null, Color, Rotation, OriginPoint, ScaleWithCamera ? Scale / EdgeGame.Camera.Scale : Scale, SpriteEffects, 0);

                RestartSpriteBatch();
            }
            if (ShowX)
            {
                RestartSpriteBatch();

                EdgeGame.Game.SpriteBatch.Draw(EdgeGame.GetTexture(Config.XTexture), Position, null, Color.White, Rotation, OriginPoint, ScaleWithCamera ? Scale / EdgeGame.Camera.Scale : Scale, SpriteEffects, 0);

                RestartSpriteBatch();
            }
        }

        public override void UpdateObject(GameTime gameTime)
        {
            base.UpdateObject(gameTime);
        }
    }
}

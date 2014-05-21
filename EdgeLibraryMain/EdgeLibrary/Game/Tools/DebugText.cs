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
    public class DebugText : TextSprite
    {
        public DebugText(string fontName, Vector2 position) : base(fontName, "", position) { CenterAsOrigin = false; }

        public override void Update(GameTime gameTime)
        {
            Text = "Mouse Position: (" + Input.MousePosition.X.ToString() + ", " + Input.MousePosition.Y.ToString() + ")\n";
            Text += "2D Camera Position: " + EdgeGame.Camera.Position.ToString() + "\n";
            Text += "3D Camera Position: " + EdgeGame.Camera3D.Position.ToString() + "\n";
            Text += "3D Camera Looking At: " + EdgeGame.Camera3D.Target.ToString() + "\n";
            Text += "FPS: " + EdgeGame.FPS.ToString();
            base.Update(gameTime);
        }
    }
}

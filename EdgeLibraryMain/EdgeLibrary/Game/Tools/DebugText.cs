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
        public bool Include3D
        {
            set
            {
                Include3DCameraPosition = value;
                Include3DCameraLookingAt = value;
            }
        }

        public bool Include2D
        {
            set
            {
                Include2DCameraPosition = value;
                Include2DCameraScale = value;
                Include2DCameraRotation = value;
            }
        }

        public bool IncludeFPS = true;
        public bool IncludeMousePosition = true;
        public bool Include2DCameraPosition = true;
        public bool Include2DCameraScale = true;
        public bool Include2DCameraRotation = true;
        public bool Include3DCameraPosition = true;
        public bool Include3DCameraLookingAt = true;

        public DebugText(string fontName, Vector2 position) : base(fontName, "", position) { CenterAsOrigin = false; }

        public override void Update(GameTime gameTime)
        {
            if (IncludeFPS)
            { Text = "FPS: " + EdgeGame.FPS.ToString() + "\n"; }

            if (IncludeMousePosition)
            { Text += "Mouse Position: (" + Input.MousePosition.X.ToString() + ", " + Input.MousePosition.Y.ToString() + ")\n"; }

            if (Include2DCameraPosition)
            { Text += "2D Camera Position: " + EdgeGame.Camera.Position.ToString() + "\n"; }

            if (Include2DCameraScale)
            { Text += "2D Camera Scale: " + EdgeGame.Camera.Scale + "\n"; }

            if (Include2DCameraRotation)
            { Text += "2D Camera Rotation: " + EdgeGame.Camera.Rotation.ToString() + "\n"; }

            if (Include3DCameraPosition)
            { Text += "3D Camera Position: " + EdgeGame.Camera3D.Position.ToString() + "\n"; }

            if (Include3DCameraLookingAt)
            { Text += "3D Camera Looking At: " + EdgeGame.Camera3D.Target.ToString() + "\n"; }

            base.Update(gameTime);
        }
    }
}

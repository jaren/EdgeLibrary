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

        protected override void UpdateObject(GameTime gameTime)
        {
            _text = "Mouse Position: (" + Input.MousePosition.X.ToString() + ", " + Input.MousePosition.Y.ToString() + ")\n";
            _text += "FPS: " + EdgeGame.GetCurrentGame().FPS.ToString();
            _text += "Elements in current scene (" + EdgeGame.GetCurrentScene().Elements.Count.ToString() + "): ";
            foreach (Element element in EdgeGame.GetCurrentScene().Elements)
            {
                _text += element.ID + ", ";
            }
            if (EdgeGame.GetCurrentScene().Elements.Count > 0)
            {
                _text.Remove(_text.Length - 2);
            }
            _text += "\nScenes in current game (" + EdgeGame.GetCurrentGame().SceneHandler.Scenes.Count.ToString() + "): ";
            foreach (Scene scene in EdgeGame.GetCurrentGame().SceneHandler.Scenes)
            {
                _text += scene.ID + ", ";
            }
            if (EdgeGame.GetCurrentGame().SceneHandler.Scenes.Count > 0)
            {
                _text.Remove(_text.Length - 2);
            }
            base.UpdateObject(gameTime);
        }
    }
}

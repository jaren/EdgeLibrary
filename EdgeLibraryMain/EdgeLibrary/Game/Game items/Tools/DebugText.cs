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
            _text += "FPS: " + EdgeGame.Instance.FPS.ToString();
            _text += "\nElements in current scene (" + EdgeGame.Instance.SceneHandler.CurrentScene.Elements.Count.ToString() + "): ";
            foreach (Element element in EdgeGame.Instance.SceneHandler.CurrentScene.Elements)
            {
                _text += element.ID + ", ";
            }
            if (EdgeGame.Instance.SceneHandler.CurrentScene.Elements.Count > 0)
            {
                _text.Remove(_text.Length - 2);
            }
            _text += "\nScenes in current game (" + EdgeGame.Instance.SceneHandler.Scenes.Count.ToString() + "): ";
            foreach (Scene scene in EdgeGame.Instance.SceneHandler.Scenes)
            {
                _text += scene.ID + ", ";
            }
            if (EdgeGame.Instance.SceneHandler.Scenes.Count > 0)
            {
                _text.Remove(_text.Length - 2);
            }
            reloadOriginPoint();
            base.UpdateObject(gameTime);
        }
    }
}

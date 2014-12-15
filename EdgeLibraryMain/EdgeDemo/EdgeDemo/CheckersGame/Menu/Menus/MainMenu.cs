using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using FarseerPhysics.Factories;

namespace EdgeDemo.CheckersGame
{
    public class MainMenu : MenuBase
    {
        public MainMenu() : base("MainMenu")
        {
            DebugText debug = new DebugText("Impact-20", Vector2.Zero) { CenterAsOrigin = false };
            //Components.Add(debug);

            TextSprite title = new TextSprite("Georgia-50", "Checkers Game", new Vector2(500, 50));
            Components.Add(title);

            Button button = new Button("ShadedDark42", new Microsoft.Xna.Framework.Vector2(500)) { ClickTexture = EdgeGame.GetTexture("TransparentDark40"), MouseOverTexture = EdgeGame.GetTexture("FlatDark41"), Scale = new Vector2(1) };
            button.OnRelease += button_OnClick;
            Components.Add(button);

            Sprite physicsSprite = new Sprite("Checkers", new Vector2(500)) { Scale = new Vector2(0.5f) };
            physicsSprite.EnablePhysics(BodyFactory.CreateCircle(EdgeGame.World, physicsSprite.Width, 1));
            Components.Add(physicsSprite);
        }

        void button_OnClick(Button sender, Microsoft.Xna.Framework.GameTime gameTime)
        {
            MenuManager.SwitchMenu("GameMenu");
        }

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = Color.Gray;
            base.SwitchTo();
        }
    }
}

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
        Sprite physicsSprite;
        Sprite physicsSprite2;

        public MainMenu() : base("MainMenu")
        {
            DebugText debug = new DebugText("Impact-20", Vector2.Zero) { CenterAsOrigin = false };
            //Components.Add(debug);

            Input.OnKeyRelease += Input_OnKeyRelease;

            TextSprite title = new TextSprite("Georgia-50", "Checkers Game", new Vector2(500, 50));
            Components.Add(title);

            Button screenButton = new Button("Pixel", new Vector2(500)) { Visible = false, Scale = new Vector2(1000) };
            screenButton.OnClick += screenButton_OnClick;
            Components.Add(screenButton);

            Button button = new Button("ShadedDark42", new Microsoft.Xna.Framework.Vector2(500)) { ClickTexture = EdgeGame.GetTexture("TransparentDark40"), MouseOverTexture = EdgeGame.GetTexture("FlatDark41"), Scale = new Vector2(1) };
            button.OnRelease += button_OnClick;
            Components.Add(button);

            physicsSprite = new Sprite("Checkers", new Vector2(500)) { Scale = new Vector2(0.5f), Color = Color.Red };
            physicsSprite.EnablePhysics(BodyFactory.CreateCircle(EdgeGame.World, (physicsSprite.Width*physicsSprite.Scale.X/2f).ToSimUnits(), 1));
            physicsSprite.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
            physicsSprite.Body.Restitution = 0.5f;
            float max = 500;
            Vector2 force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            Vector2 point = physicsSprite.Position;
            physicsSprite.Body.ApplyForce(ref force, ref point);
            Components.Add(physicsSprite);

            physicsSprite2 = (Sprite)physicsSprite.Clone();
            physicsSprite2.Color = Color.Black;
            Components.Add(physicsSprite2);

            Sprite bottom = new Sprite("Pixel", new Vector2(500, 1000)) { Scale = new Vector2(1000, 10), Color = Color.White };
            bottom.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (bottom.Width * bottom.Scale.X).ToSimUnits(), (bottom.Height * bottom.Scale.Y).ToSimUnits(), 1));
            bottom.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(bottom);

            Sprite left = new Sprite("Pixel", new Vector2(0, 500)) { Scale = new Vector2(10, 1000), Color = Color.White };
            left.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (left.Width * left.Scale.X).ToSimUnits(), (left.Height * left.Scale.Y).ToSimUnits(), 1));
            left.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(left);

            Sprite right = new Sprite("Pixel", new Vector2(1000, 500)) { Scale = new Vector2(10, 1000), Color = Color.White };
            right.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (right.Width * right.Scale.X).ToSimUnits(), (right.Height * right.Scale.Y).ToSimUnits(), 1));
            right.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(right);

            Sprite top = new Sprite("Pixel", new Vector2(500, 0)) { Scale = new Vector2(1000, 10), Color = Color.White };
            top.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (bottom.Width * top.Scale.X).ToSimUnits(), (top.Height * top.Scale.Y).ToSimUnits(), 1));
            top.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(top);
        }

        void Input_OnKeyRelease(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (key == Config.BackKey)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to quit?", "Quit", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    EdgeGame.Stop();
                }
                else
                {

                }
            }
        }

        void screenButton_OnClick(Button sender, GameTime gameTime)
        {
            float max = 1000;
            Vector2 force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            Vector2 point = physicsSprite.Position;
            physicsSprite.Body.ApplyForce(ref force, ref point);

            force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            point = physicsSprite2.Position;
            physicsSprite2.Body.ApplyForce(ref force, ref point);
        }

        void button_OnClick(Button sender, Microsoft.Xna.Framework.GameTime gameTime)
        {
            MenuManager.SwitchMenu("GameMenu");
        }

        public override void Update(GameTime gameTime)
        {
            physicsSprite.Rotation = 0;
            physicsSprite2.Rotation = 0;
            base.Update(gameTime);
        }

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = Color.Gray;
            base.SwitchTo();
        }
    }
}

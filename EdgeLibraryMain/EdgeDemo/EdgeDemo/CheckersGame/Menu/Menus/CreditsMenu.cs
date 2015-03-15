using EdgeLibrary;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class CreditsMenu : MenuBase
    {
        ParticleEmitter emitter;

        public CreditsMenu() : base("CreditsMenu")
        {
            emitter = new ParticleEmitter("Fire", EdgeGame.WindowSize / 2)
            {
                MinEmitWait = 10,
                MaxEmitWait = 20,

                BlendState = BlendState.Additive,

                MinVelocity = new Vector2(-1.5f),
                MaxVelocity = new Vector2(2.5f),

                GrowSpeed = 0,

                MinColorIndex = new ColorChangeIndex(1000, Color.MediumAquamarine, Color.Purple, Color.OrangeRed, Color.Transparent),
                MaxColorIndex = new ColorChangeIndex(1000, Color.Aquamarine, Color.MediumPurple, Color.DarkOrange, Color.Transparent),

                MinLife = 4000,
                MaxLife = 4500,

                MinScale = new Vector2(2),
                MaxScale = new Vector2(4),

                MaxParticles = 1000
            };
            Components.Add(emitter);

            float max = 100;
            Vector2 force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            Vector2 point = Vector2.One;

            TextSprite credits = new TextSprite(Config.MenuTitleFont, "Credit 1\n\nCredit 2\n\nCredit 3\n\nCredit 4", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.35f));
            Components.Add(credits);

            Sprite bottom = new Sprite("Pixel", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y)) { Visible = false, Scale = new Vector2(EdgeGame.WindowSize.X, 10), Color = Color.White };
            bottom.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (bottom.Width * bottom.Scale.X).ToSimUnits(), (bottom.Height * bottom.Scale.Y).ToSimUnits(), 1));
            bottom.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(bottom);

            Sprite left = new Sprite("Pixel", new Vector2(0, EdgeGame.WindowSize.Y / 2)) { Visible = false, Scale = new Vector2(10, EdgeGame.WindowSize.Y), Color = Color.White };
            left.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (left.Width * left.Scale.X).ToSimUnits(), (left.Height * left.Scale.Y).ToSimUnits(), 1));
            left.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(left);

            Sprite right = new Sprite("Pixel", new Vector2(EdgeGame.WindowSize.X, EdgeGame.WindowSize.Y / 2)) { Visible = false, Scale = new Vector2(10, EdgeGame.WindowSize.Y), Color = Color.White };
            right.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (right.Width * right.Scale.X).ToSimUnits(), (right.Height * right.Scale.Y).ToSimUnits(), 1));
            right.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(right);

            Sprite top = new Sprite("Pixel", new Vector2(EdgeGame.WindowSize.X / 2, 0)) { Visible = false, Scale = new Vector2(EdgeGame.WindowSize.X, 10), Color = Color.White };
            top.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (bottom.Width * top.Scale.X).ToSimUnits(), (top.Height * top.Scale.Y).ToSimUnits(), 1));
            top.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(top);

            TextSprite title = new TextSprite(Config.MenuTitleFont, "Credits", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Click!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            Button returnButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.9f)) { Color = Config.MenuButtonColor, Scale = new Vector2(0.8f) };
            returnButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            returnButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            returnButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            returnButton.Style.AllColors = Config.MenuButtonColor;
            returnButton.OnRelease += (x, y) =>
            {
                MenuManager.SwitchMenu("MainMenu");
            };
            Components.Add(returnButton);

            returnButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (returnButton.Width * returnButton.Scale.X).ToSimUnits(), (returnButton.Height * returnButton.Scale.Y).ToSimUnits(), 1f));
            returnButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            TextSprite returnButtonText = new TextSprite(Config.MenuButtonTextFont, "Return", returnButton.Position);
            Components.Add(returnButtonText);
        }

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = Color.Black;
            base.SwitchTo();
        }
    }
}

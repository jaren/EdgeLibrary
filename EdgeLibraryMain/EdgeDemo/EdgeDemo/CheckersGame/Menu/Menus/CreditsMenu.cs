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
    public class CreditsMenu : ParticleMenu
    {
        TextSprite credit1;
        TextSprite credit2;
        TextSprite credit3;
        TextSprite credit4;
        Button returnButton;

        public CreditsMenu() : base("CreditsMenu")
        {
            TextSprite title = new TextSprite(Config.MenuTitleFont, "Credits", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Click for Checkers! Right Click to Move Them!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            credit1 = new TextSprite(Config.MenuTitleFont, "Credit 1", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.25f));
            Components.Add(credit1);

            credit2 = new TextSprite(Config.MenuTitleFont, "Credit 2", new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.25f));
            Components.Add(credit2);

            credit3 = new TextSprite(Config.MenuTitleFont, "Credit 3", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.75f));
            Components.Add(credit3);

            credit4 = new TextSprite(Config.MenuTitleFont, "Credit 4", new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.75f));
            Components.Add(credit4);

            returnButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.9f)) { Color = Config.MenuButtonColor, Scale = new Vector2(0.8f) };
            returnButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            returnButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            returnButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            returnButton.Style.AllColors = Config.MenuButtonColor;
            returnButton.OnRelease += (x, y) =>
            {
                MenuManager.SwitchMenu("MainMenu");
            };
            Components.Add(returnButton);

            Input.OnKeyRelease += (x) => { if (x == Config.BackKey) { MenuManager.SwitchMenu("MainMenu"); } };

            TextSprite returnButtonText = new TextSprite(Config.MenuButtonTextFont, "Return", returnButton.Position);
            Components.Add(returnButtonText);
        }

        public override void SwitchTo()
        {
            credit1.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (credit1.Width * credit1.Scale.X).ToSimUnits(), (credit1.Height * credit1.Scale.Y).ToSimUnits(), 1f));
            credit1.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            credit2.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (credit2.Width * credit2.Scale.X).ToSimUnits(), (credit2.Height * credit2.Scale.Y).ToSimUnits(), 1f));
            credit2.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            credit3.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (credit3.Width * credit3.Scale.X).ToSimUnits(), (credit3.Height * credit3.Scale.Y).ToSimUnits(), 1f));
            credit3.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            credit4.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (credit4.Width * credit4.Scale.X).ToSimUnits(), (credit4.Height * credit4.Scale.Y).ToSimUnits(), 1f));
            credit4.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            
            returnButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (returnButton.Width * returnButton.Scale.X).ToSimUnits(), (returnButton.Height * returnButton.Scale.Y).ToSimUnits(), 1f));
            returnButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            base.SwitchTo();
        }
    }
}

using EdgeLibrary;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class CreditsMenu : MenuBase
    {
        public CreditsMenu() : base("CreditsMenu")
        {
            ParticleEmitter Fire = new ParticleEmitter("Fire", new Vector2(500))
            {
                //BlendState = BlendState.Additive,
                Life = 4000,

                EmitPositionVariance = new Vector2(10, 10),

                MinVelocity = new Vector2(4, 4),
                MaxVelocity = new Vector2(-5, -5),

                MinScale = new Vector2(1.5f),
                MaxScale = new Vector2(2f),

                MinColorIndex = new ColorChangeIndex(1000, Color.Magenta, Color.Orange, Color.Purple, Color.Transparent),
                MaxColorIndex = new ColorChangeIndex(1000, Color.Teal, Color.OrangeRed, Color.DarkOrange, Color.Transparent),
                EmitWait = 0,
                ParticlesToEmit = 15
            };
            Components.Add(Fire);

            Sprite credits = new Sprite("CreditsTexture", EdgeGame.WindowSize / 2);
            Components.Add(credits);

            Input.OnKeyRelease += (x) =>
            {
                if (MenuManager.SelectedMenu == this && x == Config.BackKey)
                {
                    MenuManager.SwitchMenu("MainMenu");
                }
            };
        }

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = new Color(10, 10, 10, 255);

            base.SwitchTo();
        }

        public override void SwitchOut()
        {
            EdgeGame.ClearColor = Color.Gray;

            base.SwitchOut();
        }
    }
}

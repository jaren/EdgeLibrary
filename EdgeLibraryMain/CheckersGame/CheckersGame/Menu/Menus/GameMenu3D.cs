﻿using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersGame
{
    public class GameMenu3D : MenuBase
    {
        public GameMenu3D() : base("GameMenu")
        {
            Input.OnKeyRelease += (x) =>
            {
                if (MenuManager.SelectedMenu == this && x == Config.BackKey)
                {
                    MenuManager.SwitchMenu("OptionsMenu");
                }
            };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            BoardManager.Instance.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            BoardManager.Instance.Draw(gameTime);
        }

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = Color.Gray;
            if (BoardManager.Instance.ResetGame)
            {
                BoardManager3D.ResetInstance3D();
            }

            base.SwitchTo();
        }
    }
}
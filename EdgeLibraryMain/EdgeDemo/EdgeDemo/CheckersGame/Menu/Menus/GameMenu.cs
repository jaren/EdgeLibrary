using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class GameMenu : MenuBase
    {
        BoardManager manager;

        public GameMenu() : base("GameMenu")
        {
            Input.OnKeyRelease += (x) =>
            {
                if (MenuManager.SelectedMenu == this && x == Config.BackKey)
                {
                    MenuManager.SwitchMenu("OptionsMenu");
                }
            };
        }

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = Color.Gray;
            if (BoardManager.ResetGame)
            {
                if (Components.Contains(manager))
                {
                    Components.Remove(manager);
                }
                manager = new BoardManager();
                Components.Add(manager);
            }
            BoardManager.ResetGame = true;

            base.SwitchTo();
        }
    }
}

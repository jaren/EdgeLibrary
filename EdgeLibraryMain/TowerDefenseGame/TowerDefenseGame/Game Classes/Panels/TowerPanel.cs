using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class TowerPanel : Scene
    {
        public Sprite BackPanel;
        public TextSprite TowerName;
        public TextSprite TowerDescription;
        public Button CloseButton;
        public Button SellButton;
        public TextSprite SellSprite;
        public TextSprite CloseSprite;
        public Sprite TowerPicture;
        public List<Button> UpgradeButtons;
        public List<TextSprite> UpgradeSprites;
        public Tower SelectedTower
        {
            get
            {
                return selectedTower;
            }
            set
            {
                selectedTower = value;
                TowerName.Text = selectedTower.TowerData.Name;

                UpgradeButtons.Clear();
                UpgradeSprites.Clear();

                int upgradeCount = 0;
                foreach (TowerData data in Config.Towers)
                {
                    if (data.BaseName == selectedTower.TowerData.Name)
                    {
                        Button button = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.5f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * (0.45f + 0.1f * upgradeCount))) { ID = String.Format("{0}_Button", data.Name), Color = Config.MenuButtonColor, Scale = new Vector2(2.5f, 1f) };
                        button.Style = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);
                        button.OnRelease += (x, y) =>
                        {
                            if (OnUpgradeTower != null)
                            {
                                OnUpgradeTower(x.ID.Split('_')[0], SelectedTower);
                            }
                        };
                        UpgradeButtons.Add(button);
                        base.Components.Add(button);

                        TextSprite text = new TextSprite(Config.StatusFont, data.Name + " (" + data.Cost + ")", button.Position) { Color = Color.White };
                        UpgradeSprites.Add(text);
                        base.Components.Add(text);

                        upgradeCount++;
                    }
                }
            }
        }
        private Tower selectedTower;

        public delegate void UpgradeEvent(string upgradeId, Tower tower);
        public event UpgradeEvent OnUpgradeTower;

        public TowerPanel()
            : base(new List<Microsoft.Xna.Framework.GameComponent>())
        {
            UpgradeButtons = new List<Button>();
            UpgradeSprites = new List<TextSprite>();

            BackPanel = new Sprite("grey_panel", new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.5f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.6f)) { Color = new Color(20, 20, 20, 175), Scale = new Vector2(5f, 5f) };
            base.Components.Add(BackPanel);

            TowerName = new TextSprite(Config.StatusFont, "", new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.5f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.35f)) { Color = Color.DarkGoldenrod };
            base.Components.Add(TowerName);

            SellButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.35f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.85f)) { Color = Config.MenuButtonColor };
            SellButton.Style = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);
            SellButton.OnRelease += (x, y) => { this.Visible = false; this.Enabled = false; };
            base.Components.Add(SellButton);

            SellSprite = new TextSprite(Config.StatusFont, "Sell", SellButton.Position) { Color = Color.White };
            base.Components.Add(SellSprite);

            CloseButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.65f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.85f)) { Color = Config.MenuButtonColor };
            CloseButton.Style = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);
            CloseButton.OnRelease += (x, y) => { this.Visible = false; this.Enabled = false; };
            base.Components.Add(CloseButton);

            CloseSprite = new TextSprite(Config.StatusFont, "Close", CloseButton.Position) { Color = Color.White };
            base.Components.Add(CloseSprite);

            TowerName = new TextSprite(Config.StatusFont, "", new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.5f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.35f)) { Color = Color.DarkGoldenrod };
            base.Components.Add(TowerName);
        }

        public void Enable(Tower tower)
        {
            SelectedTower = tower;
            Enabled = true;
            Visible = true;
        }
    }
}

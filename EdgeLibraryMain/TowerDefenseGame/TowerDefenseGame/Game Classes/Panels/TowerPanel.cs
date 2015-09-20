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
        public ButtonToggle RangeButton;
        public TextSprite RangeText;
        public ButtonMultiToggle TargetButton;
        public TextSprite TargetText;
        public TextSprite CloseSprite;
        public Sprite TowerPicture;
        public List<Button> UpgradeButtons;
        public List<TextSprite> UpgradeSprites;
        public TextSprite NoUpgradesText;
        public bool ButtonCanClick;
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
                        Button button = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.5f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * (0.3f + 0.1f * upgradeCount))) { ID = String.Format("{0}_Button", data.Name), Color = Config.MenuButtonColor, Scale = new Vector2(2.5f, 1f) };
                        button.Style = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);
                        button.OnRelease += (x, y) =>
                        {
                            if (Visible && ButtonCanClick)
                            {
                                if (OnUpgradeTower != null)
                                {
                                    OnUpgradeTower(x.ID.Split('_')[0], SelectedTower);
                                }
                            }
                        };
                        UpgradeButtons.Add(button);

                        TextSprite text = new TextSprite(Config.StatusFont, data.Name + " (" + data.Cost + ")", button.Position) { Color = Color.White };
                        UpgradeSprites.Add(text);

                        upgradeCount++;
                    }
                }
            }
        }
        private Tower selectedTower;

        public delegate void UpgradeEvent(string upgradeId, Tower tower);
        public event UpgradeEvent OnUpgradeTower;
        public delegate void SellEvent(Tower tower);
        public event SellEvent OnSellTower;
        string[] targetTypes = new string[] { "First", "Last", "Strong", "Weak" };

        public TowerPanel()
            : base(new List<Microsoft.Xna.Framework.GameComponent>())
        {
            UpgradeButtons = new List<Button>();
            UpgradeSprites = new List<TextSprite>();
            Style buttonStyle = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);

            ButtonCanClick = false;

            BackPanel = new Sprite("Pixel", new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.5f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.5f)) { Color = new Color(40,40,40), Scale = new Vector2(500f, 500f) };
            base.Components.Add(BackPanel);

            NoUpgradesText = new TextSprite(Config.StatusFont, "No Upgrades Available", new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.5f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.3f));
            Components.Add(NoUpgradesText);

            TowerName = new TextSprite(Config.StatusFont, "", new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.5f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.2f)) { Color = Color.DarkGoldenrod };
            base.Components.Add(TowerName);

            SellButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.35f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.8f)) { Color = Config.MenuButtonColor };
            SellButton.Style = buttonStyle;
            SellButton.OnRelease += (x, y) => { if (Visible == true && ButtonCanClick) { Visible = false; Enabled = false; if (OnSellTower != null) { OnSellTower(SelectedTower); } } };
            base.Components.Add(SellButton);

            SellSprite = new TextSprite(Config.StatusFont, "Sell", SellButton.Position) { Color = Color.White };
            base.Components.Add(SellSprite);

            TargetButton = new ButtonMultiToggle(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.65f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.7f), 3) { Color = Config.MenuButtonColor };
            TargetButton.Style = buttonStyle;
            TargetButton.Styles = new List<Style>() { buttonStyle };
            TargetButton.OnToggled += (x, y) =>
            {
                TargetText.Text = targetTypes[TargetButton.CurrentIndex];
                selectedTower.AttackTarget = (AttackTarget)Enum.Parse(typeof(AttackTarget), targetTypes[TargetButton.CurrentIndex]);
            };
            Components.Add(TargetButton);

            TargetText = new TextSprite(Config.StatusFont, "First", TargetButton.Position);
            Components.Add(TargetText);

            RangeButton = new ButtonToggle(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.35f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.7f)) { Color = Config.MenuButtonColor };
            RangeButton.Style = buttonStyle;
            RangeButton.OffStyle = buttonStyle;
            RangeButton.On = false;
            RangeButton.OnStyle = new Style(EdgeGame.GetTexture(Config.ButtonClickTexture),Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture),Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture),Config.MenuButtonColor);
            RangeButton.OnRelease += (x, y) =>
            {
                selectedTower.ShowRadius = !selectedTower.ShowRadius;
            };
            Components.Add(RangeButton);

            RangeText = new TextSprite(Config.StatusFont, "Show Radius", RangeButton.Position);
            Components.Add(RangeText);

            CloseButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.65f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.8f)) { Color = Config.MenuButtonColor };
            CloseButton.Style = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);
            CloseButton.OnRelease += (x, y) => { if (this.Visible == true && ButtonCanClick) { this.Visible = false; this.Enabled = false; } };
            base.Components.Add(CloseButton);

            CloseSprite = new TextSprite(Config.StatusFont, "Close", CloseButton.Position) { Color = Color.White };
            base.Components.Add(CloseSprite);
        }

        public override void UpdateObject(GameTime gameTime)
        {
            base.UpdateObject(gameTime);
            foreach (Button upgradeButton in UpgradeButtons)
            {
                upgradeButton.Update(gameTime);
            }
            foreach (Sprite upgradeSprite in UpgradeSprites)
            {
                upgradeSprite.Update(gameTime);
            }
            ButtonCanClick = true;
        }

        public override void DrawObject(GameTime gameTime)
        {
            base.DrawObject(gameTime);
            foreach (Button upgradeButton in UpgradeButtons)
            {
                upgradeButton.Draw(gameTime);
            }
            foreach (Sprite upgradeSprite in UpgradeSprites)
            {
                upgradeSprite.Draw(gameTime);
            }
        }

        public void Enable(Tower tower)
        {
            SelectedTower = tower;
            Enabled = true;
            Visible = true;
            ButtonCanClick = false;
        }
    }
}
